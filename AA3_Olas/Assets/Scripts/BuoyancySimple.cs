// File: BuoyancySimple.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calcula la flotabilidad de un objeto (cuboide o cilindro vertical) 
/// utilizando la altura de la malla de agua generada por MeshWater.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BuoyancySimple : MonoBehaviour
{
    [Header("Parámetros de la boya")]
    [Tooltip("Densidad del fluido (kg/m³). Ej: agua dulce ~1000.")]
    public float fluidDensity = 1000f;

    [Tooltip("Gravedad positiva (m/s²). Ej: 9.81.")]
    public float gravity = 9.81f;

    [Tooltip("Altura total de la boya (en unidades).")]
    public float objectHeight = 1f;

    [Tooltip("Área de la base de la boya (en unidades²). Ej: cubo 1×1×1 → base 1.")]
    public float baseArea = 1f;

    [Header("Referencia a la malla de agua")]
    [Tooltip("Asigna aquí el componente MeshWater del objeto Water.")]
    public MeshWater waterMesh;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (waterMesh == null)
        {
            Debug.LogError("BuoyancySimple: debes asignar el MeshWater en el Inspector.");
        }
    }

    private void FixedUpdate()
    {
        if (waterMesh == null) return;

        // 1) Obtener posición world de la boya
        Vector3 worldPos = transform.position;

        // 2) Convertir a coordenadas locales de la malla (Water)
        Vector3 localPos = waterMesh.transform.InverseTransformPoint(worldPos);

        // 3) Interpolar bilinealmente la altura del agua en (localPos.x, localPos.z)
        float waterLocalY = SampleWaterHeight(localPos.x, localPos.z);

        // 4) Transformar esa altura local a world Y
        Vector3 waterLocalPoint = new Vector3(0f, waterLocalY, 0f);
        float waterWorldY = waterMesh.transform.TransformPoint(waterLocalPoint).y;

        // 5) Calcular profundidad sumergida
        float bottomY = worldPos.y - objectHeight / 2f;
        float submergedDepth = waterWorldY - bottomY;
        if (submergedDepth <= 0f) return; // no hay inmersión

        // Se limita a la altura máxima de la boya
        float h = Mathf.Min(submergedDepth, objectHeight);

        // 6) Volumen desplazado = áreaBase * h
        float displacedVolume = baseArea * h;

        // 7) Fuerza de flotación = ρ·g·Volumen
        float buoyantForce = fluidDensity * gravity * displacedVolume;

        // 8) Aplicar fuerza vertical al Rigidbody
        rb.AddForce(Vector3.up * buoyantForce, ForceMode.Force);
    }

    /// <summary>
    /// Muestra una línea de depuración que indica de dónde está muestreando la altura del agua.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (waterMesh == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f,
                        transform.position - Vector3.up * 2f);
    }

    /// <summary>
    /// Interpolación bilineal simple para obtener la componente Y del agua
    /// en un punto (localX, localZ) dentro de la malla de MeshWater.
    /// </summary>
    private float SampleWaterHeight(float localX, float localZ)
    {
        float width = waterMesh.width;
        float height = waterMesh.height;
        int xSize = waterMesh.xSize;
        int ySize = waterMesh.ySize;

        // Normalizamos localX, localZ de [-width/2..+width/2] a [0..width]
        float normX = (localX + width / 2f) / width;
        float normZ = (localZ + height / 2f) / height;

        // Índice flotante en la grilla
        float fx = normX * xSize;
        float fz = normZ * ySize;

        int ix = Mathf.FloorToInt(fx);
        int iz = Mathf.FloorToInt(fz);

        // Clamp para no pasarnos de los bordes
        ix = Mathf.Clamp(ix, 0, xSize - 1);
        iz = Mathf.Clamp(iz, 0, ySize - 1);

        float tx = fx - ix; // fracción en X
        float tz = fz - iz; // fracción en Z

        Vector3[] verts = waterMesh.mesh.vertices;
        int rowStride = xSize + 1;

        int iBL = iz * rowStride + ix;         // bottom-left
        int iBR = iBL + 1;                     // bottom-right
        int iTL = iBL + rowStride;             // top-left
        int iTR = iTL + 1;                     // top-right

        float hBL = verts[iBL].y;
        float hBR = verts[iBR].y;
        float hTL = verts[iTL].y;
        float hTR = verts[iTR].y;

        // Interpolación bilineal
        float hB = Mathf.Lerp(hBL, hBR, tx);
        float hT = Mathf.Lerp(hTL, hTR, tx);
        float hFinal = Mathf.Lerp(hB, hT, tz);

        return hFinal;
    }
}
