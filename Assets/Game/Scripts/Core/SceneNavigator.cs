using UnityEngine;
using UnityEngine.SceneManagement;

namespace PointClickTemplate
{
    public sealed class SceneNavigator : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("SceneNavigator: Scene Name ว่าง", this);
                return;
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void ReloadCurrentScene()
        {
            Time.timeScale = 1f;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("QuitGame ถูกเรียกใน Editor");
#else
            Application.Quit();
#endif
        }
    }
}
