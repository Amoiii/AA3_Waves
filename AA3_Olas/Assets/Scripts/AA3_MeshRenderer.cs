using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AA3_MeshRenderer : MonoBehaviour
{
    MeshFilter mf;

    [Header("Dimensiones de la malla")]
    [Min(1)] public float width;
    [Min(1)] public float height;
    [Min(2)] public int xSize;
    [Min(2)] public int ySize;

    [Header("Referencias")]
    public AA3_Waves waves;        // Lógica de olas sinusoidales
    public Transform buoyRender;   // GameObject que renderiza la boya (por ejemplo, una esfera)

    public Mesh mesh;

    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();            // Crear la malla plana subdividida
        mf.sharedMesh = mesh;
    }

    private void Update()
    {
        if (waves == null)
            return;

        // 1) Actualizar olas sinusoidales
        waves.Update(Time.deltaTime);

        // 2) Reemplazar vértices con posiciones deformadas (solo en Y)
        Vector3[] vertices = new Vector3[waves.points.Length];
        for (int i = 0; i < waves.points.Length; i++)
        {
            vertices[i] = waves.points[i].position.ToUnity();
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();

        // 3) Mover el GameObject que renderiza la boya
        if (buoyRender != null)
        {
            buoyRender.position = waves.buoy.position.ToUnity();
        }
    }

    /// <summary>
    /// Genera una malla subdividida en xSize×ySize y
    /// almacena en waves.points la posición original de cada vértice.
    /// </summary>
    public Mesh Create()
    {
        Mesh newmesh = new Mesh
        {
            name = "Procedural Sinusoidal Grid"
        };

        int totalVerts = (xSize + 1) * (ySize + 1);
        Vector3[] vertices = new Vector3[totalVerts];
        waves.points = new AA3_Waves.Vertex[totalVerts];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float posX = x * width / xSize - width * 0.5f;
                float posZ = y * height / ySize - height * 0.5f;
                Vector3 worldPos = new Vector3(posX, 0f, posZ);

                vertices[i] = worldPos;
                waves.points[i] = new AA3_Waves.Vertex(worldPos.ToCustom());
            }
        }

        newmesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
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

        newmesh.triangles = triangles;
        newmesh.RecalculateNormals();
        newmesh.MarkDynamic();
        return newmesh;
    }

    private void OnDrawGizmosSelected()
    {
        if (waves != null)
            waves.Debug(); // Dibuja gizmos de los vértices y la boya
    }
}
