using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    [CreateAssetMenu(fileName = "RECIPE_NewCombination", menuName = "Point & Click/Item Combination Recipe")]
    public sealed class ItemCombinationRecipe : ScriptableObject
    {
        [SerializeField] private string recipeId = "RECIPE_NEW";
        [SerializeField] private List<ItemData> requiredItems = new List<ItemData>();
        [SerializeField] private ItemData resultItem;
        [SerializeField] private bool consumeIngredients = true;
        [SerializeField] private bool combineAutomatically = true;
        [SerializeField] private string completionFlagId;
        [SerializeField] private DialogueData completionDialogue;
        [SerializeField] private AudioClip completionSfx;

        public string RecipeId => recipeId;
        public IReadOnlyList<ItemData> RequiredItems => requiredItems;
        public ItemData ResultItem => resultItem;
        public bool ConsumeIngredients => consumeIngredients;
        public bool CombineAutomatically => combineAutomatically;
        public string CompletionFlagId => completionFlagId;
        public DialogueData CompletionDialogue => completionDialogue;
        public AudioClip CompletionSfx => completionSfx;
    }
}
