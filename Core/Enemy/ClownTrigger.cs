using UnityEngine;

namespace ThePurified.AI
{
    /// <summary>
    /// klasa znajdujaca sie na obiekcie z BoxColliderem w trybie trigger. Trigger sprawdza czy gracz znajduje sie wystarczajaco blisko w pokoju z Robo-Clownem zeby go aktywowac. Chodzi o pierwsze spotkanie clowna z graczem.
    /// </summary>
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

