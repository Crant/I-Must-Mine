using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renderer : MonoBehaviour
{

    private int blockWidth;
    private int blockHeight;

    private const int pixelSizeX = 16;
    private const int pixelSizeY = 16;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Color[] colors;

    private Mesh mesh;
    [SerializeField] private Material material;



    public void CreateMesh()
    {
        blockWidth = Camera.main.scaledPixelWidth / pixelSizeX;
        blockHeight = Camera.main.scaledPixelHeight / pixelSizeY;

        UpdateMesh();
    }


    public void UpdateMesh()
    {
        vertices = new Vector3[(blockWidth + 1) * (blockHeight + 1)];
        uvs = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];

        //for (int x = -blockWidth / 2, i = 0; x < blockWidth / 2; x++)
        //{
        //    for (int y = -blockHeight / 2; y < blockHeight / 2; y++)
        //    {

        //        vertices[i] = (new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) + new Vector2(x, y));
        //        i++;

        //    }
        //}
        //triangles = new int[blockWidth * blockHeight * 6];
        //for (int x = -blockWidth / 2, vert = 0, tris = 0; x < blockWidth / 2; x++)
        //{
        //    for (int y = -blockHeight / 2; y < blockHeight / 2; y++)
        //    {

        //        triangles[tris + 0] = vert + 0;
        //        triangles[tris + 1] = vert + blockHeight + 1;
        //        triangles[tris + 2] = vert + 1;
        //        triangles[tris + 3] = vert + 1;
        //        triangles[tris + 4] = vert + blockHeight + 1;
        //        triangles[tris + 5] = vert + blockHeight + 2;

        //        vert++;
        //        tris += 6;
        //    }
        //    //vert++;
        //}
        for (int x = 0, i = 0; x < blockWidth; x++)
        {
            for (int y = 0; y < blockHeight; y++)
            {

                vertices[i] = (new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) + new Vector2(x, y));
                i++;

            }
        }
        triangles = new int[blockWidth * blockHeight * 6];
        for (int x = 0, vert = 0, tris = 0; x < blockWidth; x++)
        {
            for (int y = 0; y < blockHeight; y++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + blockHeight + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + blockHeight + 1;
                triangles[tris + 5] = vert + blockHeight + 2;


                tris += 6;
                vert++;
            }

            //vert++;
        }


        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    public void DrawScreen()
    {


        for (int i = 0, z = 0; z <= blockWidth; z++)
        {
            for (int x = 0; x <= blockHeight; x++)
            {
                uvs[i] = new Vector2((float)x / blockHeight, (float)z / blockWidth);
                float tileType = Random.Range(0f, 1f);
                colors[i] = tileType < 0.5f ? Color.cyan : Color.red;

                i++;
            }
        }

        Graphics.DrawMesh(mesh, UnitController.activeUnits[0].GetPosition() - new Vector2(blockWidth / 2, blockHeight / 2), Quaternion.identity, material, 0);
    }
    //private void OnDrawGizmos()
    //{
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], .1f);
    //    }
    //}
}
