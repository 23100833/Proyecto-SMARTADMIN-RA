using UnityEngine;
using System.Collections.Generic;

namespace SmartAdminRA.Navigation
{
    public class NavigationManager : MonoBehaviour
    {
        [System.Serializable]
        public class DestinoOficina
        {
            public string nombreOficina;
            public bool usarGPS;
            public int indiceWaypointFinal;
            public Vector2 coordenadasGPS;
        }

        [Header("Sistemas de Navegación")]
        [SerializeField] private GameObject sistemaGPS;
        [SerializeField] private GameObject sistemaWaypoints;

        [Header("Componentes de Soporte")]
        [SerializeField] private GPSNavigator gpsNavigator;
        [SerializeField] private WaypointSystem waypointSystem;

        [Header("Configuración de Destinos")]
        [SerializeField] private List<DestinoOficina> destinos = new List<DestinoOficina>();

        public void StartRoute(string officeName)
        {
            if (string.IsNullOrWhiteSpace(officeName))
            {
                Debug.LogWarning("[NavigationManager] No se recibió un destino válido.");
                return;
            }

            DestinoOficina destino = destinos.Find(d =>
                d.nombreOficina.Equals(officeName, System.StringComparison.OrdinalIgnoreCase));

            if (destino == null)
            {
                Debug.LogWarning($"[NavigationManager] '{officeName}' no está configurado. Usando GPS por defecto.");
                CambiarModo(true);
                return;
            }

            Debug.Log($"[NavigationManager] Ruta hacia: {officeName}");

            if (destino.usarGPS)
            {
                if (gpsNavigator != null)
                    gpsNavigator.ConfigurarDestino(destino.coordenadasGPS.x, destino.coordenadasGPS.y);

                CambiarModo(true);
            }
            else
            {
                if (waypointSystem != null)
                    waypointSystem.IniciarRutaHaciaDestino(destino.indiceWaypointFinal);

                CambiarModo(false);
            }
        }

        private void CambiarModo(bool gpsActivo)
        {
            if (sistemaGPS != null)
                sistemaGPS.SetActive(gpsActivo);

            if (gpsNavigator != null)
                gpsNavigator.enabled = gpsActivo;

            if (sistemaWaypoints != null)
                sistemaWaypoints.SetActive(!gpsActivo);
        }
    }
}
