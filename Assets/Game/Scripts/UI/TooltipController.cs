using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class TooltipController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;

        private void Awake()
        {
            Hide();
        }

        public void ShowItem(ItemData item)
        {
            if (item == null)
            {
                Hide();
                return;
            }

            if (panel != null) panel.SetActive(true);
            if (titleText != null) titleText.text = item.DisplayName;
            if (descriptionText != null) descriptionText.text = item.Description;
            if (iconImage != null)
            {
                iconImage.sprite = item.Icon;
                iconImage.gameObject.SetActive(item.Icon != null);
            }
        }

        public void Hide()
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}
