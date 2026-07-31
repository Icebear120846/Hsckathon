using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private Transform slotContainer;
        [SerializeField] private InventorySlotUI slotPrefab;

        private readonly List<InventorySlotUI> spawnedSlots = new List<InventorySlotUI>();

        private void OnEnable()
        {
            if (inventory == null)
            {
                Debug.LogError("InventoryView: ยังไม่ได้ใส่ InventoryManager", this);
                return;
            }

            inventory.InventoryChanged += Rebuild;
            inventory.SelectionChanged += RefreshSelection;
            Rebuild();
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.InventoryChanged -= Rebuild;
                inventory.SelectionChanged -= RefreshSelection;
            }
        }

        private void Rebuild()
        {
            for (int i = 0; i < spawnedSlots.Count; i++)
            {
                if (spawnedSlots[i] != null)
                {
                    Destroy(spawnedSlots[i].gameObject);
                }
            }

            spawnedSlots.Clear();

            if (slotContainer == null || slotPrefab == null || inventory == null)
            {
                return;
            }

            for (int i = 0; i < inventory.Items.Count; i++)
            {
                InventorySlotUI slot = Instantiate(slotPrefab, slotContainer);
                slot.Setup(inventory, inventory.Items[i]);
                spawnedSlots.Add(slot);
            }
        }

        private void RefreshSelection(ItemData selectedItem)
        {
            for (int i = 0; i < spawnedSlots.Count; i++)
            {
                spawnedSlots[i].RefreshSelection(selectedItem);
            }
        }
    }
}
