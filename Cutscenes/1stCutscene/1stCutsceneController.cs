using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ThePurified.FX
{
    public class _1stCutsceneController : MonoBehaviour
    {
        [SerializeField] private float turningDownSpeed = 1f;

        private AsyncOperation operation;

        [SerializeField] Image loadingCircle;

        [SerializeField] Animator cutsceneAnimator;


        private void Start()
        {
            StartCoroutine(LoadNextSceneAsync());
        }        

        private IEnumerator LoadNextSceneAsync()
        { 
            //"ladowanie sceny" - w rzeczywistosci czekanie na zakonczenie cutscenki

            AnimatorStateInfo stateInfo = cutsceneAnimator.GetCurrentAnimatorStateInfo(0);

            

            float elapsed = 0;
            
            while(elapsed < stateInfo.length)
            {
                elapsed += Time.deltaTime;
                
                loadingCircle.fillAmount = Mathf.Lerp(0, 0.9f, elapsed / stateInfo.length);
                yield return null;
            }

            //faktyczne ladowanie sceny.
            operation = SceneManager.LoadSceneAsync("House");
            operation.allowSceneActivation = false;

            while (true)
            {
                if(operation.progress >= 0.9f)
                    break;


                loadingCircle.fillAmount = Mathf.Lerp(0.9f, 1f, operation.progress);
                yield return null;
                
            }

            operation.allowSceneActivation = true;
            ResetListener();
        }





        /// <summary>
        /// funkcja uzywana na timeline animacji do sciszania dzwieku w grze.
        /// </summary>
        public void TurnDownTheSound()
        {
            StartCoroutine(TurnDownListener());
        }

        private IEnumerator TurnDownListener()
        {
            float elapsed = 0;

            while(elapsed < turningDownSpeed)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, elapsed / turningDownSpeed);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void ResetListener()
        {
            AudioListener.volume = 1f;
        }

    }
}

