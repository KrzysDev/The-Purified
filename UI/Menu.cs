using System.Collections;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ThePurified.AudioSystem;

namespace ThePurified.UI
{
    /// <summary>
    /// klasa obslugujaca main menu gry. Funkcje sa przypisane do przyciskow w silniku. 
    /// </summary>
    public class Menu : MonoBehaviour
    {
        [Header("Menu")]
        [Tooltip("press any key text")]
        [SerializeField] GameObject pressAnyKey;
        [SerializeField] GameObject buttonsHolder;

        [SerializeField] GameObject settingsHolder;

        [Header("Settings")]
        [SerializeField] Slider volumeSlider;

        [Header("Scene Transition")]
        [SerializeField] Animator animator;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void Update()
        {
            if (Input.anyKey)
            {
                pressAnyKey.SetActive(false);
                buttonsHolder.SetActive(true);
            }
        }


        public void Begin()
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            animator.SetBool("transition", true);

            yield return new WaitForSeconds(info.length);

            SceneManager.LoadScene(sceneIndex);
        }

        public void Settings(bool on)
        {
            settingsHolder.SetActive(on);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();

        }

        //ustawienia


        public void ChangeVolume()
        {
            AudioListener.volume = volumeSlider.value;

            Debug.Log($" main volume set: {AudioListener.volume}");
        }

    }
}

