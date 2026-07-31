using UnityEngine;

namespace PointClickTemplate
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject howToPlayPanel;
        [SerializeField] private GameObject settingsPanel;

        private void Start()
        {
            if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
        }

        public void SetHowToPlayVisible(bool visible)
        {
            if (howToPlayPanel != null) howToPlayPanel.SetActive(visible);
        }

        public void SetSettingsVisible(bool visible)
        {
            if (settingsPanel != null) settingsPanel.SetActive(visible);
        }
    }
}
