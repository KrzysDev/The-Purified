using System.Collections;
using ThePurified.PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

namespace ThePurified.Items
{
    /// <summary>
    /// klasa obslugujaca logike komputera, do ktorego gracz wpisuje kod aby uzyskac karte dostepu.
    /// </summary>
    public class ComputerSystem : GameItem
    {
        [Header("Player: ")]
        [Tooltip("playerBody: ")]
        [SerializeField] Transform playerHead;

        private Vector3 originalHeadPos;

        [Header("Ustawienia komputera: ")]

        [Tooltip("pozycja do ktorej kamera bedzie sie przyblizac przy interakcji z komputerem")]
        [SerializeField] Transform zoomPosition;

        [Tooltip("jak szybko bedzie sie przyblizac kamera do pozycji przyblizenia")]
        [SerializeField] private float zoomSpeed = 2f;

        [Tooltip("pole tekstowe wyswietlane na komputerze")]
        [SerializeField] TMP_InputField inputField;

        private Coroutine currentZoom;

        public static KeyCode leaveZoom = KeyCode.Tab; //przycisk ktorym wychodzi sie z interakcji z komputerem.

        private bool interacting = false;

        [Header("Krzywa animacji Zoom: ")]
        [Tooltip("krzywa animacji przyblizania do komputera")]
        [SerializeField] AnimationCurve curve;

        [Header("Poprawne haslo: ")]
        [SerializeField] string password = "1234";

        [Header("Gdy haslo prawidlowe: ")]
        [Tooltip("wydarzenie ktore sie wywola gdy haslo w komputerze jest prawidlowe")]
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

        /// <summary>
        /// zmienia pozycje {playerHead} (obiektu sledzonego przez cinemachine) interpolujac miedzy pozycjami uzywajac krzywej animacji.
        /// </summary>
        /// <param name="pos">pozycja do ktorej glowa gracza jest przenoszona</param>
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

        }

        /// <summary>
        /// sprawdza czy wpisane haslo jest poprawne
        /// </summary>
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


