using UnityEngine;

namespace SmartAdminRA.Navigation
{
    public class ArrowController : MonoBehaviour
    {
        [Header("Configuración de Flotación")]
        [SerializeField] private float velocidadFlotacion = 3f;
        [SerializeField] private float amplitudFlotacion = 0.15f;
        [SerializeField] private float desfase = 0f;

        private float alturaInicialY;

        private void Awake()
        {
            // Guardamos únicamente la altura local original en Y para usarla como punto de equilibrio
            alturaInicialY = transform.localPosition.y;
        }

        private void Update()
        {
            // Obtenemos la posición local actual (que el WaypointSystem modifica al mover el objeto)
            Vector3 pos = transform.localPosition;

            // Aplicamos el bounce matemático basándonos en su altura base local
            pos.y = alturaInicialY + Mathf.Sin(Time.time * velocidadFlotacion + desfase) * amplitudFlotacion;

            // Asignamos la posición de vuelta sin alterar los ejes X ni Z globales/locales
            transform.localPosition = pos;
        }
    }
}