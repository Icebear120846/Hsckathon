using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    /// <summary>
    /// ชุดผลลัพธ์กลางที่ปริศนาหรือวัตถุสามารถเรียกใช้ได้
    /// ตั้งค่าผ่าน Inspector เพื่อให้เปลี่ยนธีมและ Flow โดยไม่แก้ Core Code
    /// </summary>
    [Serializable]
    public sealed class OutcomeActions
    {
        [Header("State")]
        [SerializeField] private string setFlagId;

        [Header("Reward")]
        [SerializeField] private ItemData rewardItem;
        [SerializeField] private bool preventDuplicateReward = true;

        [Header("Presentation")]
        [SerializeField] private DialogueData dialogue;
        [SerializeField] private AudioClip successSfx;

        [Header("Scene Objects")]
        [SerializeField] private List<GameObject> activateObjects = new List<GameObject>();
        [SerializeField] private List<GameObject> deactivateObjects = new List<GameObject>();

        [Header("Navigation / Flow")]
        [SerializeField] private string openRoomViewId;
        [SerializeField] private bool triggerEnding;

        public void Execute(GameContext context)
        {
            if (context == null)
            {
                Debug.LogError("OutcomeActions: ไม่พบ GameContext");
                return;
            }

            if (!string.IsNullOrWhiteSpace(setFlagId))
            {
                context.State.SetFlag(setFlagId);
            }

            if (rewardItem != null)
            {
                bool canAdd = !preventDuplicateReward || !context.Inventory.Contains(rewardItem);
                if (canAdd)
                {
                    context.Inventory.AddItem(rewardItem);
                }
            }

            for (int i = 0; i < activateObjects.Count; i++)
            {
                if (activateObjects[i] != null)
                {
                    activateObjects[i].SetActive(true);
                }
            }

            for (int i = 0; i < deactivateObjects.Count; i++)
            {
                if (deactivateObjects[i] != null)
                {
                    deactivateObjects[i].SetActive(false);
                }
            }

            if (!string.IsNullOrWhiteSpace(openRoomViewId))
            {
                context.RoomViews.OpenView(openRoomViewId, true);
            }

            if (successSfx != null && context.Audio != null)
            {
                context.Audio.PlaySfx(successSfx);
            }

            if (dialogue != null)
            {
                context.Dialogue.Show(dialogue);
            }

            if (triggerEnding)
            {
                context.Flow.ShowEnding();
            }
        }
    }
}
