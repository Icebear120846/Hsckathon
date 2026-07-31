using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PointClickTemplate
{
    public sealed class ThemeApplier : MonoBehaviour
    {
        [SerializeField] private GameThemeData theme;
        [SerializeField] private TMP_Text gameTitleText;
        [SerializeField] private Image logoImage;
        [SerializeField] private Image menuBackgroundImage;
        [SerializeField] private List<Graphic> primaryColorGraphics = new List<Graphic>();
        [SerializeField] private List<Graphic> accentColorGraphics = new List<Graphic>();
        [SerializeField] private List<ThemeImageSlot> roomImageSlots = new List<ThemeImageSlot>();
        [SerializeField] private AudioManager audioManager;

        private void Start()
        {
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (theme == null)
            {
                Debug.LogWarning("ThemeApplier: ยังไม่ได้ใส่ GameThemeData", this);
                return;
            }

            if (gameTitleText != null) gameTitleText.text = theme.GameTitle;
            if (logoImage != null && theme.Logo != null) logoImage.sprite = theme.Logo;
            if (menuBackgroundImage != null && theme.MenuBackground != null) menuBackgroundImage.sprite = theme.MenuBackground;

            for (int i = 0; i < primaryColorGraphics.Count; i++)
            {
                if (primaryColorGraphics[i] != null) primaryColorGraphics[i].color = theme.PrimaryColor;
            }

            for (int i = 0; i < accentColorGraphics.Count; i++)
            {
                if (accentColorGraphics[i] != null) accentColorGraphics[i].color = theme.AccentColor;
            }

            for (int i = 0; i < roomImageSlots.Count; i++)
            {
                ThemeImageSlot slot = roomImageSlots[i];
                if (slot == null) continue;
                Sprite sprite = theme.GetRoomSprite(slot.SlotId);
                if (sprite != null) slot.Apply(sprite);
            }

            if (audioManager != null && theme.BackgroundMusic != null)
            {
                audioManager.PlayMusic(theme.BackgroundMusic);
            }
        }
    }
}
