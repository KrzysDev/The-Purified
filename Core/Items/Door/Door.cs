using System.Collections;
using ThePurified.Items;
using ThePurified.AudioSystem;
using UnityEngine;
using UnityEditor.Rendering;

/// <summary>
///klasa uzywana przez drzwi
/// </summary>
public class Door : GameItem
{
    [Header("ustawienia drzwi")]
    [SerializeField] Transform doorHolder;
    [Tooltip("kat w ktorym drzwi sa otwarte (np 90)")]
    [SerializeField] float openAngle;
    [Tooltip("kat w ktorym drzwi sa zamkniete (np 0)")]
    [SerializeField] float closeAngle;
    [SerializeField] float openingDuration = 2f;

    private float newAngle;
    private float currentAngle;
    public bool isOpened = false;
    Coroutine currentCoroutine = null;

    [Header("Door animator")]
    [Tooltip("Animator ktory wyswietla animacje drzwi zamknietych. Drzwi zamkniete -> interakcja -> ta animacja na tym animatorze")]
    [SerializeField] Animator animator;
    public bool doorUnlocked = false;

    [Header("Door Sound")]
    [SerializeField] string openingSoundName;
    [SerializeField] string closingSoundName;

    public override void OnItemInteract()
    {
        //Debug.Log($"{nameof(Door)} interakcja");
        HandleDoor();

        if (doorUnlocked)
        {
            isOpened = !isOpened;
        }
    }
    /// <summary>
    /// obsluga logiki drzwi
    /// </summary>
    private void HandleDoor()
    {
        if (doorUnlocked)
        {
            if(animator!=null)
            {
                animator.SetTrigger("return");

                animator.enabled = false;
            }


            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            newAngle = isOpened ? closeAngle : openAngle;

            currentCoroutine = StartCoroutine(RotateDoor(newAngle));

            if (!isOpened)
            {
                Debug.Log("zamkniete wiec otwieram");
                AudioManager.instance.PlaySoundInPosition(openingSoundName, transform.position, 0.95f, 1.05f);
            }

            else
            {
                Debug.Log("otwarte wiec zamykam");
                AudioManager.instance.PlaySoundInPosition(closingSoundName, transform.position, 0.95f, 1.05f);
            }
                

        }
        else
        {
            if(animator!=null)
            animator.SetTrigger("close");
            AudioManager.instance.PlaySoundInPosition("doorClosed", transform.position);
            if(animator!=null)
            animator.SetTrigger("return");
        }
    }

    /// <summary>
    /// interpoluje rotacje drzwi
    /// </summary>
    /// <param name="newAngle"> kat do ktorego drzwi beda sie obracac </param>
    private IEnumerator RotateDoor(float newAngle)
    {
        float elapsed = 0f;

        currentAngle = doorHolder.transform.localRotation.eulerAngles.y;

        while (elapsed < openingDuration)
        {
            doorHolder.transform.localRotation = Quaternion.Euler(doorHolder.localRotation.eulerAngles.x, Mathf.LerpAngle(currentAngle, newAngle, elapsed / openingDuration), doorHolder.localRotation.eulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorHolder.transform.localRotation = Quaternion.Euler(doorHolder.localRotation.eulerAngles.x, newAngle, doorHolder.localRotation.eulerAngles.z);

    }


    public void Open(float angle)
    {
        StartCoroutine(RotateDoor(angle));
    }

    public void Close()
    {
        StartCoroutine(RotateDoor(closeAngle));
    }

}
