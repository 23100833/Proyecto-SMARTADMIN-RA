using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSystem : MonoBehaviour
{
    [Header("Configuración de Ruta")]
    [Tooltip("Arrastra aquí tus 36 Image Targets en orden secuencial (001 al 036)")]
    public List<Transform> waypoints = new List<Transform>();

    [Tooltip("Arrastra aquí el objeto ARCamera de Vuforia")]
    public Transform usuarioCamara;

    [Tooltip("Distancia en metros para considerar que el usuario llegó al punto actual")]
    public float distanciaUmbral = 1.5f;

    [Header("Elementos de Guía")]
    [Tooltip("Arrastra aquí el objeto 3D de la flecha")]
    public GameObject flechaGuia;

    [Tooltip("Arrastra aquí el Canvas o Panel UI emergente de la meta")]
    public GameObject panelDestino;

    // Variables de control de estado del software
    private int currentWaypointIndex = 0;
    private int waypointMetaFinal = 35; // Por defecto es el elemento 35 (Target 036)
    private bool navegacionActiva = false;

    void Start()
    {
        // El sistema inicia apagado hasta que el usuario elija un destino en el menú
        if (flechaGuia != null) flechaGuia.SetActive(false);
        if (panelDestino != null) panelDestino.SetActive(false);
    }

    void Update()
    {
        // Patrón de seguridad de software: abortar si la navegación no está activa o faltan componentes
        if (!navegacionActiva || usuarioCamara == null || waypoints.Count == 0 || flechaGuia == null)
            return;

        // 1. Control del ciclo de vida de la ruta y detección de llegada a la meta
        if (currentWaypointIndex > waypointMetaFinal)
        {
            TerminarNavegacion();
            return;
        }

        // 2. Obtener el Transform del objetivo actual
        Transform puntoObjetivo = waypoints[currentWaypointIndex];

        if (puntoObjetivo != null)
        {
            // 3. Lógica matemática de vectores: Calcular dirección hacia el siguiente hito
            Vector3 direccion = puntoObjetivo.position - usuarioCamara.position;

            // Mantener la flecha horizontal para evitar que apunte de forma extraña hacia arriba o abajo
            direccion.y = 0;

            if (direccion.magnitude > 0.1f)
            {
                // Rotar la flecha de forma suave usando Quaternions hacia el vector calculado
                flechaGuia.transform.rotation = Quaternion.LookRotation(direccion);
            }

            // 4. Calcular distancia euclidiana en el espacio local de Vuforia
            float distanciaAlPunto = Vector3.Distance(usuarioCamara.position, puntoObjetivo.position);

            // Si el alumno entra en el rango de tolerancia (Umbral), avanzar de nodo
            if (distanciaAlPunto <= distanciaUmbral)
            {
                currentWaypointIndex++;

                // Si el incremento causó llegar al final del destino seleccionado
                if (currentWaypointIndex > waypointMetaFinal)
                {
                    TerminarNavegacion();
                }
            }
        }
    }

    /// <summary>
    /// API Pública para conectar con los botones de tu Menú UI de ESAN.
    /// </summary>
    /// <param name="indexMeta">El índice del Target final (Ej: Comedor = 10, Bienestar = 27, EdificioA = 35)</param>
    public void IniciarRutaHaciaDestino(int indexMeta)
    {
        // Validar límites del arreglo para evitar excepciones de tipo IndexOutOfRangeException
        if (indexMeta < 0 || indexMeta >= waypoints.Count)
        {
            Debug.LogError("Error en Sistemas: El índice de meta está fuera del rango del lote de fotos.");
            return;
        }

        // Inicializar variables de estado
        currentWaypointIndex = 0;
        waypointMetaFinal = indexMeta;
        navegacionActiva = true;

        // Control de visibilidad de componentes
        if (flechaGuia != null) flechaGuia.SetActive(true);
        if (panelDestino != null) panelDestino.SetActive(false);

        Debug.Log("Navegación inicializada de forma óptima hacia el nodo: " + indexMeta);
    }

    /// <summary>
    /// Encapsulamiento del proceso de parada y despliegue de interfaz de meta.
    /// </summary>
    private void TerminarNavegacion()
    {
        navegacionActiva = false;
        if (flechaGuia != null) flechaGuia.SetActive(false);
        if (panelDestino != null) panelDestino.SetActive(true); // Pop-up en pantalla activa

        Debug.Log("Meta alcanzada con éxito. Desplegando Panel Informativo.");
    }
}