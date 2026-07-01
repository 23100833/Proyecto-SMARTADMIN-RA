using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SmartAdminRA.Navigation
{
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

        private int currentWaypointIndex = 0;
        private int waypointMetaFinal = 34;
        private bool navegacionActiva = false;

        void Start()
        {
            if (flechaGuia != null) flechaGuia.SetActive(false);
            if (panelDestino != null) panelDestino.SetActive(false);
        }

        void Update()
        {
            if (!navegacionActiva || usuarioCamara == null || waypoints.Count == 0 || flechaGuia == null)
                return;

            if (currentWaypointIndex > waypointMetaFinal)
            {
                TerminarNavegacion();
                return;
            }

            Transform puntoObjetivo = waypoints[currentWaypointIndex];

            if (puntoObjetivo != null)
            {
                Vector3 direccion = puntoObjetivo.position - usuarioCamara.position;

                direccion.y = 0;

                if (direccion.magnitude > 0.1f)
                {
                    flechaGuia.transform.rotation = Quaternion.LookRotation(direccion);
                }

                float distanciaAlPunto = Vector3.Distance(usuarioCamara.position, puntoObjetivo.position);

                if (distanciaAlPunto <= distanciaUmbral)
                {
                    currentWaypointIndex++;

                    if (currentWaypointIndex > waypointMetaFinal)
                    {
                        TerminarNavegacion();
                    }
                }
            }
        }

        public void IniciarRutaHaciaDestino(int indexMeta)
        {
            if (indexMeta < 0 || indexMeta >= waypoints.Count)
            {
                Debug.LogError("[WaypointSystem] El índice de meta está fuera del rango de waypoints.");
                return;
            }

            currentWaypointIndex = 0;
            waypointMetaFinal = indexMeta;
            navegacionActiva = true;

            if (flechaGuia != null) flechaGuia.SetActive(true);
            if (panelDestino != null) panelDestino.SetActive(false);
        }

        private void TerminarNavegacion()
        {
            navegacionActiva = false;
            if (flechaGuia != null) flechaGuia.SetActive(false);
            if (panelDestino != null) panelDestino.SetActive(true);
        }
    }
}
