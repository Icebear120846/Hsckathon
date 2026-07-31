using System.Collections;
using UnityEngine;

namespace PointClickTemplate
{
    /// <summary>
    /// ติดบน Hotspot ของชิ้นส่วนภาพ เพื่อซ่อน Hotspot อัตโนมัติ
    /// หากชิ้นส่วนนั้นเคยถูกเก็บแล้วใน Scene ก่อนหน้า
    /// </summary>
    public sealed class PuzzleFragmentPickupVisibility : MonoBehaviour
    {
        [SerializeField] private ItemData fragmentItem;

        private Coroutine validationRoutine;

        private void OnEnable()
        {
            validationRoutine = StartCoroutine(ValidateVisibilityNextFrame());
        }

        private void OnDisable()
        {
            if (validationRoutine == null)
            {
                return;
            }

            StopCoroutine(validationRoutine);
            validationRoutine = null;
        }

        private IEnumerator ValidateVisibilityNextFrame()
        {
            // รอให้ Persistent Manager และ Scene Systems Awake ครบก่อน
            yield return null;

            PuzzleFragmentProgress progress = PuzzleFragmentProgress.Instance;
            if (progress != null && progress.HasCollected(fragmentItem))
            {
                gameObject.SetActive(false);
            }

            validationRoutine = null;
        }
    }
}
