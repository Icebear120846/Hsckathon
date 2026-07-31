using UnityEngine;

namespace PointClickTemplate
{
    /// <summary>
    /// จุดรวม Reference ของระบบหลักภายใน Gameplay Scene
    /// มีเพียงตัวเดียวใน Scene และช่วยลดการใช้ Find ระหว่างเล่น
    /// </summary>
    public sealed class GameContext : MonoBehaviour
    {
        public static GameContext Instance { get; private set; }

        [Header("Core Systems")]
        [SerializeField] private GameStateManager state;
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private DialogueController dialogue;
        [SerializeField] private RoomViewController roomViews;
        [SerializeField] private GameFlowController flow;
        [SerializeField] private AudioManager audioManager;

        public GameStateManager State => state;
        public InventoryManager Inventory => inventory;
        public DialogueController Dialogue => dialogue;
        public RoomViewController RoomViews => roomViews;
        public GameFlowController Flow => flow;
        public AudioManager Audio => audioManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("พบ GameContext มากกว่า 1 ตัวใน Scene", this);
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ValidateReferences();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void ValidateReferences()
        {
            if (state == null) Debug.LogError("GameContext: ยังไม่ได้ใส่ GameStateManager", this);
            if (inventory == null) Debug.LogError("GameContext: ยังไม่ได้ใส่ InventoryManager", this);
            if (dialogue == null) Debug.LogError("GameContext: ยังไม่ได้ใส่ DialogueController", this);
            if (roomViews == null) Debug.LogError("GameContext: ยังไม่ได้ใส่ RoomViewController", this);
            if (flow == null) Debug.LogError("GameContext: ยังไม่ได้ใส่ GameFlowController", this);
            if (audioManager == null) Debug.LogWarning("GameContext: ยังไม่ได้ใส่ AudioManager", this);
        }
    }
}
