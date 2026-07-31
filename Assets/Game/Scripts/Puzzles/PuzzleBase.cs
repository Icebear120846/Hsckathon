using UnityEngine;

namespace PointClickTemplate
{
    public abstract class PuzzleBase : MonoBehaviour
    {
        [Header("Puzzle")]
        [SerializeField] private string puzzleId = "PUZZLE_NEW";
        [SerializeField] private GameObject puzzlePanel;
        [SerializeField] private bool closePanelOnSuccess = true;
        [SerializeField] private OutcomeActions successActions = new OutcomeActions();

        public string PuzzleId => puzzleId;
        public bool IsSolved { get; private set; }

        public virtual void Open()
        {
            if (puzzlePanel != null)
            {
                puzzlePanel.SetActive(true);
            }
        }

        public virtual void Close()
        {
            if (puzzlePanel != null)
            {
                puzzlePanel.SetActive(false);
            }
        }

        protected void CompletePuzzle()
        {
            if (IsSolved)
            {
                return;
            }

            IsSolved = true;
            successActions.Execute(GameContext.Instance);

            if (closePanelOnSuccess)
            {
                Close();
            }
        }
    }
}
