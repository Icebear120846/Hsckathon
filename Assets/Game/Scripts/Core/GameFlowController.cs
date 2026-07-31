using UnityEngine;

namespace PointClickTemplate
{
    public sealed class GameFlowController : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject endingPanel;
        [SerializeField] private GameObject pausePanel;

        private void Start()
        {
            if (endingPanel != null) endingPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        public void ShowTutorial()
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(true);
        }

        public void CloseTutorial()
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
        }

        public void ShowEnding()
        {
            if (endingPanel != null) endingPanel.SetActive(true);
        }

        public void SetPaused(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
            if (pausePanel != null) pausePanel.SetActive(paused);
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
