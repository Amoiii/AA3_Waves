using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AA3_GerstnerMeshRenderer : MonoBehaviour
{
    MeshFilter mf;

    [Header("Dimensiones de la malla")]
    public float width = 5f;   
    public float height = 5f;  
    [Min(2)] public int xSize = 10;    
    [Min(2)] public int ySize = 10;     

    [Header("Referencias")]
    public AA3_GerstnerWaves waves;     
    public Transform buoyRender;        

    private Mesh mesh;

    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();                 
        mf.sharedMesh = mesh;
    }

    private void Update()
    {
        if (waves == null)
            return;

        // 1) Actualizar la simulación de olas Gerstner
        waves.Update(Time.deltaTime);

        // 2) Reemplazar los vértices del mesh con las posiciones (X, Y, Z)
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
            
            buoyRender.position = waves.buoy.position.ToUnity();
        }
    }

    public Mesh Create()
    {
        Mesh newMesh = new Mesh
        {
            name = "Gerstner Water Mesh"
        };

        int totalVerts = (xSize + 1) * (ySize + 1);
        Vector3[] vertices = new Vector3[totalVerts];
        waves.points = new AA3_GerstnerWaves.Vertex[totalVerts];

        // Rellenar vértices 
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

        // Construir los triángulos
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
