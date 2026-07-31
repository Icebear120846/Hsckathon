using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PointClickTemplate
{
    /// <summary>
    /// ใช้กับ Hotspot โปร่งใสหรือวัตถุ UI ที่ผู้เล่นคลิกได้
    /// รองรับตรวจสอบ, รับไอเทม, ใช้ไอเทม, เปิด View และเรียกผลลัพธ์ผ่าน Inspector
    /// </summary>
    public sealed class InteractableObject : MonoBehaviour, IPointerClickHandler
    {
        [Header("Identity")]
        [SerializeField] private string interactableId = "INTERACTABLE_NEW";

        [Header("Requirements")]
        [SerializeField] private List<string> requiredFlags = new List<string>();
        [SerializeField] private ItemData requiredSelectedItem;
        [SerializeField] private bool consumeRequiredItem = true;

        [Header("Behaviour")]
        [SerializeField] private bool oneTime = true;
        [SerializeField] private bool disableRaycastAfterComplete = true;
        [SerializeField] private string openViewIdWithoutCompleting;

        [Header("Dialogue")]
        [SerializeField] private DialogueData normalDialogue;
        [SerializeField] private DialogueData lockedDialogue;
        [SerializeField] private DialogueData noItemSelectedDialogue;
        [SerializeField] private DialogueData wrongItemDialogue;
        [SerializeField] private DialogueData alreadyCompletedDialogue;

        [Header("Success")]
        [SerializeField] private OutcomeActions successActions = new OutcomeActions();

        private bool completed;

        public string InteractableId => interactableId;
        public bool IsCompleted => completed;

        public void OnPointerClick(PointerEventData eventData)
        {
            Interact();
        }

        public void Interact()
        {
            GameContext context = GameContext.Instance;
            if (context == null)
            {
                Debug.LogError($"{name}: ไม่พบ GameContext", this);
                return;
            }

            if (completed && oneTime)
            {
                ShowIfAvailable(context, alreadyCompletedDialogue);
                return;
            }

            if (!context.State.HasAllFlags(requiredFlags))
            {
                ShowIfAvailable(context, lockedDialogue);
                return;
            }

            if (!string.IsNullOrWhiteSpace(openViewIdWithoutCompleting) && requiredSelectedItem == null)
            {
                context.RoomViews.OpenView(openViewIdWithoutCompleting, true);
                ShowIfAvailable(context, normalDialogue);
                return;
            }

            if (requiredSelectedItem != null)
            {
                ItemData selectedItem = context.Inventory.SelectedItem;

                if (selectedItem == null)
                {
                    ShowIfAvailable(context, noItemSelectedDialogue != null ? noItemSelectedDialogue : normalDialogue);
                    return;
                }

                if (selectedItem != requiredSelectedItem)
                {
                    ShowIfAvailable(context, wrongItemDialogue);
                    return;
                }

                bool shouldConsume = consumeRequiredItem && !requiredSelectedItem.Reusable;
                if (shouldConsume)
                {
                    context.Inventory.RemoveItem(requiredSelectedItem);
                }
                else
                {
                    context.Inventory.ClearSelection();
                }
            }

            Complete(context);
        }

        private void Complete(GameContext context)
        {
            if (oneTime)
            {
                completed = true;
            }

            successActions.Execute(context);

            if (oneTime && disableRaycastAfterComplete)
            {
                Graphic graphic = GetComponent<Graphic>();
                if (graphic != null)
                {
                    graphic.raycastTarget = false;
                }
            }
        }

        private static void ShowIfAvailable(GameContext context, DialogueData dialogue)
        {
            if (dialogue != null)
            {
                context.Dialogue.Show(dialogue);
            }
        }
    }
}
