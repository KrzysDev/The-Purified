using System.Collections;
using UnityEngine;
using ThePurified.AudioSystem;
namespace ThePurified.Flashlight
{
    public class FlashlightController : MonoBehaviour
    {
        public static KeyCode flashlightKey = KeyCode.F;
        [SerializeField] GameObject flashligthLight;

        private bool wasActiveBefore = false;

        [SerializeField] Animator animator;

        void Start()
        {
            flashligthLight.SetActive(false);
        }
        void Update()
        {
            HandleFlashlight();
        }

        void HandleFlashlight()
        {
            if (Input.GetKeyDown(flashlightKey))
            {
                flashligthLight.SetActive(!flashligthLight.activeSelf);

                AudioManager.instance.PlaySound("Flashlight");
                
                //animacja na poczatku gry 
                if (!wasActiveBefore)
                {
                    wasActiveBefore = true;
                    StartCoroutine(FirstInteraction());
                }


            }
        }

        private IEnumerator FirstInteraction()
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            yield return new WaitForSeconds(info.length);

            animator.enabled = false;
            wasActiveBefore = true;
        }
    }
}
   
