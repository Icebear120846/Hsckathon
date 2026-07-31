using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class ThemeImageSlot : MonoBehaviour
    {
        [SerializeField] private string slotId = "ROOM_FRONT";
        [SerializeField] private Image targetImage;

        public string SlotId => slotId;

        public void Apply(Sprite sprite)
        {
            if (targetImage == null)
            {
                targetImage = GetComponent<Image>();
            }

            if (targetImage != null && sprite != null)
            {
                targetImage.sprite = sprite;
            }
        }
    }
}
