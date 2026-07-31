using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class InventoryManager : MonoBehaviour
    {
        [SerializeField] private int capacity = 12;

        private readonly List<ItemData> items = new List<ItemData>();

        public event Action InventoryChanged;
        public event Action<ItemData> SelectionChanged;

        public IReadOnlyList<ItemData> Items => items;
        public ItemData SelectedItem { get; private set; }

        public bool AddItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("InventoryManager: พยายามเพิ่ม Item ที่เป็น Null", this);
                return false;
            }

            if (items.Count >= capacity)
            {
                Debug.LogWarning($"Inventory เต็ม ไม่สามารถเพิ่ม {item.DisplayName}", this);
                return false;
            }

            if (Contains(item))
            {
                return false;
            }

            items.Add(item);
            InventoryChanged?.Invoke();
            return true;
        }

        public bool RemoveItem(ItemData item)
        {
            if (item == null)
            {
                return false;
            }

            bool removed = items.Remove(item);
            if (!removed)
            {
                return false;
            }

            if (SelectedItem == item)
            {
                SelectItem(null);
            }

            InventoryChanged?.Invoke();
            return true;
        }

        public bool Contains(ItemData item)
        {
            return item != null && items.Contains(item);
        }

        public bool ContainsAll(IReadOnlyList<ItemData> requiredItems)
        {
            if (requiredItems == null || requiredItems.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < requiredItems.Count; i++)
            {
                if (requiredItems[i] == null || !Contains(requiredItems[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void SelectItem(ItemData item)
        {
            if (item != null && !Contains(item))
            {
                Debug.LogWarning("ไม่สามารถเลือก Item ที่ไม่ได้อยู่ใน Inventory", this);
                return;
            }

            SelectedItem = SelectedItem == item ? null : item;
            SelectionChanged?.Invoke(SelectedItem);
        }

        public void ClearSelection()
        {
            if (SelectedItem == null)
            {
                return;
            }

            SelectedItem = null;
            SelectionChanged?.Invoke(null);
        }

        public void ClearInventory()
        {
            items.Clear();
            SelectedItem = null;
            InventoryChanged?.Invoke();
            SelectionChanged?.Invoke(null);
        }
    }
}
