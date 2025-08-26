
using ThePurified.PlayerSystem;
using UnityEngine;

namespace ThePurified.Items
{
    public class Teddy : GameItem
    {
        public override void OnItemInteract()
        {
            InspectController.instance.SetItemToInspect(gameObject);
        }

    }
}

