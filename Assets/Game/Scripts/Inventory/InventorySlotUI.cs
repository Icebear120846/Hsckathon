using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject selectedFrame;
        [SerializeField] private TMP_Text amountText;

        private InventoryManager inventory;
        private ItemData item;

        public void Setup(InventoryManager inventoryManager, ItemData itemData)
        {
            inventory = inventoryManager;
            item = itemData;

            if (iconImage != null)
            {
                iconImage.sprite = item != null ? item.Icon : null;
                iconImage.enabled = item != null && item.Icon != null;
            }

            if (amountText != null)
            {
                amountText.text = string.Empty;
            }

            if (button != null)
            {
                button.onClick.RemoveListener(OnClicked);
                button.onClick.AddListener(OnClicked);
                button.interactable = item != null;
            }

            RefreshSelection(inventory != null ? inventory.SelectedItem : null);
        }

        public void RefreshSelection(ItemData selectedItem)
        {
            if (selectedFrame != null)
            {
                selectedFrame.SetActive(item != null && selectedItem == item);
            }
        }

        private void OnClicked()
        {
            if (inventory != null && item != null)
            {
                inventory.SelectItem(item);
            }
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnClicked);
            }
        }
    }
}
