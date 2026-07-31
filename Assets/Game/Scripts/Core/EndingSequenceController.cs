using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class EndingSequenceController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject sequenceRoot;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private GameObject toBeContinuedRoot;
        [SerializeField] private GameObject creditsRoot;
        [SerializeField] private RectTransform creditsContent;
        [SerializeField] private Button skipButton;

        [Header("Navigation")]
        [SerializeField] private SceneNavigator sceneNavigator;
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        [Header("Timing")]
        [SerializeField, Min(0.01f)] private float fadeDuration = 1.5f;
        [SerializeField, Min(0f)] private float titleDuration = 2.5f;
        [SerializeField, Min(0.01f)] private float creditsDuration = 15f;
        [SerializeField, Min(0f)] private float skipAvailableAfter = 0.5f;

        [Header("Credits Movement")]
        [SerializeField]
        private Vector2 creditsStartPosition =
            new(0f, -700f);

        [SerializeField]
        private Vector2 creditsEndPosition =
            new(0f, 1000f);

        private Coroutine runningCoroutine;
        private bool isRunning;

        private void Awake()
        {
            HideImmediately();

            if (skipButton != null)
            {
                skipButton.onClick.RemoveListener(SkipToMainMenu);
                skipButton.onClick.AddListener(SkipToMainMenu);
            }
        }

        private void OnDestroy()
        {
            if (skipButton != null)
            {
                skipButton.onClick.RemoveListener(SkipToMainMenu);
            }
        }

        public void BeginEnding()
        {
            if (isRunning)
            {
                return;
            }

            if (sequenceRoot == null || fadeCanvasGroup == null)
            {
                Debug.LogError(
                    "EndingSequenceController: ยังเชื่อม UI ไม่ครบ",
                    this
                );
                return;
            }

            Time.timeScale = 1f;
            isRunning = true;

            sequenceRoot.SetActive(true);

            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.interactable = true;

            if (toBeContinuedRoot != null)
            {
                toBeContinuedRoot.SetActive(false);
            }

            if (creditsRoot != null)
            {
                creditsRoot.SetActive(false);
            }

            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(false);
            }

            runningCoroutine = StartCoroutine(RunEnding());
            StartCoroutine(ShowSkipAfterDelay());
        }

        public void SkipToMainMenu()
        {
            if (!isRunning)
            {
                return;
            }

            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }

            LoadMainMenu();
        }

        private IEnumerator RunEnding()
        {
            yield return FadeToBlack();

            if (toBeContinuedRoot != null)
            {
                toBeContinuedRoot.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(titleDuration);

            if (toBeContinuedRoot != null)
            {
                toBeContinuedRoot.SetActive(false);
            }

            if (creditsRoot != null)
            {
                creditsRoot.SetActive(true);
            }

            yield return ScrollCredits();

            runningCoroutine = null;
            LoadMainMenu();
        }

        private IEnumerator FadeToBlack()
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                fadeCanvasGroup.alpha = Mathf.Clamp01(
                    elapsed / fadeDuration
                );

                yield return null;
            }

            fadeCanvasGroup.alpha = 1f;
        }

        private IEnumerator ScrollCredits()
        {
            if (creditsContent == null)
            {
                yield return new WaitForSecondsRealtime(creditsDuration);
                yield break;
            }

            creditsContent.anchoredPosition = creditsStartPosition;

            float elapsed = 0f;

            while (elapsed < creditsDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                float progress = Mathf.Clamp01(
                    elapsed / creditsDuration
                );

                creditsContent.anchoredPosition = Vector2.Lerp(
                    creditsStartPosition,
                    creditsEndPosition,
                    progress
                );

                yield return null;
            }

            creditsContent.anchoredPosition = creditsEndPosition;
        }

        private IEnumerator ShowSkipAfterDelay()
        {
            if (skipButton == null)
            {
                yield break;
            }

            yield return new WaitForSecondsRealtime(skipAvailableAfter);

            if (isRunning)
            {
                skipButton.gameObject.SetActive(true);
            }
        }

        private void LoadMainMenu()
        {
            if (!isRunning)
            {
                return;
            }

            isRunning = false;

            if (sceneNavigator == null)
            {
                Debug.LogError(
                    "EndingSequenceController: ไม่พบ SceneNavigator",
                    this
                );
                return;
            }

            sceneNavigator.LoadScene(mainMenuSceneName);
        }

        private void HideImmediately()
        {
            isRunning = false;

            if (sequenceRoot != null)
            {
                sequenceRoot.SetActive(false);
            }
        }
    }
}