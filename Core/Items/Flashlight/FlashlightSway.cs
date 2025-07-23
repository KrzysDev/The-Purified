using System.Collections;
using UnityEngine;

public class FlashlightSway : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float swayAmount;
    [SerializeField] float swaySpeed;

    private Vector3 offset;

    private float value;

    private Vector3 originalPos;

    private float elapsed = 0f;

    [Header("reseting speed")]
    [Tooltip("How fast flashlight returns to its original position when player is not walking (stopped swaying)")]
    [SerializeField] float resettingSpeed = 2f;

    Coroutine resetCoroutine;

    void Start()
    {
        originalPos = transform.position;   
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            HandleSway();
        else if (resetCoroutine == null)
        {
            resetCoroutine = StartCoroutine(ResetPosition());
        }

    }

    private void HandleSway()
    {
        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        value = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        offset = new Vector3(value * 2, value, 0);

        transform.position = originalPos + offset;
    }

    private IEnumerator ResetPosition()
    {
        Vector3 position = transform.position;

        while (elapsed < 1f)
        {
            transform.position = Vector3.Lerp(position, originalPos, elapsed);
            elapsed += Time.deltaTime * resettingSpeed;
            yield return null;
        }

        transform.position = originalPos;

        resetCoroutine = null;
    }
}
