using ThePurified.Items;
using UnityEngine;

namespace ThePurified.Items
{
    public class Present : GameItem
    {
        private Rigidbody rb;

        private bool unlocked = false;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            rb.isKinematic = true;
        }


        public void UnlockPresent()
        {
            unlocked = true;
        }

        public override void OnItemInteract()
        {
            if (unlocked)
            {
                rb.isKinematic = false;
                rb.AddForce(Vector3.forward, ForceMode.Impulse);
            }
        }
    }
}

