using UnityEngine;

namespace DebugTool
{
    public class CheatCodes : MonoBehaviour
    {
        // Référence au GameObject auquel vous voulez accéder depuis le code
        [field: SerializeField] public Player player { get; private set; }

        // Méthode utilitaire pour appeler depuis le code évalué
        public float ModifyPlayerSpeed(float amount)
        {
            if (player != null)
            {
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    player.Speed = amount;
                }
            }
            return player.Speed; 
        }
    }
}