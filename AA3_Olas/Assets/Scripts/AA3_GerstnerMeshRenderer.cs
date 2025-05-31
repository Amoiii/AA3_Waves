using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AA3_GerstnerMeshRenderer : MonoBehaviour
{
    MeshFilter mf;

    [Header("Dimensiones de la malla")]
    public float width = 5f;   // ancho de la malla
    public float height = 5f;  // largo de la malla
    [Min(2)] public int xSize = 10;     // subdivisiones en X (mínimo 2)
    [Min(2)] public int ySize = 10;     // subdivisiones en Z (mínimo 2)

    [Header("Referencias")]
    public AA3_GerstnerWaves waves;     // Lógica de las olas Gerstner
    public Transform buoyRender;        // GameObject que renderiza la boya (por ejemplo, una esfera)

    private Mesh mesh;

    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();                 // Crear la malla plana subdividida
        mf.sharedMesh = mesh;
    }

    private void Update()
    {
        if (waves == null)
            return;

        // 1) Actualizar la simulación de olas Gerstner
        waves.Update(Time.deltaTime);

        // 2) Reemplazar los vértices del mesh con las posiciones deformadas (X, Y, Z)
        Vector3[] vertices = new Vector3[waves.points.Length];
        for (int i = 0; i < waves.points.Length; i++)
        {
            vertices[i] = waves.points[i].position.ToUnity();
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();

        // 3) Sincronizar el GameObject de la boya (buoyRender) para que siga la posición calculada
        if (buoyRender != null)
        {
            // waves.buoy.position está en Vector3C, lo convertimos a Vector3 Unity
            buoyRender.position = waves.buoy.position.ToUnity();
        }
    }

    /// <summary>
    /// Genera una malla rectangular subdividida en xSize×ySize,
    /// inicializa waves.points con la posición “en reposo” de cada vértice.
    /// </summary>
    public Mesh Create()
    {
        Mesh newMesh = new Mesh
        {
            name = "Gerstner Water Mesh"
        };

        int totalVerts = (xSize + 1) * (ySize + 1);
        Vector3[] vertices = new Vector3[totalVerts];
        waves.points = new AA3_GerstnerWaves.Vertex[totalVerts];

        // Rellenar el array de vértices / puntos originales
        for (int i = 0, z = 0; z <= ySize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float posX = x * width / xSize - width * 0.5f;
                float posZ = z * height / ySize - height * 0.5f;
                Vector3 worldPos = new Vector3(posX, 0f, posZ);

                vertices[i] = worldPos;
                waves.points[i] = new AA3_GerstnerWaves.Vertex(worldPos.ToCustom());
            }
        }

        newMesh.vertices = vertices;

        // Construir los triángulos (dos por cada cuadrado)
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, z = 0; z < ySize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti + 0] = vi;
                triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        newMesh.triangles = triangles;
        newMesh.RecalculateNormals();
        newMesh.MarkDynamic();
        return newMesh;
    }

    private void OnDrawGizmosSelected()
    {
        if (waves != null)
            waves.Debug(); // Dibuja gizmos de puntos originales y deformados, y la boya
    }
}
