using UnityEngine;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez kartę, ktora otwiera drzwi na ów kartę.
    /// </summary>
    public class Card : GameItem
    {
        [SerializeField] Door cardDoor;
        public override void OnItemInteract()
        {
            cardDoor.doorUnlocked = true;
            Destroy(gameObject);
        }
    }
}

