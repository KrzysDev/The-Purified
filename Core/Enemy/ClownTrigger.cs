using UnityEngine;

namespace ThePurified.AI
{
    [RequireComponent(typeof(BoxCollider))]
    public class ClownTrigger : MonoBehaviour
    {
        private bool interacted = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !interacted)
            {
                interacted = true;
                ClownSystem.instance.ActivateAI();
            }
        }
    }
}

