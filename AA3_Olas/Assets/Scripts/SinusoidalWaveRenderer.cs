// File: SinusoidalWaveSimple.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulación de ola puramente sinusoidal que mueve solo la componente Y
/// de cada vértice, en la dirección +X, con parámetros: amplitud, L, velocidad y fase.
/// </summary>
[RequireComponent(typeof(MeshWater))]
public class SinusoidalWaveSimple : MonoBehaviour
{
    [Header("Parámetros de la ola sinusoidal")]
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

        // Pre-alocar el array de vértices
        displacedVerts = new Vector3[mesh.vertexCount];
        // Inicialmente, copiamos mesh.vertices en displacedVerts
        mesh.vertices.CopyTo(displacedVerts, 0);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Recuperar array de posiciones base (X,Z con Y=0)
        Vector3[] baseVerts = meshWater.baseVertices;

        float k = 2f * Mathf.PI / wavelength;       // número de onda
        float ω = k * speed;                        // ω = k·v  → simplifica 2π/L · v

        for (int i = 0; i < baseVerts.Length; i++)
        {
            Vector3 p0 = baseVerts[i]; // (x, y=0, z)
            float x = p0.x;

            // Desplazamiento vertical: A·sin(k·(x - speed·t) + phase)
            float y = amplitude * Mathf.Sin(k * (x - speed * elapsedTime) + phase);

            displacedVerts[i] = new Vector3(p0.x, y, p0.z);
        }

        // Asignar de nuevo al mesh:
        mesh.vertices = displacedVerts;
        mesh.RecalculateNormals();
    }
}
