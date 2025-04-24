using UnityEngine;

namespace Armory
{
    [RequireComponent(typeof(ArmoryStorage))]
    public class ArmoryTerminal : MonoBehaviour
    {
        [Tooltip("Reference to the UI controller prefab in scene")]
        [SerializeField] private ArmoryUIController uiController;

        private ArmoryStorage storage;
        private bool hasChosen;

        public bool HasChosen => hasChosen;
        public ArmoryStorage Storage => storage;

        private void Awake()
        {
            storage = GetComponent<ArmoryStorage>();
            if (uiController == null)
                Debug.LogError("ArmoryTerminal: missing UIController reference.", this);
        }

        /// <summary>
        /// Called by PlayerInteract when player presses F in front of this terminal.
        /// </summary>
        public void TryOpen()
        {
            if (hasChosen) return;
            uiController.Show(this);
        }

        /// <summary>
        /// Called by UIController once a weapon has been picked.
        /// </summary>
        public void NotifyChosen()
        {
            hasChosen = true;
        }
    }
}