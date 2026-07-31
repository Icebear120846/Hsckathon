using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class SequencePuzzle : PuzzleBase
    {
        [SerializeField] private List<int> correctSequence = new List<int> { 0, 2, 1, 3 };
        [SerializeField] private DialogueData wrongSequenceDialogue;

        private readonly List<int> currentSequence = new List<int>();

        public void PressButton(int buttonId)
        {
            if (IsSolved || correctSequence.Count == 0)
            {
                return;
            }

            currentSequence.Add(buttonId);
            int currentIndex = currentSequence.Count - 1;

            if (currentIndex >= correctSequence.Count || currentSequence[currentIndex] != correctSequence[currentIndex])
            {
                currentSequence.Clear();
                if (wrongSequenceDialogue != null && GameContext.Instance != null)
                {
                    GameContext.Instance.Dialogue.Show(wrongSequenceDialogue);
                }
                return;
            }

            if (currentSequence.Count == correctSequence.Count)
            {
                CompletePuzzle();
            }
        }

        public override void Close()
        {
            currentSequence.Clear();
            base.Close();
        }
    }
}
