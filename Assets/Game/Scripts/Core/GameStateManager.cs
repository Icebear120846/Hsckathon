using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClickTemplate
{
    /// <summary>
    /// เก็บสถานะเหตุการณ์แบบ Flag เช่น PHOTO_COMPLETED หรือ DRAWER_OPENED
    /// สถานะจะรีเซ็ตเมื่อเริ่ม Gameplay Scene ใหม่
    /// </summary>
    public sealed class GameStateManager : MonoBehaviour
    {
        private readonly HashSet<string> activeFlags = new HashSet<string>(StringComparer.Ordinal);

        public event Action<string, bool> FlagChanged;

        public bool HasFlag(string flagId)
        {
            return !string.IsNullOrWhiteSpace(flagId) && activeFlags.Contains(flagId);
        }

        public void SetFlag(string flagId, bool value = true)
        {
            if (string.IsNullOrWhiteSpace(flagId))
            {
                return;
            }

            bool changed;
            if (value)
            {
                changed = activeFlags.Add(flagId);
            }
            else
            {
                changed = activeFlags.Remove(flagId);
            }

            if (changed)
            {
                FlagChanged?.Invoke(flagId, value);
            }
        }

        public bool HasAllFlags(IReadOnlyList<string> requiredFlags)
        {
            if (requiredFlags == null || requiredFlags.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < requiredFlags.Count; i++)
            {
                string flagId = requiredFlags[i];
                if (!string.IsNullOrWhiteSpace(flagId) && !HasFlag(flagId))
                {
                    return false;
                }
            }

            return true;
        }

        public void ResetAllFlags()
        {
            activeFlags.Clear();
        }
    }
}
