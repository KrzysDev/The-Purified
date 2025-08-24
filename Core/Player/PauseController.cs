using UnityEditor;
using UnityEngine;


namespace ThePurified.PlayerSystem
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] GameObject pauseCanvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseCanvas.SetActive(!pauseCanvas.activeSelf);
                Time.timeScale = pauseCanvas.activeSelf ? 0 : 1;
                Cursor.visible = pauseCanvas.activeSelf;
                Cursor.lockState = pauseCanvas.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }


        public void Resume()
        {
            pauseCanvas?.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Settings()
        {
            //TODO: jakies ustawienia w przyszlosci
        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}

