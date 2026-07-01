using System.Collections;
using UnityEngine;

namespace SmartAdminRA.Navigation
{
    public class GPSNavigator : MonoBehaviour
    {
        [Header("Configuración GPS")]
        [SerializeField] private float latitudDestino = -12.104408f;
        [SerializeField] private float longitudDestino = -76.961912f;
        [SerializeField] private float distanciaUmbralLlegada = 10f;

        [Header("Elementos Visuales AR")]
        [SerializeField] private Transform flechaAR;
        [SerializeField] private Transform usuarioCamara;

        private bool servicioInicializado = false;

        private void OnEnable()
        {
            StartCoroutine(IniciarServicioUbicacion());
        }

        private void OnDisable()
        {
            Input.location.Stop();
            servicioInicializado = false;
        }

        private IEnumerator IniciarServicioUbicacion()
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.LogWarning("[GPSNavigator] El GPS del celular está apagado o no tiene permisos.");
                yield break;
            }

            Input.location.Start(5f, 5f);

            int maxEspera = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxEspera > 0)
            {
                yield return new WaitForSeconds(1);
                maxEspera--;
            }

            if (maxEspera < 1 || Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("[GPSNavigator] No se pudo inicializar el servicio de GPS.");
                yield break;
            }

            servicioInicializado = true;
        }

        private void Update()
        {
            if (!servicioInicializado || usuarioCamara == null || flechaAR == null) return;

            float latitudActual = Input.location.lastData.latitude;
            float longitudActual = Input.location.lastData.longitude;

            float distancia = CalcularDistanciaMetros(latitudActual, longitudActual, latitudDestino, longitudDestino);

            if (distancia < distanciaUmbralLlegada)
            {
                enabled = false;
                return;
            }

            Vector3 direccionDestino = new Vector3(longitudDestino - longitudActual, 0, latitudDestino - latitudActual);
            if (direccionDestino != Vector3.zero)
            {
                flechaAR.rotation = Quaternion.LookRotation(direccionDestino);
            }
        }

        private float CalcularDistanciaMetros(float lat1, float lon1, float lat2, float lon2)
        {
            float R = 6371000f;
            float dLat = (lat2 - lat1) * Mathf.Deg2Rad;
            float dLon = (lon2 - lon1) * Mathf.Deg2Rad;
            float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                      Mathf.Cos(lat1 * Mathf.Deg2Rad) * Mathf.Cos(lat2 * Mathf.Deg2Rad) *
                      Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
            float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            return R * c;
        }

        public void ConfigurarDestino(float lat, float lon)
        {
            latitudDestino = lat;
            longitudDestino = lon;
        }
    }
}
