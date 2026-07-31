using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    public sealed class TimedRevealSequence : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField, Min(0f)] private float delayBeforeReveal = 1.5f;

        [Header("Presentation")]
        [SerializeField] private AudioClip startSfx;
        [SerializeField] private DialogueData dialogueAfterReveal;

        [Header("Scene Objects")]
        [SerializeField] private List<GameObject> activateAfterDelay = new();
        [SerializeField] private List<GameObject> deactivateAfterDelay = new();

        private bool hasPlayed;
        private Coroutine runningCoroutine;

        private void OnEnable()
        {
            if (hasPlayed)
            {
                return;
            }

            runningCoroutine = StartCoroutine(PlaySequence());
        }

        private IEnumerator PlaySequence()
        {
            hasPlayed = true;

            GameContext context = GameContext.Instance;
            if (context == null)
            {
                Debug.LogError(
                    $"{nameof(TimedRevealSequence)}: äÁè¾º GameContext",
                    this
                );
                yield break;
            }

            if (startSfx != null && context.Audio != null)
            {
                context.Audio.PlaySfx(startSfx);
            }

            if (delayBeforeReveal > 0f)
            {
                yield return new WaitForSecondsRealtime(delayBeforeReveal);
            }

            SetObjectsActive(deactivateAfterDelay, false);
            SetObjectsActive(activateAfterDelay, true);

            if (dialogueAfterReveal != null)
            {
                context.Dialogue.Show(dialogueAfterReveal);
            }

            runningCoroutine = null;
        }

        private static void SetObjectsActive(
            IReadOnlyList<GameObject> objects,
            bool active
        )
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].SetActive(active);
                }
            }
        }

        private void OnDisable()
        {
            if (runningCoroutine == null)
            {
                return;
            }

            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }
    }
}