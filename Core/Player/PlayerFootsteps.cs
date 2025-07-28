using UnityEngine;
using ThePurified.AudioSystem;
using System.Collections;
using ThePurified.Items;

/// <summary>
/// Odtwarza dzwieki krokow gdy gracz chodzi.
/// </summary>
public class PlayerFootsteps : MonoBehaviour
{
    private Vector3 lastPos;

    [Tooltip("Jak daleko musisz isc zeby uslyszec dzwieki krokow?")]
    [SerializeField] float walkingInterval;
    [SerializeField] float runningInterval;

    private bool stepping = false;

    private bool isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

    private bool isRunning => Input.GetKey(KeyCode.LeftShift);

    Coroutine currentCoroutine;



    void Start()
    {
        lastPos = transform.position;
    }

    private void Update()
    {
        HandleFootstepSound();
    }

    private void HandleFootstepSound()
    {
        if (!stepping && isMoving && lastPos != transform.position)
        {
            if (isRunning)
            {
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);

                currentCoroutine = StartCoroutine(PlayFootsteps(runningInterval));
            }
            else
            {
                currentCoroutine = StartCoroutine(PlayFootsteps(walkingInterval));
            }
        }
    }

    private IEnumerator PlayFootsteps(float time)
    {
        stepping = true;
        AudioManager.instance.PlayRandomWithTag("footstep", transform.position);
        lastPos = transform.position;
        yield return new WaitForSeconds(time);
        stepping = false;
    }
}