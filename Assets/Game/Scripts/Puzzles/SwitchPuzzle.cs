using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class SwitchPuzzle : PuzzleBase
    {
        [SerializeField] private List<Toggle> switches = new List<Toggle>();
        [SerializeField] private List<bool> correctStates = new List<bool>();
        [SerializeField] private DialogueData wrongStateDialogue;

        public void CheckAnswer()
        {
            if (IsSolved)
            {
                return;
            }

            if (switches.Count != correctStates.Count || switches.Count == 0)
            {
                Debug.LogError("SwitchPuzzle: จำนวน Toggle กับคำตอบไม่เท่ากัน", this);
                return;
            }

            for (int i = 0; i < switches.Count; i++)
            {
                if (switches[i] == null || switches[i].isOn != correctStates[i])
                {
                    if (wrongStateDialogue != null && GameContext.Instance != null)
                    {
                        GameContext.Instance.Dialogue.Show(wrongStateDialogue);
                    }
                    return;
                }
            }

            CompletePuzzle();
        }
    }
}
