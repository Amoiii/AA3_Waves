// File: MeshWater.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crea, en Awake(), una malla plana subdividida en xSize × ySize quads.
/// Guarda las posiciones base de los vértices en baseVertices para que
/// luego los scripts de ola (sinusoidal o Gerstner) las modifiquen.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class MeshWater : MonoBehaviour
{
    [Header("Dimensiones de la malla")]
    [Min(0.1f)] public float width = 10f;      // ancho total (X)
    [Min(0.1f)] public float height = 10f;     // profundidad total (Z)
    [Min(2)] public int xSize = 100;         // subdivisiones en X (quads horizontales)
    [Min(2)] public int ySize = 100;         // subdivisiones en Z (quads verticales)

    [HideInInspector] public Mesh mesh;               // el Mesh que generaré
    [HideInInspector] public Vector3[] baseVertices;  // posiciones originales de los vértices

    private MeshFilter mf;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mesh = BuildMesh();
        mf.sharedMesh = mesh;
    }

    /// <summary>
    /// Construye la malla rectangular subdividida.
    /// </summary>
    private Mesh BuildMesh()
    {
        Mesh m = new Mesh
        {
            name = "ProceduralWaterMesh"
        };

        // Cantidad total de vértices = (xSize + 1) * (ySize + 1)
        int numVerts = (xSize + 1) * (ySize + 1);
        Vector3[] vertices = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        Vector3[] normals = new Vector3[numVerts];

        // Rellenar vértices, UVs y normales
        for (int y = 0, i = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                // x va de -width/2  a +width/2
                float xPos = ((float)x / xSize - 0.5f) * width;
                // z va de -height/2 a +height/2
                float zPos = ((float)y / ySize - 0.5f) * height;
                vertices[i] = new Vector3(xPos, 0f, zPos);
                uvs[i] = new Vector2((float)x / xSize, (float)y / ySize);
                normals[i] = Vector3.up;
            }
        }

        baseVertices = vertices; // guardo la posición inicial (Y=0)

        // Construir triángulos: cada quad = 2 triángulos = 6 índices
        int[] triangles = new int[xSize * ySize * 6];
        int ti = 0;
        for (int y = 0, vi = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                // vertice bajo-izquierda = vi
                triangles[ti + 0] = vi;
                triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        m.vertices = vertices;
        m.uv = uvs;
        m.normals = normals;
        m.triangles = triangles;

        m.RecalculateBounds();
        m.MarkDynamic(); // marca el mesh como dinámico (vamos a actualizarlo cada frame)
        return m;
    }
}
