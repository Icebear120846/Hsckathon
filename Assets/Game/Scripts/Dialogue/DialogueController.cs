using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class DialogueController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button closeButton;

        private DialogueData currentDialogue;
        private int lineIndex;

        private void Awake()
        {
            if (nextButton != null) nextButton.onClick.AddListener(Next);
            if (closeButton != null) closeButton.onClick.AddListener(Close);
            if (panel != null) panel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (nextButton != null) nextButton.onClick.RemoveListener(Next);
            if (closeButton != null) closeButton.onClick.RemoveListener(Close);
        }

        public void Show(DialogueData dialogue)
        {
            if (dialogue == null || dialogue.Lines == null || dialogue.Lines.Count == 0)
            {
                return;
            }

            currentDialogue = dialogue;
            lineIndex = 0;
            if (panel != null) panel.SetActive(true);
            DisplayCurrentLine();
        }

        public void Next()
        {
            if (currentDialogue == null)
            {
                Close();
                return;
            }

            lineIndex++;
            if (lineIndex >= currentDialogue.Lines.Count)
            {
                Close();
                return;
            }

            DisplayCurrentLine();
        }

        public void Close()
        {
            currentDialogue = null;
            lineIndex = 0;
            if (panel != null) panel.SetActive(false);
        }

        private void DisplayCurrentLine()
        {
            DialogueLine line = currentDialogue.Lines[lineIndex];

            if (speakerNameText != null)
            {
                speakerNameText.text = line.SpeakerName;
                speakerNameText.gameObject.SetActive(!string.IsNullOrWhiteSpace(line.SpeakerName));
            }

            if (messageText != null)
            {
                messageText.text = line.Message;
            }

            if (portraitImage != null)
            {
                portraitImage.sprite = line.Portrait;
                portraitImage.gameObject.SetActive(line.Portrait != null);
            }

            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(lineIndex < currentDialogue.Lines.Count - 1);
            }

            if (closeButton != null)
            {
                closeButton.gameObject.SetActive(lineIndex >= currentDialogue.Lines.Count - 1);
            }
        }
    }
}
