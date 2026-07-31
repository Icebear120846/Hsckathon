using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    [Serializable]
    public sealed class ThemeSpriteEntry
    {
        [SerializeField] private string slotId;
        [SerializeField] private Sprite sprite;

        public string SlotId => slotId;
        public Sprite Sprite => sprite;
    }

    [CreateAssetMenu(fileName = "THEME_Current", menuName = "Point & Click/Game Theme")]
    public sealed class GameThemeData : ScriptableObject
    {
        [SerializeField] private string gameTitle = "POINT & CLICK TEMPLATE";
        [SerializeField] private Color primaryColor = Color.white;
        [SerializeField] private Color accentColor = Color.gray;
        [SerializeField] private Sprite logo;
        [SerializeField] private Sprite menuBackground;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private List<ThemeSpriteEntry> roomSprites = new List<ThemeSpriteEntry>();

        public string GameTitle => gameTitle;
        public Color PrimaryColor => primaryColor;
        public Color AccentColor => accentColor;
        public Sprite Logo => logo;
        public Sprite MenuBackground => menuBackground;
        public AudioClip BackgroundMusic => backgroundMusic;
        public IReadOnlyList<ThemeSpriteEntry> RoomSprites => roomSprites;

        public Sprite GetRoomSprite(string slotId)
        {
            for (int i = 0; i < roomSprites.Count; i++)
            {
                ThemeSpriteEntry entry = roomSprites[i];
                if (entry != null && entry.SlotId == slotId)
                {
                    return entry.Sprite;
                }
            }

            return null;
        }
    }
}
