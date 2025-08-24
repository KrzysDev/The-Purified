using ThePurified.Items;
using ThePurified.PlayerSystem;
using ThePurified.AudioSystem;
using UnityEngine;

/// <summary>
/// notatka ktora gracz moze podniesc i zrobic jej inspekcje.
/// </summary>
public class Note : GameItem
{
    public override void OnItemInteract()
    {
        //Debug.Log("note - on item interact");

        AudioManager.instance.PlaySound("note");
        InspectController.instance.SetItemToInspect(gameObject);
    }

    public override void OnItemInspectionEnd()
    {
        //Debug.Log("Note - on inspection end");
        AudioManager.instance.PlaySound("note");
    }
}
    
