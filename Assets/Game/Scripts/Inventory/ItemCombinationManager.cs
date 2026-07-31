using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class ItemCombinationManager : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private List<ItemCombinationRecipe> recipes = new List<ItemCombinationRecipe>();

        private bool isProcessing;

        private void OnEnable()
        {
            if (inventory == null)
            {
                Debug.LogError("ItemCombinationManager: ยังไม่ได้ใส่ InventoryManager", this);
                return;
            }

            inventory.InventoryChanged += CheckAutomaticRecipes;
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.InventoryChanged -= CheckAutomaticRecipes;
            }
        }

        public bool TryCombine(ItemCombinationRecipe recipe)
        {
            if (recipe == null || inventory == null)
            {
                return false;
            }

            GameContext context = GameContext.Instance;
            if (context == null)
            {
                Debug.LogError("ItemCombinationManager: ไม่พบ GameContext", this);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(recipe.CompletionFlagId) && context.State.HasFlag(recipe.CompletionFlagId))
            {
                return false;
            }

            if (!inventory.ContainsAll(recipe.RequiredItems))
            {
                return false;
            }

            isProcessing = true;

            if (recipe.ConsumeIngredients)
            {
                for (int i = 0; i < recipe.RequiredItems.Count; i++)
                {
                    inventory.RemoveItem(recipe.RequiredItems[i]);
                }
            }

            if (recipe.ResultItem != null)
            {
                inventory.AddItem(recipe.ResultItem);
            }

            if (!string.IsNullOrWhiteSpace(recipe.CompletionFlagId))
            {
                context.State.SetFlag(recipe.CompletionFlagId);
            }

            if (recipe.CompletionSfx != null && context.Audio != null)
            {
                context.Audio.PlaySfx(recipe.CompletionSfx);
            }

            if (recipe.CompletionDialogue != null)
            {
                context.Dialogue.Show(recipe.CompletionDialogue);
            }

            isProcessing = false;
            return true;
        }

        private void CheckAutomaticRecipes()
        {
            if (isProcessing)
            {
                return;
            }

            for (int i = 0; i < recipes.Count; i++)
            {
                ItemCombinationRecipe recipe = recipes[i];
                if (recipe != null && recipe.CombineAutomatically && TryCombine(recipe))
                {
                    // หนึ่งครั้งต่อการเปลี่ยน Inventory เพื่อลดการยิง Event ซ้อน
                    break;
                }
            }
        }
    }
}
