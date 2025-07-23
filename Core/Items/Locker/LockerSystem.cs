using ThePurified.Items;
using ThePurified.PlayerSystem;
using UnityEngine;

public class LockerSystem : GameItem
{
    [Header("Locker Hiding point")]
    [SerializeField] Transform hidingSpot;
    [Header("Leave Locker point")]
    [SerializeField] Transform leaveLocker;

    [Header("Player Body")]
    [SerializeField] Transform playerBody;

    private bool isHidden = false;

    private CharacterController controller;

    public override void ItemStart()
    {
        controller = playerBody.GetComponent<CharacterController>();
    }

    public override void OnItemInteract()
    {
        Debug.Log("wywołanie funkcji on item interact w lockerSystem");

        controller.enabled = false;
        playerBody.transform.position = isHidden ? leaveLocker.position : hidingSpot.position;
        controller.enabled = true;

        isHidden = !isHidden;

        PlayerMovement.movementEnabled = !isHidden;
    }
    
}
