using UnityEngine;

namespace SmartAdminRA.Navigation
{
    public class NavigationManager : MonoBehaviour
    {
        [Header("Sistemas de Navegación")]
        [SerializeField] private GameObject sistemaGPS;
        [SerializeField] private GameObject sistemaWaypoints;

        [Header("Componentes de Soporte")]
        [SerializeField] private GPSNavigator gpsNavigator; // Vinculamos tu nuevo script de GPS

        public void StartRoute(string officeName)
        {
            if (string.IsNullOrWhiteSpace(officeName))
            {
                Debug.LogWarning("[NavigationManager] No se recibió un destino válido.");
                return;
            }

            Debug.Log($"[NavigationManager] Solicitando ruta hacia: {officeName}");

            bool esInterior = officeName.Equals(
                "Servicios Académicos",
                System.StringComparison.OrdinalIgnoreCase);

            CambiarModo(!esInterior);
        }

        private void CambiarModo(bool gpsActivo)
        {
            Debug.Log(gpsActivo
                ? "[NavigationManager] Modo GPS Activado (Exteriores)."
                : "[NavigationManager] Modo Waypoints Activado (Interiores).");

            if (sistemaGPS != null)
                sistemaGPS.SetActive(gpsActivo);

            // Encendemos o apagamos el componente lógico del GPS según corresponda
            if (gpsNavigator != null)
                gpsNavigator.enabled = gpsActivo;

            if (sistemaWaypoints != null)
                sistemaWaypoints.SetActive(!gpsActivo);
        }
    }
}