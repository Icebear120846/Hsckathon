#if UNITY_EDITOR
using System;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PointClickTemplate;

namespace PointClickTemplate.EditorTools
{
    /// <summary>
    /// สร้างหน้า Main Menu แบบเรียบง่ายให้อัตโนมัติใน Scene ที่กำลังเปิดอยู่
    /// เมนู: Tools > Point & Click > สร้าง Main Menu แบบเรียบง่าย
    /// </summary>
    public static class SimpleMainMenuBuilder
    {
        private const string RootName = "SimpleMainMenu";

        [MenuItem("Tools/Point & Click/สร้าง Main Menu แบบเรียบง่าย")]
        private static void BuildSimpleMainMenu()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (!activeScene.IsValid())
            {
                EditorUtility.DisplayDialog(
                    "สร้าง Main Menu ไม่สำเร็จ",
                    "ไม่พบ Scene ที่กำลังเปิดอยู่",
                    "ตกลง"
                );
                return;
            }

            if (!string.Equals(activeScene.name, "MainMenu", StringComparison.Ordinal))
            {
                bool continueBuild = EditorUtility.DisplayDialog(
                    "Scene ปัจจุบันไม่ใช่ MainMenu",
                    $"ตอนนี้กำลังเปิด Scene ชื่อ '{activeScene.name}'\n\n" +
                    "แนะนำให้เปิด MainMenu.unity ก่อนสร้าง ต้องการสร้างต่อหรือไม่?",
                    "สร้างต่อ",
                    "ยกเลิก"
                );

                if (!continueBuild)
                {
                    return;
                }
            }

            GameObject oldRoot = GameObject.Find(RootName);
            if (oldRoot != null)
            {
                bool replace = EditorUtility.DisplayDialog(
                    "พบ Main Menu เดิม",
                    "พบ Object ชื่อ SimpleMainMenu อยู่แล้ว\n" +
                    "ต้องการลบของเดิมแล้วสร้างใหม่หรือไม่?",
                    "สร้างใหม่",
                    "ยกเลิก"
                );

                if (!replace)
                {
                    return;
                }

                Undo.DestroyObjectImmediate(oldRoot);
            }

            GameObject root = CreateGameObject(RootName, null, typeof(RectTransform));
            RectTransform rootRect = root.GetComponent<RectTransform>();
            StretchFull(rootRect);

            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            root.AddComponent<GraphicRaycaster>();

            Image background = CreateImage(
                "Background",
                root.transform,
                new Color(0.055f, 0.06f, 0.075f, 1f)
            );
            StretchFull(background.rectTransform);
            background.raycastTarget = false;

            Image menuPanel = CreateImage(
                "MainMenuRoot",
                root.transform,
                new Color(0.10f, 0.11f, 0.14f, 0.96f)
            );
            SetRect(menuPanel.rectTransform, new Vector2(680f, 720f), Vector2.zero);

            TextMeshProUGUI title = CreateText(
                "GameTitle",
                menuPanel.transform,
                "POINT & CLICK",
                72f,
                FontStyles.Bold,
                TextAlignmentOptions.Center
            );
            SetRect(title.rectTransform, new Vector2(600f, 110f), new Vector2(0f, 245f));

            TextMeshProUGUI subtitle = CreateText(
                "Subtitle",
                menuPanel.transform,
                "DEMO",
                28f,
                FontStyles.Normal,
                TextAlignmentOptions.Center
            );
            subtitle.color = new Color(0.72f, 0.74f, 0.82f, 1f);
            SetRect(subtitle.rectTransform, new Vector2(500f, 50f), new Vector2(0f, 175f));

            Button startButton = CreateButton(
                "StartButton",
                menuPanel.transform,
                "เริ่มเกม",
                new Vector2(0f, 70f)
            );

            Button howToPlayButton = CreateButton(
                "HowToPlayButton",
                menuPanel.transform,
                "วิธีเล่น",
                new Vector2(0f, -45f)
            );

