using UnityEngine;

namespace SmartAdminRA.Navigation
{
    public class GuideController : MonoBehaviour
    {
        [Header("Componente")]
        [SerializeField] private Animator animator;

        [Header("Parámetros del Animator Controller")]
        [SerializeField] private string boolCaminar = "isWalking";
        [SerializeField] private string triggerSenalar = "pointForward";
        [SerializeField] private string triggerCelebrar = "celebrate";

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogWarning("[GuideController] No se encontró un componente Animator en este objeto.");
                }
            }
        }

        // Activa caminar al navegar
        public void SetCaminando(bool estaCaminando)
        {
            if (animator != null)
                animator.SetBool(boolCaminar, estaCaminando);
        }

        // Señala en waypoints
        public void DispararSenalar()
        {
            if (animator != null)
                animator.SetTrigger(triggerSenalar);
        }

        // Celebra al llegar al destino final
        public void DispararCelebrar()
        {
            if (animator != null)
                animator.SetTrigger(triggerCelebrar);
        }
    }
}