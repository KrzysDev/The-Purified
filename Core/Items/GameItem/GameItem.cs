using ThePurified.PlayerSystem;
using UnityEngine;

namespace ThePurified.Items
{
    public abstract class GameItem : MonoBehaviour, IInteraction, IInteractionHover
    {
        public virtual void OnItemInteract() { }
        public virtual void OnItemInteractEnd() { }
        public virtual void OnItemHover() { }
        public virtual void OnItemHoverExit() { }
        public virtual void ItemUpdate() { }

        public virtual void ItemStart() { }

        public void OnInteract()
        {
            //Debug.Log($"{gameObject.name} OnItemInteract!");
            OnItemInteract();
        }


        public void OnHoverExit()
        {
            OnItemHover();
        }

        public void OnHover()
        {
            OnItemHoverExit();
        }


        private void Update()
        {
            ItemUpdate();
        }

        private void Start()
        {
            ItemStart();
        }

        //TODO: display interaction icon.

    }
}

