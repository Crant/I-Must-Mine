using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class ClientRenderer
{
    public ClientRenderer(Material material, Material tileMat)
    {
        this.material = material;
        memerial = tileMat;
        Setup();

    }

    private struct MeshProperties
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return sizeof(float) * 4 * 4 + // matrix
                sizeof(float) * 4;         // color 
        }
    }

    private int meshAmount;
    private List<BaseUnit> visibleUnits;
    private List<BaseUnit> unitsOnScreen;
    //private PlayerUnit player;

    private const int pixelSizeX = 16;
    private const int pixelSizeY = 16;
    private int unitMeshes = 0;

    private int screenW = Screen.width / pixelSizeX;
    private int screenH = Screen.height / pixelSizeY;
    private Vector2 offset;
    private Bounds worldBounds;

    private ComputeBuffer meshPropertiesBuffer;
    private ComputeBuffer argsBuffer;

    private MeshProperties[] properties;

    private Material material;
    private Material memerial;

    private Mesh mesh;

    //private void Start()
    //{
    //    player = GetComponent<PlayerUnit>();
    //    Setup();
    //}
    //private void Update()
    //{
    //    UpdateBuffer();
    //}
    private void Setup()
    {
        //unitsOnScreen = new List<BaseUnit>();
        //visibleUnits = new List<BaseUnit>();
        
        CreateMesh();
        InitBuffer();
        worldBounds = new Bounds(Vector3.zero, new Vector3(World.GetWidth() * 2, World.GetHeight() * 2));
    }
    private void CreateMesh()
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 1);
        vertices[1] = new Vector3(1, 1);
        vertices[2] = new Vector3(0, 0);
        vertices[3] = new Vector3(1, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    private void InitBuffer()
    {
        screenW = Camera.main.scaledPixelWidth / pixelSizeX;
        screenH = Camera.main.scaledPixelHeight / pixelSizeY;

        meshAmount = screenW * screenH + unitMeshes;

        properties = new MeshProperties[meshAmount];

        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

        args[0] = (uint)mesh.GetIndexCount(0);
        args[1] = (uint)meshAmount;
        args[2] = (uint)mesh.GetIndexStart(0);
        args[3] = (uint)mesh.GetBaseVertex(0);

        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        offset = new Vector3(-(screenW / 2), -(screenH / 2));

        meshPropertiesBuffer = new ComputeBuffer(meshAmount, MeshProperties.Size());
    }
    public void UpdateBuffer() // ÄNDRA VARFÖR FIXEDUPDATE FUCKAR MED SCANLINES
    {
        //unitsOnScreen.Clear();
        //visibleUnits.Clear();

        //for (int i = 0; i < visibleUnits.Count; i++)
        //{
        //    if (Mapgenerator.activeUnits[visibleUnits[i]].x >= player.GetPosition().x + offset.x &&
        //        Mapgenerator.activeUnits[visibleUnits[i]].y >= player.GetPosition().y + offset.y &&
        //        Mapgenerator.activeUnits[visibleUnits[i]].x < player.GetPosition().x - offset.x &&
        //        Mapgenerator.activeUnits[visibleUnits[i]].y < player.GetPosition().y - offset.y)
        //    {
        //        unitMeshes += Mathf.FloorToInt(visibleUnits[i].GetSize().x * visibleUnits[i].GetSize().y);
        //        unitsOnScreen.Add(visibleUnits[i]);

        //    }
        //}
        meshAmount = (screenW * screenH) + 1;

        MeshProperties props;

        int propertiesIndex = 0;
        for (int x = -screenW / 2; x < screenW / 2; x++)
        {
            for (int y = -screenH / 2; y < screenH / 2; y++)
            {
                props = new MeshProperties();

                Vector2 pos = Vector2Int.FloorToInt(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) + new Vector2(x, y));
                props.mat = Matrix4x4.Translate(pos);
                if (World.IsBlock(pos))
                {
                    if (World.GetActiveTiles().TryGetValue(World.Index(pos), out TileData tile))
                    {
                        props.color = Color.yellow;
                    }
                    else
                    {
                        props.color = Color.black;
                    }
                }
                else
                    props.color = Color.white;

                properties[propertiesIndex] = props;
                propertiesIndex++;
            }
        }
        //for (int i = 0; i < unitsOnScreen.Count; i++)
        //{
        //    for (int x = 0; x < unitsOnScreen[i].GetSize().x; x++)
        //    {
        //        for (int y = 0; y < unitsOnScreen[i].GetSize().y; y++)
        //        {
        //            MeshProperties props = new MeshProperties();

        //            props.mat = Matrix4x4.Translate(unitsOnScreen[i].GetPosition() + Mapgenerator.Coordinates(Mapgenerator.Index(x, y)));
        //            props.color = Color.cyan;
        //            properties[propertiesIndex] = props;
        //            propertiesIndex++;
        //        }
        //    }
        //}
        props = new MeshProperties();

        props.mat = Matrix4x4.Translate(UnitController.activeUnits[0].GetPosition());
        props.color = Color.red;
        properties[propertiesIndex] = props;
        propertiesIndex++;

        meshPropertiesBuffer.SetData(properties);
        material.SetBuffer("_Properties", meshPropertiesBuffer);
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, worldBounds, argsBuffer);
        //Graphics.DrawMeshInstancedProcedural(mesh, 0, material, worldBounds, meshAmount);
    }
    private void TestRender()
    {

    }

    // NY SKIT HÄR UNDER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    //int blockSize = 32;
    //public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
    //{
    //    return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    //}
    //public void TestingInit()
    //{
    //    screenW = Camera.main.scaledPixelWidth / pixelSizeX;
    //    screenH = Camera.main.scaledPixelHeight / pixelSizeY;

    //    meshAmount = screenW * screenH;

    //    properties = new MeshProperties[meshAmount];

    //    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    //    args[0] = (uint)mesh.GetIndexCount(0);
    //    args[1] = (uint)meshAmount;
    //    args[2] = (uint)mesh.GetIndexStart(0);
    //    args[3] = (uint)mesh.GetBaseVertex(0);

    //    argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    //    argsBuffer.SetData(args);

    //    offset = new Vector3(-(screenW / 2), -(screenH / 2));

    //    Debug.Log(meshAmount);
    //    meshPropertiesBuffer = new ComputeBuffer(meshAmount, MeshProperties.Size());
    //    coolMap = new NativeMultiHashMap<int, Matrix4x4>(10, Allocator.Persistent);

    //}
    //NativeMultiHashMap<int, Matrix4x4> coolMap;
    //Matrix4x4[] retardArray;

    //public void RenderScreen() // ÄNDRA VARFÖR FIXEDUPDATE FUCKAR MED SCANLINES
    //{
    //    coolMap.Clear();
    //    for (int x = -screenW / 2; x < screenW / 2; x++)
    //    {
    //        for (int y = -screenH / 2; y < screenH / 2; y++)
    //        {

    //            Vector2 pos = Vector2Int.FloorToInt(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) + new Vector2(x, y));
    //            coolMap.Add(World.GetTile(pos), Matrix4x4.Translate(pos));
    //        }
    //    }


    //    NativeKeyValueArrays<int, Matrix4x4> dude = coolMap.GetKeyValueArrays(Allocator.Temp);
    //    int nigger = dude.Keys.Unique();


    //    for (int i = 0; i < nigger; i++)
    //    {
    //        GraphicsBuffer bufer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, coolMap.CountValuesForKey(i), 1 * args.Length * sizeof(uint));
    //        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, worldBounds, bufer);

    //    }
    //    dude.Dispose();
    //    //Debug.Log(dude.Keys + " keys length " + dude.Values.Length);
    //    //Graphics.DrawMeshInstanced(mesh, 0 , material, dude.)
    //    // Graphics.DrawMeshInstanced(mesh, 0 , memerial)
    //}

    ////int blockSize = 32;
    ////public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
    ////{
    ////    return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    ////}
    //private ComputeBuffer positionBuffer;
    //void UpdateBuffers()
    //{
    //    // Ensure submesh index is in range

    //    // Positions
    //    if (positionBuffer != null)
    //        positionBuffer.Release();
    //    positionBuffer = new ComputeBuffer(meshAmount, 16);
    //    Vector4[] positions = new Vector4[meshAmount];
    //    for (int i = 0; i < meshAmount; i++)
    //    {
    //        float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
    //        float distance = Random.Range(20.0f, 100.0f);
    //        float height = Random.Range(-2.0f, 2.0f);
    //        float size = Random.Range(0.05f, 0.25f);
    //        positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
    //    }
    //    positionBuffer.SetData(positions);
    //    instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

    //    // Indirect args
    //    if (instanceMesh != null)
    //    {
    //        args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
    //        args[1] = (uint)instanceCount;
    //        args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
    //        args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);
    //    }
    //    else
    //    {
    //        args[0] = args[1] = args[2] = args[3] = 0;
    //    }
    //    argsBuffer.SetData(args);

    //    cachedInstanceCount = instanceCount;
    //    cachedSubMeshIndex = subMeshIndex;
    //}
}
