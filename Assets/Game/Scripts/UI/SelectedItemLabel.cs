using TMPro;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class SelectedItemLabel : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private TMP_Text label;
        [SerializeField] private string emptyText = "ยังไม่ได้เลือกไอเทม";

        private void OnEnable()
        {
            if (inventory != null)
            {
                inventory.SelectionChanged += Refresh;
                Refresh(inventory.SelectedItem);
            }
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.SelectionChanged -= Refresh;
            }
        }

        private void Refresh(ItemData item)
        {
            if (label != null)
            {
                label.text = item == null ? emptyText : $"เลือก: {item.DisplayName}";
            }
        }
    }
}
