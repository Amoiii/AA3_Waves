// File: GerstnerWaveSimple.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulación de ola de Gerstner sencilla (solo una onda, dirección +X).
/// Desplaza X, Y según la fórmula de Gerstner (Z se mantiene igual).
/// </summary>
[RequireComponent(typeof(MeshWater))]
public class GerstnerWaveSimple : MonoBehaviour
{
    [Header("Parámetros de la ola de Gerstner")]
    [Tooltip("Amplitud máxima (altura) de la ola.")]
    public float amplitude = 1f;

    [Tooltip("Longitud de onda L (en unidades). Debe ser > 0.")]
    public float wavelength = 5f;

    [Tooltip("Velocidad de propagación sobre X (unidades/segundo).")]
    public float speed = 2f;

    [Tooltip("Fase inicial (en radianes).")]
    public float phase = 0f;

    private MeshWater meshWater;
    private Mesh mesh;
    private Vector3[] displacedVerts;
    private float elapsedTime = 0f;

    private void Start()
    {
        meshWater = GetComponent<MeshWater>();
        mesh = meshWater.mesh;

        displacedVerts = new Vector3[mesh.vertexCount];
        mesh.vertices.CopyTo(displacedVerts, 0);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        Vector3[] baseVerts = meshWater.baseVertices;
        float k = 2f * Mathf.PI / wavelength;       // número de onda
        float ω = k * speed;                        // ω = k·v

        for (int i = 0; i < baseVerts.Length; i++)
        {
            Vector3 p0 = baseVerts[i]; // (x0, y0=0, z0)
            float x0 = p0.x;
            float z0 = p0.z;

            float arg = k * (x0 - speed * elapsedTime) + phase;
            float cosA = Mathf.Cos(arg);
            float sinA = Mathf.Sin(arg);

            // Gerstner: X' = x0 + A·cos(arg), Y' = A·sin(arg), Z' = z0
            float newX = x0 + amplitude * cosA;
            float newY = amplitude * sinA;
            float newZ = z0;

            displacedVerts[i] = new Vector3(newX, newY, newZ);
        }

        mesh.vertices = displacedVerts;
        mesh.RecalculateNormals();
    }
}