            Button quitButton = CreateButton(
                "QuitButton",
                menuPanel.transform,
                "ออกจากเกม",
                new Vector2(0f, -160f)
            );

            TextMeshProUGUI themeHint = CreateText(
                "ThemeHint",
                menuPanel.transform,
                "ภาพ ชื่อเกม สี และฟอนต์ เปลี่ยนตามธีมวันแข่งขันได้",
                22f,
                FontStyles.Italic,
                TextAlignmentOptions.Center
            );
            themeHint.color = new Color(0.6f, 0.62f, 0.7f, 1f);
            SetRect(themeHint.rectTransform, new Vector2(580f, 70f), new Vector2(0f, -285f));

            Image howToPlayPanel = CreateImage(
                "HowToPlayPanel",
                root.transform,
                new Color(0f, 0f, 0f, 0.82f)
            );
            StretchFull(howToPlayPanel.rectTransform);
            howToPlayPanel.raycastTarget = true;

            Image howToPlayBox = CreateImage(
                "HowToPlayBox",
                howToPlayPanel.transform,
                new Color(0.11f, 0.12f, 0.16f, 1f)
            );
            SetRect(howToPlayBox.rectTransform, new Vector2(1050f, 650f), Vector2.zero);

            TextMeshProUGUI howTitle = CreateText(
                "HowToPlayTitle",
                howToPlayBox.transform,
                "วิธีเล่น",
                54f,
                FontStyles.Bold,
                TextAlignmentOptions.Center
            );
            SetRect(howTitle.rectTransform, new Vector2(800f, 80f), new Vector2(0f, 235f));

            TextMeshProUGUI howBody = CreateText(
                "HowToPlayText",
                howToPlayBox.transform,
                "• กดปุ่มซ้าย–ขวาเพื่อเปลี่ยนมุมห้อง\n" +
                "• คลิกสิ่งของเพื่อสำรวจและเก็บเข้ากระเป๋า\n" +
                "• คลิกไอเทมในกระเป๋าเพื่อเลือกใช้งาน\n" +
                "• นำไอเทมไปใช้กับจุดที่ถูกต้องเพื่อแก้ปริศนา\n" +
                "• ค้นหาเส้นทางและเปิดเผยเรื่องราวที่ซ่อนอยู่",
                34f,
                FontStyles.Normal,
                TextAlignmentOptions.TopLeft
            );
            howBody.enableWordWrapping = true;
            SetRect(howBody.rectTransform, new Vector2(850f, 330f), new Vector2(0f, 25f));

            Button closeHowToPlayButton = CreateButton(
                "CloseHowToPlayButton",
                howToPlayBox.transform,
                "ปิด",
                new Vector2(0f, -245f),
                new Vector2(260f, 80f)
            );

            TextMeshProUGUI loadingIndicator = CreateText(
                "LoadingIndicator",
                root.transform,
                "กำลังโหลด...",
                32f,
                FontStyles.Normal,
                TextAlignmentOptions.Center
            );
            SetRect(loadingIndicator.rectTransform, new Vector2(500f, 70f), new Vector2(0f, -440f));

            SimpleMainMenuController controller = root.AddComponent<SimpleMainMenuController>();

            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("gameplaySceneName").stringValue = "Gameplay";
            serializedController.FindProperty("mainMenuRoot").objectReferenceValue = menuPanel.gameObject;
            serializedController.FindProperty("howToPlayPanel").objectReferenceValue = howToPlayPanel.gameObject;
            serializedController.FindProperty("loadingIndicator").objectReferenceValue = loadingIndicator.gameObject;
            serializedController.FindProperty("startButton").objectReferenceValue = startButton;
            serializedController.FindProperty("howToPlayButton").objectReferenceValue = howToPlayButton;
            serializedController.FindProperty("closeHowToPlayButton").objectReferenceValue = closeHowToPlayButton;
            serializedController.FindProperty("quitButton").objectReferenceValue = quitButton;
            serializedController.ApplyModifiedPropertiesWithoutUndo();

