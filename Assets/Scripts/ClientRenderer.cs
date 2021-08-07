using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRenderer
{
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
    //private List<BaseUnit> visibleUnits;
    //private List<BaseUnit> unitsOnScreen;
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

    private Mesh mesh;
    public ClientRenderer(Material material)
    {
        this.material = material;
        Setup();

    }
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
        worldBounds = new Bounds(Vector3.zero, new Vector3(World.GetWidth() * 10, World.GetHeight() * 10));
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
        //visibleUnits.AddRange(Mapgenerator.activeUnits.Keys);
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
        meshAmount = (screenW * screenH) + unitMeshes;

        int propertiesIndex = 0;
        for (int x = -screenW / 2; x < screenW / 2; x++)
        {
            for (int y = -screenH / 2; y < screenH / 2; y++)
            {
                MeshProperties props = new MeshProperties();
                
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
        meshPropertiesBuffer.SetData(properties);
        material.SetBuffer("_Properties", meshPropertiesBuffer);
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, worldBounds, argsBuffer);
    }
}
