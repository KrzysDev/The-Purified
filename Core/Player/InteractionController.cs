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
    /// <summary>
    /// obsluguje system interakcji. Definiuje kiedy gracz moze wchodzic w interakcje z przedmiotami.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float distance;
        [Tooltip("Layer with objects that player can interact with")]
        [SerializeField] private LayerMask layer;

        public static KeyCode interactionKey = KeyCode.E;

        private IInteraction currentInteraction = null;
        private IInteractionHover previousHover = null, currentHover = null;


        void Update()
        {
            HandleInteraction();
        }

        ///<summary>
        ///Zajmuje sie detekcja interakcji i jej wywolywaniem.
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

            if (previousHover != currentHover && previousHover != null)
            {
                previousHover.OnHoverExit();
            }

            previousHover = currentHover;
        }
    }
}

