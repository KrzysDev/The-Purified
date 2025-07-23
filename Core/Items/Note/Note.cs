using ThePurified.Items;
using ThePurified.PlayerSystem;

/// <summary>
/// notatka ktora gracz moze podniesc i zrobic jej inspekcje.
/// </summary>
public class Note : GameItem
{
    public override void OnItemInteract()
    {
        InspectController.instance.SetItemToInspect(gameObject);
    }
}
    
