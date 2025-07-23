using ThePurified.PlayerSystem;
using UnityEngine;

namespace ThePurified.Items
{
    /// <summary>
    /// Dziedziczą ją wszystkie itemy w grze z ktorymi gracz wchodzi w interakcje. Przechowuje logike interakcji z itemami w grze.
    /// </summary>
    public abstract class GameItem : MonoBehaviour, IInteraction, IInteractionHover
    {
        /// <summary>
        /// funkcja wywolywana na obiekcie dziedziczącym te klasę kiedy gracz wejdzie z nim w interakcje
        /// </summary>
        public virtual void OnItemInteract() { }
        /// <summary>
        /// funkcja wywolywana na obiekcie dziedziczacym te klase kiedy gracz najedzie na niego myszka.
        /// </summary>
        public virtual void OnItemHover() { }
        /// <summary>
        /// funkcja wywolywana na obiekcie dziedziczacym te klase kiedy gracz przestanie najezdzac na niego myszka
        /// </summary>
        public virtual void OnItemHoverExit() { }
        /// <summary>
        /// funkcja wywolywana na obiekcie dziedziczacym te klase co klatke
        /// </summary>
        public virtual void ItemUpdate() { }

        /// <summary>
        /// funkcja wywolywana na obiekcie dziedziczacym te klase w pierwszej klatce gry
        /// </summary>
        public virtual void ItemStart() { }

        public void OnInteract()
        {
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

