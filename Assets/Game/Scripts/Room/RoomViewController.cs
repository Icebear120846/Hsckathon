using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    [Serializable]
    public sealed class RoomViewEntry
    {
        [SerializeField] private string viewId;
        [SerializeField] private GameObject root;
        [Tooltip("เปิดไว้เฉพาะมุมหลักที่ปุ่มซ้าย-ขวาควรวนผ่าน ไม่เปิดสำหรับมุมซูม")]
        [SerializeField] private bool includeInLeftRightCycle = true;

        public string ViewId => viewId;
        public GameObject Root => root;
        public bool IncludeInLeftRightCycle => includeInLeftRightCycle;
    }

    public sealed class RoomViewController : MonoBehaviour
    {
        [SerializeField] private List<RoomViewEntry> views = new List<RoomViewEntry>();
        [SerializeField] private string startingViewId = "VIEW_FRONT";

        private readonly Dictionary<string, GameObject> viewLookup = new Dictionary<string, GameObject>(StringComparer.Ordinal);
        private readonly List<string> cycleViewIds = new List<string>();
        private readonly Stack<string> history = new Stack<string>();
        private int currentCycleIndex;

        public string CurrentViewId { get; private set; }

        private void Awake()
        {
            BuildLookup();
            OpenView(startingViewId, false);
        }

        public void OpenView(string viewId)
        {
            OpenView(viewId, true);
        }

        public void OpenView(string viewId, bool rememberCurrent)
        {
            if (string.IsNullOrWhiteSpace(viewId) || !viewLookup.TryGetValue(viewId, out GameObject targetView))
            {
                Debug.LogWarning($"RoomViewController: ไม่พบ View ID '{viewId}'", this);
                return;
            }

            if (rememberCurrent && !string.IsNullOrWhiteSpace(CurrentViewId) && CurrentViewId != viewId)
            {
                history.Push(CurrentViewId);
            }

            foreach (KeyValuePair<string, GameObject> pair in viewLookup)
            {
                if (pair.Value != null)
                {
                    pair.Value.SetActive(pair.Key == viewId);
                }
            }

            CurrentViewId = viewId;
            int cycleIndex = cycleViewIds.IndexOf(viewId);
            if (cycleIndex >= 0)
            {
                currentCycleIndex = cycleIndex;
            }
        }

        public void ShowNext()
        {
            if (cycleViewIds.Count == 0)
            {
                return;
            }

            int nextIndex = (currentCycleIndex + 1) % cycleViewIds.Count;
            OpenView(cycleViewIds[nextIndex], false);
            history.Clear();
        }

        public void ShowPrevious()
        {
            if (cycleViewIds.Count == 0)
            {
                return;
            }

            int previousIndex = (currentCycleIndex - 1 + cycleViewIds.Count) % cycleViewIds.Count;
            OpenView(cycleViewIds[previousIndex], false);
            history.Clear();
        }

        public void GoBack()
        {
            if (history.Count == 0)
            {
                return;
            }

            string previousViewId = history.Pop();
            OpenView(previousViewId, false);
        }

        private void BuildLookup()
        {
            viewLookup.Clear();
            cycleViewIds.Clear();

            for (int i = 0; i < views.Count; i++)
            {
                RoomViewEntry entry = views[i];
                if (entry == null || string.IsNullOrWhiteSpace(entry.ViewId) || entry.Root == null)
                {
                    continue;
                }

                if (viewLookup.ContainsKey(entry.ViewId))
                {
                    Debug.LogError($"RoomViewController: View ID ซ้ำ '{entry.ViewId}'", this);
                    continue;
                }

                viewLookup.Add(entry.ViewId, entry.Root);

                if (entry.IncludeInLeftRightCycle)
                {
                    cycleViewIds.Add(entry.ViewId);
                }
            }
        }
    }
}
