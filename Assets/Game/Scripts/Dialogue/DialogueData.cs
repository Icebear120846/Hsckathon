using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    [Serializable]
    public sealed class DialogueLine
    {
        [SerializeField] private string speakerName;
        [TextArea(2, 6)]
        [SerializeField] private string message;
        [SerializeField] private Sprite portrait;

        public string SpeakerName => speakerName;
        public string Message => message;
        public Sprite Portrait => portrait;
    }

    [CreateAssetMenu(fileName = "DIALOGUE_NewDialogue", menuName = "Point & Click/Dialogue Data")]
    public sealed class DialogueData : ScriptableObject
    {
        [SerializeField] private List<DialogueLine> lines = new List<DialogueLine>();

        public IReadOnlyList<DialogueLine> Lines => lines;
    }
}
