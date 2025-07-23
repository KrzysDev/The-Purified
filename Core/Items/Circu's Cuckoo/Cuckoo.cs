using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez 'kukułkę' w kształcie cyrku z wyskakującą na sprężynie maską clowna. Prosty jumpscare.
    /// </summary>
    public class Cuckoo : MonoBehaviour
    {
        [SerializeField] Animator animator;

        private bool entered = false;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !entered)
            {
                Jumpscare();
            }
        }

        private void Jumpscare()
        {
            entered = true;
            animator.SetBool("entered", true);
            AudioManager.instance.PlaySoundInPosition("Cuckoo", transform.position);
        }
    }
}

