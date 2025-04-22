using UnityEngine;

namespace TooltipsSystem
{
    [CreateAssetMenu(fileName = "New Tooltip Text", menuName = "Tooltips/Tooltip Text")]
    public class SO_TooltipText : ScriptableObject
    {
        [SerializeField, TextArea(3, 10)] private string header;

        [SerializeField, TextArea(5, 20)] private string content;

        public string Header => header;
        public string Content => content;
    }
}