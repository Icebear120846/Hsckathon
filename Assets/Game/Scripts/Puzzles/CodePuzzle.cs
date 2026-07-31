using TMPro;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class CodePuzzle : PuzzleBase
    {
        [SerializeField] private TMP_InputField codeInput;
        [SerializeField] private string correctCode = "2413";
        [SerializeField] private DialogueData wrongCodeDialogue;
        [SerializeField] private bool clearInputWhenWrong = true;

        public void Submit()
        {
            if (IsSolved)
            {
                return;
            }

            string enteredCode = codeInput != null ? codeInput.text.Trim() : string.Empty;
            if (enteredCode == correctCode)
            {
                CompletePuzzle();
                return;
            }

            if (clearInputWhenWrong && codeInput != null)
            {
                codeInput.text = string.Empty;
            }

            if (wrongCodeDialogue != null && GameContext.Instance != null)
            {
                GameContext.Instance.Dialogue.Show(wrongCodeDialogue);
            }
        }
    }
}
