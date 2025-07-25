
namespace ThePurified.Items
{
    public class CubePart : GameItem
    {
        public static int cubePartsCollected = 0;
        public override void OnItemInteract()
        {
            cubePartsCollected++;
            Destroy(gameObject);
        }
    }
}

