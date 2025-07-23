using System.Collections;
using ThePurified.Items;
using ThePurified.AudioSystem;
using UnityEngine;



public class Door : GameItem
{
    [Header("Door settings")]
    [SerializeField] Transform doorHolder;
    [SerializeField] float openAngle;
    [SerializeField] float closeAngle;
    [SerializeField] float openingDuration = 2f;

    private float newAngle;
    private float currentAngle;
    bool isOpened = false;
    Coroutine currentCoroutine = null;

    [Header("Door animator")]
    [SerializeField] Animator animator;
    public bool doorUnlocked = false;

    public override void OnItemInteract()
    {
        HandleDoor();
    }
    private void HandleDoor()
    {
        if (doorUnlocked)
        {

            animator.SetBool("isOpen", true);
            animator.SetTrigger("open");

            animator.enabled = false;

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            newAngle = isOpened ? closeAngle : openAngle;

            currentCoroutine = StartCoroutine(RotateDoor(newAngle));

            isOpened = !isOpened;

            AudioManager.instance.PlaySoundInPosition("doorOpen", transform.position);

        }
        else
        {
            animator.SetTrigger("open");
            AudioManager.instance.PlaySoundInPosition("doorClosed", transform.position);
        }
    }


    private IEnumerator RotateDoor(float newAngle)
    {
        float elapsed = 0f;

        currentAngle = doorHolder.transform.localRotation.eulerAngles.y;

        // Debug.Log("kąt przed petlą: " + currentAngle);

        while (elapsed < openingDuration)
        {
            doorHolder.transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.LerpAngle(currentAngle, newAngle, elapsed), transform.localRotation.eulerAngles.z);
            elapsed +=  Time.deltaTime;
            yield return null;
        }

        doorHolder.transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, newAngle, transform.localRotation.eulerAngles.z);

    }
}
