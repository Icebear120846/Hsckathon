using UnityEngine;

namespace PointClickTemplate
{
    [CreateAssetMenu(fileName = "ITEM_NewItem", menuName = "Point & Click/Item Data")]
    public sealed class ItemData : ScriptableObject
    {
        [SerializeField] private string itemId = "ITEM_NEW";
        [SerializeField] private string displayName = "New Item";
        [TextArea(2, 5)]
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [Tooltip("เปิดไว้เมื่อเครื่องมือนี้ควรอยู่ต่อหลังใช้งาน เช่น ไขควง")]
        [SerializeField] private bool reusable;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Reusable => reusable;

        private void OnValidate()
        {
            itemId = itemId == null ? string.Empty : itemId.Trim();
        }
    }
}
