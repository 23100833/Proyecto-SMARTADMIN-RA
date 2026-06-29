using UnityEngine;

namespace SmartAdminRA.Navigation
{
    public class WaypointSystem : MonoBehaviour
    {
        [Header("Configuración de Ruta")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform usuarioCamara;
        [SerializeField] private float distanciaUmbral = 1.5f;

        [Header("Elementos de Guía")]
        [SerializeField] private GameObject flechaGuia;

        private int indiceWaypointActual;

        private void Awake()
        {
            if (usuarioCamara == null)
                Debug.LogWarning("[WaypointSystem] No se asignó la cámara del usuario.");

            if (flechaGuia == null)
                Debug.LogWarning("[WaypointSystem] No se asignó la flecha guía.");

            if (waypoints == null || waypoints.Length == 0)
                Debug.LogWarning("[WaypointSystem] No hay waypoints configurados.");
        }

        private void OnEnable()
        {
            indiceWaypointActual = 0;

            if (flechaGuia != null)
                flechaGuia.SetActive(true);

            if (waypoints != null && waypoints.Length > 0)
                ActualizarPosicionFlecha();
        }

        private void Update()
        {
            if (!PuedeActualizar())
                return;

            RevisarWaypoint();
        }

        private bool PuedeActualizar()
        {
            return usuarioCamara != null &&
                   waypoints != null &&
                   waypoints.Length > 0 &&
                   indiceWaypointActual < waypoints.Length;
        }

        private void RevisarWaypoint()
        {
            Vector3 diferencia = usuarioCamara.position - waypoints[indiceWaypointActual].position;
            diferencia.y = 0f;

            float distancia = diferencia.magnitude;

            if (distancia <= distanciaUmbral)
            {
                Debug.Log($"[WaypointSystem] Waypoint {indiceWaypointActual} alcanzado.");

                indiceWaypointActual++;

                if (indiceWaypointActual >= waypoints.Length)
                {
                    FinalizarRuta();
                }
                else
                {
                    ActualizarPosicionFlecha();
                }
            }
        }

        private void ActualizarPosicionFlecha()
        {
            if (flechaGuia == null)
                return;

            Transform flecha = flechaGuia.transform;

            flecha.position = waypoints[indiceWaypointActual].position;

            if (indiceWaypointActual + 1 < waypoints.Length)
            {
                Vector3 objetivo = waypoints[indiceWaypointActual + 1].position;
                objetivo.y = flecha.position.y;

                flecha.LookAt(objetivo);
            }
        }

        private void FinalizarRuta()
        {
            Debug.Log("[WaypointSystem] ¡Has llegado a la oficina de destino!");

            if (flechaGuia != null)
                flechaGuia.SetActive(false);

            // Aquí podrá llamarse más adelante al sistema de gamificación.

            enabled = false;
        }
    }
}