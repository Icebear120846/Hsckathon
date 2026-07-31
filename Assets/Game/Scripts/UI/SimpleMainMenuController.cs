using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PointClickTemplate
{
    /// <summary>
    /// ควบคุมหน้า Main Menu แบบเรียบง่าย
    /// รองรับเริ่มเกม เปิด/ปิดวิธีเล่น และออกจากเกม
    /// </summary>
    public sealed class SimpleMainMenuController : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private string gameplaySceneName = "Gameplay";

        [Header("Panels")]
        [SerializeField] private GameObject mainMenuRoot;
        [SerializeField] private GameObject howToPlayPanel;
        [SerializeField] private GameObject loadingIndicator;

        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button howToPlayButton;
        [SerializeField] private Button closeHowToPlayButton;
        [SerializeField] private Button quitButton;

        private bool isLoading;

        private void Awake()
        {
            Time.timeScale = 1f;

            if (mainMenuRoot != null)
            {
                mainMenuRoot.SetActive(true);
            }

            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(false);
            }

            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }

            RegisterButtonEvents();
        }

        private void OnDestroy()
        {
            UnregisterButtonEvents();
        }

        public void StartGame()
        {
            if (isLoading)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(gameplaySceneName))
            {
                Debug.LogError(
                    "SimpleMainMenuController: Gameplay Scene Name ยังว่างอยู่",
                    this
                );
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(gameplaySceneName))
            {
                Debug.LogError(
                    $"SimpleMainMenuController: ไม่พบ Scene ชื่อ '{gameplaySceneName}' " +
                    "ใน Build Profiles > Scene List",
                    this
                );
                return;
            }

            isLoading = true;
            SetButtonsInteractable(false);

            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
        }

        public void ShowHowToPlay()
        {
            if (isLoading)
            {
                return;
            }

            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(true);
            }
        }

        public void HideHowToPlay()
        {
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(false);
            }
        }

        public void QuitGame()
        {
            if (isLoading)
            {
                return;
            }

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void RegisterButtonEvents()
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(StartGame);
            }

            if (howToPlayButton != null)
            {
                howToPlayButton.onClick.AddListener(ShowHowToPlay);
            }

            if (closeHowToPlayButton != null)
            {
                closeHowToPlayButton.onClick.AddListener(HideHowToPlay);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(QuitGame);
            }
        }

        private void UnregisterButtonEvents()
        {
            if (startButton != null)
            {
                startButton.onClick.RemoveListener(StartGame);
            }

            if (howToPlayButton != null)
            {
                howToPlayButton.onClick.RemoveListener(ShowHowToPlay);
            }

            if (closeHowToPlayButton != null)
            {
                closeHowToPlayButton.onClick.RemoveListener(HideHowToPlay);
            }

            if (quitButton != null)
            {
                quitButton.onClick.RemoveListener(QuitGame);
            }
        }

        private void SetButtonsInteractable(bool interactable)
        {
            if (startButton != null)
            {
                startButton.interactable = interactable;
            }

            if (howToPlayButton != null)
            {
                howToPlayButton.interactable = interactable;
            }

            if (quitButton != null)
            {
                quitButton.interactable = interactable;
            }
        }
    }
}
