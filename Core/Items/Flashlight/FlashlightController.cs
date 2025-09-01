using System.Collections;
using UnityEngine;
using ThePurified.AudioSystem;
namespace ThePurified.Flashlight
{
    /// <summary>
    /// obsluguje logike latarki
    /// </summary>
    public class FlashlightController : MonoBehaviour
    {
        public static KeyCode flashlightKey = KeyCode.F;
        [SerializeField] GameObject flashligthLight;

        private bool wasActiveBefore = false;

        [SerializeField] Animator animator;

        public static bool controllerEnabled = true;

        void Start()
        {
            flashligthLight.SetActive(false);
        }
        void Update()
        {
            if(controllerEnabled)
                HandleFlashlight();
        }

        /// <summary>
        /// obsluguje input latarki
        /// </summary>
        void HandleFlashlight()
        {
            if (Input.GetKeyDown(flashlightKey))
            {
                flashligthLight.SetActive(!flashligthLight.activeSelf);

                AudioManager.instance.PlaySound("Flashlight");

                //animacja przy pierwszym uruchomieniu latarki
                if (!wasActiveBefore)
                {
                    wasActiveBefore = true;
                    StartCoroutine(FirstInteraction());
                }


            }
        }

        /// <summary>
        /// korutyna czekajaca az animacja uruchamiajaca sie przy pierwszym wlaczaniu latarki sie skonczy i wylaczajaca animator.
        /// </summary>
        private IEnumerator FirstInteraction()
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            yield return new WaitForSeconds(info.length);

            animator.enabled = false;
            wasActiveBefore = true;
        }
    }
}
   
