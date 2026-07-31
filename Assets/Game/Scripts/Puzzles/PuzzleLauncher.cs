using UnityEngine;
using UnityEngine.EventSystems;

namespace PointClickTemplate
{
    public sealed class PuzzleLauncher : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PuzzleBase puzzle;

        public void OnPointerClick(PointerEventData eventData)
        {
            OpenPuzzle();
        }

        public void OpenPuzzle()
        {
            if (puzzle != null)
            {
                puzzle.Open();
            }
        }
    }
}
