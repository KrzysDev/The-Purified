using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace ThePurified.UI
{
    /// <summary>
    /// klasa ktora odtwarza cutscenke nagrana jako plik video. Mozliwe ze skrypt ten nie bedzie potrzebny pozniej i zrobie cutscenke jako animacja w silniku.
    /// </summary>
    public class CutsceneController : MonoBehaviour
    {
        [SerializeField] VideoPlayer player;
        void Start()
        {
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds((float)player.clip.length);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}