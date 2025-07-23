using UnityEngine;

namespace ThePurified.Items
{
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

