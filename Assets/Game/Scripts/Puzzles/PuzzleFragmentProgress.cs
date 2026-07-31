using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PointClickTemplate
{
    /// <summary>
    /// เก็บความคืบหน้าชิ้นส่วนภาพข้าม Scene ภายในหนึ่งรอบการเปิดเกม
    /// เช่น Gameplay -> MainMenu -> Gameplay โดยไม่ต้องแก้ InventoryManager เดิม
    ///
    /// หมายเหตุ: ค่าเริ่มต้นไม่ได้บันทึกข้ามการปิดโปรแกรมหรือหยุด Play Mode
    /// </summary>
    public sealed class PuzzleFragmentProgress : MonoBehaviour
    {
        public static PuzzleFragmentProgress Instance { get; private set; }

        [Header("Puzzle Fragment Data")]
        [SerializeField] private List<ItemData> fragmentItems = new();
        [SerializeField] private ItemData completedImageItem;

        [Header("Behaviour")]
        [SerializeField] private bool restoreItemsToGameplayInventory = true;

        private readonly HashSet<string> collectedFragmentIds =
            new(StringComparer.Ordinal);

        private InventoryManager boundInventory;
        private bool completedImageCollected;
        private bool suppressInventoryCallback;

        public event Action<int, int, bool> ProgressChanged;

        public int TotalFragmentCount => fragmentItems.Count;

        public int CollectedFragmentCount => completedImageCollected
            ? TotalFragmentCount
            : collectedFragmentIds.Count;

        public bool IsCompleted => completedImageCollected;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // DontDestroyOnLoad ใช้ได้แน่นอนเมื่อ Object เป็น Root
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            BindToCurrentGameplayScene();
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            UnbindInventory();
            Instance = null;
        }

        public bool HasCollected(ItemData fragmentItem)
        {
            if (fragmentItem == null)
            {
                return false;
            }

            if (completedImageCollected)
            {
                return fragmentItems.Contains(fragmentItem);
            }

            return collectedFragmentIds.Contains(fragmentItem.ItemId);
        }

        /// <summary>
        /// ใช้สำหรับทดสอบหรือทำปุ่ม New Game ในอนาคต
        /// </summary>
        public void ResetProgress()
        {
            suppressInventoryCallback = true;

            if (boundInventory != null)
            {
                for (int i = 0; i < fragmentItems.Count; i++)
                {
                    ItemData fragment = fragmentItems[i];
                    if (fragment != null)
                    {
                        boundInventory.RemoveItem(fragment);
                    }
                }

                if (completedImageItem != null)
                {
                    boundInventory.RemoveItem(completedImageItem);
                }
            }

            collectedFragmentIds.Clear();
            completedImageCollected = false;
            suppressInventoryCallback = false;

            NotifyProgressChanged();
        }

        [ContextMenu("Reset Puzzle Fragment Progress")]
        private void ResetProgressFromContextMenu()
        {
            ResetProgress();
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BindToCurrentGameplayScene();
        }

        private void BindToCurrentGameplayScene()
        {
            UnbindInventory();

            GameContext context = GameContext.Instance;
            if (context == null || context.Inventory == null)
            {
                // MainMenu ไม่มี GameContext ถือว่าเป็นเรื่องปกติ
                return;
            }

            boundInventory = context.Inventory;
            boundInventory.InventoryChanged += HandleInventoryChanged;

            if (restoreItemsToGameplayInventory)
            {
                RestoreProgressToInventory();
            }

            CaptureProgressFromInventory();
        }

        private void UnbindInventory()
        {
            if (boundInventory != null)
            {
                boundInventory.InventoryChanged -= HandleInventoryChanged;
            }

            boundInventory = null;
        }

        private void HandleInventoryChanged()
        {
            if (suppressInventoryCallback)
            {
                return;
            }

            CaptureProgressFromInventory();
        }

        private void CaptureProgressFromInventory()
        {
            if (boundInventory == null)
            {
                return;
            }

            bool changed = false;

            for (int i = 0; i < fragmentItems.Count; i++)
            {
                ItemData fragment = fragmentItems[i];
                if (fragment == null || !boundInventory.Contains(fragment))
                {
                    continue;
                }

                changed |= collectedFragmentIds.Add(fragment.ItemId);
            }

            if (!completedImageCollected &&
                completedImageItem != null &&
                boundInventory.Contains(completedImageItem))
            {
                completedImageCollected = true;
                changed = true;
            }

            if (changed)
            {
                NotifyProgressChanged();
            }
        }

        private void RestoreProgressToInventory()
        {
            if (boundInventory == null)
            {
                return;
            }

            suppressInventoryCallback = true;

            if (completedImageCollected)
            {
                if (completedImageItem != null &&
                    !boundInventory.Contains(completedImageItem))
                {
                    boundInventory.AddItem(completedImageItem);
                }
            }
            else
            {
                for (int i = 0; i < fragmentItems.Count; i++)
                {
                    ItemData fragment = fragmentItems[i];
                    if (fragment == null ||
                        !collectedFragmentIds.Contains(fragment.ItemId) ||
                        boundInventory.Contains(fragment))
                    {
                        continue;
                    }

                    boundInventory.AddItem(fragment);
                }
            }

            suppressInventoryCallback = false;
        }

        private void NotifyProgressChanged()
        {
            ProgressChanged?.Invoke(
                CollectedFragmentCount,
                TotalFragmentCount,
                completedImageCollected
            );
        }
    }
}
