using ThePurified.Items;
using ThePurified.PlayerSystem;
using UnityEngine;

public class Note : GameItem 
{
    public override void OnItemInteract()
    {
        InspectController.instance.SetItemToInspect(gameObject);
    }
}
    
