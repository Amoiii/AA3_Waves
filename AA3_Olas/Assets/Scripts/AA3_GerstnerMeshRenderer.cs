using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AA3_GerstnerMeshRenderer : MonoBehaviour
{
    MeshFilter mf;
    public float width = 5f;   // ancho de la malla
    public float height = 5f;  // largo de la malla
    public int xSize = 10;     // subdivisiones en X (mínimo 2)
    public int ySize = 10;     // subdivisiones en Z (mínimo 2)

    public Mesh mesh;
    public AA3_GerstnerWaves waves;

    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();
        mf.sharedMesh = mesh;
    }

    private void Update()
    {
        // Actualizar la simulación de olas
        waves.Update(Time.deltaTime);

        // Extraer los vértices deformados y asignarlos a la malla
        Vector3[] vertices = new Vector3[waves.points.Length];
        for (int i = 0; i < waves.points.Length; i++)
        {
            vertices[i] = waves.points[i].position.ToUnity();
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
        // Si deseas optimizar, puedes usar mesh.UploadMeshData(false);
    }

    /// <summary>
    /// Genera una malla rectangular plana subdividida en xSize × ySize,
    /// inicializando el array de vértices (originalPosition) para GerstnerWaves.
    /// </summary>
    public Mesh Create()
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "Gerstner Water Mesh";

        // Cantidad total de vértices: (xSize + 1) × (ySize + 1)
        Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        waves.points = new AA3_GerstnerWaves.Vertex[(xSize + 1) * (ySize + 1)];

        // Recorrer filas y columnas para colocar vértices
        for (int i = 0, z = 0; z <= ySize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float posX = x * width / xSize - width * 0.5f;
                float posZ = z * height / ySize - height * 0.5f;
                Vector3 worldPos = new Vector3(posX, 0, posZ);

                vertices[i] = worldPos;
                waves.points[i] = new AA3_GerstnerWaves.Vertex(worldPos.ToCustom());
            }
        }

        newMesh.vertices = vertices;

        // Triángulos (dos triángulos por “cuadrado”)
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
            waves.Debug();
    }
}