            howToPlayPanel.gameObject.SetActive(false);
            loadingIndicator.gameObject.SetActive(false);

            EnsureEventSystem();

            Undo.RegisterCreatedObjectUndo(root, "Create Simple Main Menu");
            Selection.activeGameObject = root;
            EditorSceneManager.MarkSceneDirty(activeScene);

            EditorUtility.DisplayDialog(
                "สร้าง Main Menu สำเร็จ",
                "สร้างหน้า Main Menu แบบเรียบง่ายเรียบร้อยแล้ว\n\n" +
                "ขั้นต่อไป:\n" +
                "1. กด Ctrl + S\n" +
                "2. ตรวจ Build Profiles ว่ามี MainMenu และ Gameplay\n" +
                "3. กด Play ทดสอบ",
                "ตกลง"
            );
        }

        private static void EnsureEventSystem()
        {
            EventSystem existing = UnityEngine.Object.FindFirstObjectByType<EventSystem>();
            if (existing != null)
            {
                return;
            }

            GameObject eventSystemObject = CreateGameObject(
                "EventSystem",
                null,
                typeof(EventSystem)
            );

            Type inputSystemModuleType = Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem"
            );

            if (inputSystemModuleType != null)
            {
                eventSystemObject.AddComponent(inputSystemModuleType);
            }
            else
            {
                eventSystemObject.AddComponent<StandaloneInputModule>();
            }
        }

        private static GameObject CreateGameObject(
            string name,
            Transform parent,
            params Type[] components
        )
        {
            GameObject gameObject = new GameObject(name, components);
            if (parent != null)
            {
                gameObject.transform.SetParent(parent, false);
            }
            return gameObject;
        }

        private static Image CreateImage(
            string name,
            Transform parent,
            Color color
        )
        {
            GameObject gameObject = CreateGameObject(
                name,
                parent,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

            Image image = gameObject.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            return image;
        }

        private static TextMeshProUGUI CreateText(
            string name,
            Transform parent,
            string text,
            float fontSize,
            FontStyles fontStyle,
            TextAlignmentOptions alignment
        )
        {
            GameObject gameObject = CreateGameObject(
                name,
                parent,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(TextMeshProUGUI)
            );

            TextMeshProUGUI label = gameObject.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = fontSize;
            label.fontStyle = fontStyle;
            label.alignment = alignment;
            label.color = Color.white;
            label.raycastTarget = false;
            label.enableWordWrapping = false;

            if (TMP_Settings.defaultFontAsset != null)
            {
                label.font = TMP_Settings.defaultFontAsset;
            }

            return label;
        }

        private static Button CreateButton(
            string name,
            Transform parent,
            string text,
            Vector2 anchoredPosition,
            Vector2? size = null
        )
        {
            GameObject gameObject = CreateGameObject(
                name,
                parent,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(Button)
            );

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            SetRect(
                rectTransform,
                size ?? new Vector2(430f, 90f),
                anchoredPosition
            );

            Image image = gameObject.GetComponent<Image>();
            image.color = new Color(0.22f, 0.24f, 0.31f, 1f);

            Button button = gameObject.GetComponent<Button>();
            button.targetGraphic = image;

            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.22f, 0.24f, 0.31f, 1f);
            colors.highlightedColor = new Color(0.32f, 0.35f, 0.45f, 1f);
            colors.pressedColor = new Color(0.16f, 0.18f, 0.24f, 1f);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color(0.13f, 0.14f, 0.17f, 0.65f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;

            TextMeshProUGUI label = CreateText(
                "Text (TMP)",
                gameObject.transform,
                text,
                32f,
                FontStyles.Normal,
                TextAlignmentOptions.Center
            );
            StretchFull(label.rectTransform);

            return button;
        }

        private static void SetRect(
            RectTransform rectTransform,
            Vector2 size,
            Vector2 anchoredPosition
        )
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.localScale = Vector3.one;
        }

        private static void StretchFull(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }
}
#endif
