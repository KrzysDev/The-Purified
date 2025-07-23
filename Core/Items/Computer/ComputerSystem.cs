using System.Collections;
using ThePurified.PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

namespace ThePurified.Items
{
    public class ComputerSystem : GameItem
    {
        [Header("Player: ")]
        [SerializeField] Transform playerHead;

        [Tooltip("player arm")]
        [SerializeField] GameObject playerArm;

        private Vector3 originalHeadPos;

        [Header("Computer Settings: ")]

        [SerializeField] Transform zoomPosition;

        [SerializeField] private float zoomSpeed = 2f;

        [SerializeField] TMP_InputField inputField;

        private Coroutine currentZoom;

        public static KeyCode leaveZoom = KeyCode.Tab;

        private bool interacting = false;

        [Header("Zoom animation curve: ")]
        [SerializeField] AnimationCurve curve;

        [Header("Correct password: ")]
        [SerializeField] string password = "1234";

        [Header("Events: ")]
        [SerializeField] UnityEvent onPasswordCorrect;

        public override void OnItemInteract()
        {
            if (!interacting)
            {
                interacting = true;

                PlayerHeadBob.headBobEnabled = false;

                originalHeadPos = playerHead.position;

                if (currentZoom != null)
                {
                    StopCoroutine(currentZoom);
                    currentZoom = null;
                }

                currentZoom = StartCoroutine(Lerp(zoomPosition.position));

                playerArm.SetActive(false);
                PlayerMovement.movementEnabled = false;

                inputField.Select();
                inputField.ActivateInputField();
            }

        }

        public override void ItemUpdate()
        {
            if (Input.GetKeyDown(leaveZoom) && interacting)
            {
                interacting = false;

                if (currentZoom != null)
                {
                    StopCoroutine(currentZoom);
                    currentZoom = null;
                }

                currentZoom = StartCoroutine(Lerp(originalHeadPos));
                //Debug.Log("powrot");


                inputField.DeactivateInputField();

            }

        }


        private IEnumerator Lerp(Vector3 pos)
        {
            float duration = 1f / zoomSpeed;
            float elapsed = 0f;
            float curveValue;

            Vector3 start = playerHead.transform.position;

            while (elapsed < duration)
            {
                float t = elapsed / duration;

                curveValue = curve.Evaluate(t);

                playerHead.transform.position = Vector3.Lerp(start, pos, curveValue);

                elapsed += Time.deltaTime;

                yield return null;
            }

            playerHead.transform.position = pos;

            PlayerMovement.movementEnabled = !interacting;
            PlayerHeadBob.headBobEnabled = !interacting;

            playerArm.SetActive(!interacting);
        }


        public void CheckPassword()
        {
            if (inputField.text == password)
            {
                onPasswordCorrect.Invoke();
                inputField.text = "";
                inputField.textComponent.color = Color.green;
            }
            else
            {
                inputField.Select();
                inputField.ActivateInputField();

                inputField.textComponent.color = Color.red;
            }
        }


    }
}


