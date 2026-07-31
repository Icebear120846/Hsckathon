using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class CollectionProgressUI : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private GameStateManager state;
        [SerializeField] private List<ItemData> trackedItems = new List<ItemData>();
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private string label = "ชิ้นส่วน";
        [SerializeField] private string completionFlagId;
        [SerializeField] private string completedText = "ประกอบสำเร็จ";

        private void OnEnable()
        {
            if (inventory != null)
            {
                inventory.InventoryChanged += Refresh;
            }

            if (state != null)
            {
                state.FlagChanged += OnFlagChanged;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.InventoryChanged -= Refresh;
            }

            if (state != null)
            {
                state.FlagChanged -= OnFlagChanged;
            }
        }

        private void OnFlagChanged(string flagId, bool value)
        {
            if (flagId == completionFlagId)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            if (progressText == null)
            {
                return;
            }

            if (state != null && !string.IsNullOrWhiteSpace(completionFlagId) && state.HasFlag(completionFlagId))
            {
                progressText.text = completedText;
                return;
            }

            int found = 0;
            for (int i = 0; i < trackedItems.Count; i++)
            {
                if (inventory != null && inventory.Contains(trackedItems[i]))
                {
                    found++;
                }
            }

            progressText.text = $"{label} {found}/{trackedItems.Count}";
        }
    }
}
