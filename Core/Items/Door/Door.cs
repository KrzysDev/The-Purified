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

    public override void OnItemInteract()
    {

        HandleDoor();
    }
    /// <summary>
    /// obsluga logiki drzwi
    /// </summary>
    private void HandleDoor()
    {
        if (doorUnlocked)
        {
            animator.SetTrigger("return");

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
            animator.SetTrigger("close");
            AudioManager.instance.PlaySoundInPosition("doorClosed", transform.position);
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
            doorHolder.transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.LerpAngle(currentAngle, newAngle, elapsed), transform.localRotation.eulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorHolder.transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, newAngle, transform.localRotation.eulerAngles.z);

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
