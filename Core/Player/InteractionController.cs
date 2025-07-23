using System.Security.Cryptography;
using UnityEngine;

namespace ThePurified.PlayerSystem
{
    interface IInteraction
    {
        void OnInteract();
    }

    interface IInteractionHover
    {
        void OnHover();
        void OnHoverExit();
    }
    public class InteractionController : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float distance;
        [Tooltip("Layer with objects that player can interact with")]
        [SerializeField] private LayerMask layer;

        public static KeyCode interactionKey = KeyCode.E;

        private IInteraction currentInteraction = null, previousInteraction = null;
        private IInteractionHover previousHover = null, currentHover = null;


        void Update()
        {
            HandleInteraction();
        }

        ///<summary>
        //Handles interaction detecting and activating
        ///</summary> 
        void HandleInteraction()
        {
            currentHover = null;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, distance, layer))
            {
                //When player hovers over object.
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractionHover hover))
                {
                    currentHover = hover;
                    hover.OnHover();
                }

                //when player hovers and interacts with object.
                if (Input.GetKeyDown(interactionKey))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out IInteraction interaction))
                    {
                        interaction.OnInteract();
                    }
                    else
                    {
                        Debug.LogWarning($"Object {hitInfo.collider.gameObject.name} is on interaction layer and does not implement IInteraction interface!");
                    }
                }


            }

            //When player stops hovering over object
            if (previousHover != currentHover && previousHover != null)
            {
                previousHover.OnHoverExit();
            }

           /* if (currentInteraction != previousInteraction && previousInteraction != null)
            {
                previousInteraction.OnInteractEnd();
            } */

            previousHover = currentHover;

            previousInteraction = currentInteraction;
        }
    }
}

