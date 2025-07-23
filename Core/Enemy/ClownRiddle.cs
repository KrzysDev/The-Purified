using System.Collections;
using ThePurified.Items;
using UnityEngine;

using ThePurified.PlayerSystem;
using UnityEngine.Events;
using ThePurified.AI;

namespace ThePurified.Ai
{
    public class ClownRiddle : GameItem
    {
        [SerializeField] Transform playerHead;

        private Vector3 defaultPos;

        [SerializeField] Transform zoomPos;

        [SerializeField] private float zoomTime = 1f;

        [SerializeField] AnimationCurve zoomCurve;
        [SerializeField] AnimationCurve boxCurve;
        public bool inInteraction = false;

        private Coroutine currentCoroutine;

        [Header("Controlling Riddle")]
        [SerializeField] Transform[] movingPoints;
        private bool[] slots;
        [Header("Cube Parts (place in password order)")]
        [Tooltip("place them on the list so they create password. f.e if password is green, blue, red, yellow place cubes in such order. ")]
        [SerializeField] GameObject[] cubeParts;
        [SerializeField] Transform centre;
        [SerializeField] Transform box;

        [SerializeField] float boxSpeed;

        private bool movingBox = false;

        int currentIndex = 0;

        int currentCubePartIndex = 0;

        [Header("On Correct Password")]
        [SerializeField] UnityEvent onCorrectPasswordEvent;

        public override void ItemStart()
        {
            slots = new bool[movingPoints.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = false;
            }
        }


        #region camera zoom
        public override void OnItemInteract()
        {
            if (!inInteraction && !ClownSystem.activated)
            {
                PlayerMovement.movementEnabled = false;
                PlayerHeadBob.headBobEnabled = false;

                defaultPos = playerHead.position;
                inInteraction = true;
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);

                currentCoroutine = StartCoroutine(Zoom(zoomPos.position));
            }
            else
            {
                if (inInteraction) Debug.Log("jest juz w interakcji!");
                if (ClownSystem.activated) Debug.Log("system clowna jest aktywowany");
            }
        }

        public override void ItemUpdate()
        {
            if (inInteraction && Input.GetKeyDown(KeyCode.Tab))
            {
                StartCoroutine(ZoomOut());
            }

            //poruszanie znacznikiem po zagadce
            HandleBoxMoving();

            //ustawianie cube parts
            HandleCubePartPlacing();

            if (CorrectPassword())
            {
                Debug.Log("haslo poprawne!");
                StartCoroutine(Zoom(defaultPos));
                onCorrectPasswordEvent.Invoke();
                enabled = false; //wylacz ten skrypt skoro haslo poprawne
            }
        }

        public IEnumerator ZoomOut() //funkcja uzywana ze skryptu clown system - dlatego tutaj jest
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            yield return currentCoroutine = StartCoroutine(Zoom(defaultPos));
        }


        private IEnumerator Zoom(Vector3 pos)
        {

            float elapsed = 0f;
            Vector3 start = playerHead.transform.position;

            while (elapsed < zoomTime)
            {
                float t = zoomCurve.Evaluate(elapsed / zoomTime);
                playerHead.position = Vector3.Lerp(start, pos, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            playerHead.position = pos;

            if (pos == defaultPos)
            {
                PlayerMovement.movementEnabled = true;
                PlayerHeadBob.headBobEnabled = true;
                inInteraction = false;

                cubeParts[currentIndex].SetActive(false);
            }

        }

        #endregion

        #region riddle 
        private void HandleBoxMoving()
        {
            if (inInteraction)
            {
                if (!movingBox)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        //Debug.Log("lewa strzalka");
                        currentIndex--;

                        if (currentIndex < 0)
                            currentIndex = movingPoints.Length - 1;

                        StartCoroutine(MoveBox(movingPoints[currentIndex].position));
                    }

                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        //Debug.Log("prawa strzalka");
                        currentIndex++;

                        if (currentIndex > movingPoints.Length - 1)
                            currentIndex = 0;


                        StartCoroutine(MoveBox(movingPoints[currentIndex].position));
                    }


                }
            }
        }

        private void HandleCubePartPlacing()
        {
            if (!movingBox && inInteraction)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cubeParts[currentCubePartIndex].SetActive(false);

                    currentCubePartIndex--;

                    if (currentCubePartIndex < 0)
                        currentCubePartIndex = cubeParts.Length - 1;

                    if (!slots[currentIndex])
                        cubeParts[currentCubePartIndex].SetActive(true);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cubeParts[currentCubePartIndex].SetActive(false);

                    currentCubePartIndex++;

                    currentCubePartIndex = currentCubePartIndex % cubeParts.Length;

                    if (!slots[currentIndex])
                        cubeParts[currentCubePartIndex].SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //Debug.Log("nacisnieto enter");
                    if (cubeParts[currentCubePartIndex].activeSelf && !slots[currentIndex])
                    {
                        GameObject o = Instantiate(cubeParts[currentCubePartIndex], centre.position, Quaternion.identity, centre);
                        o.transform.SetParent(movingPoints[currentIndex], true);
                        o.name = cubeParts[currentCubePartIndex].name;
                        slots[currentIndex] = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (slots[currentIndex])
                    {
                        Destroy(movingPoints[currentIndex].transform.GetChild(0).gameObject);
                        slots[currentIndex] = false;

                        if (cubeParts[currentIndex].activeSelf)
                        {
                            cubeParts[currentIndex].SetActive(false);
                        }
                    }
                }
            }
            else if (movingBox)
            {
                cubeParts[currentCubePartIndex].SetActive(false);
            }
        }

        private IEnumerator MoveBox(Vector3 pos)
        {
            movingBox = true;

            float elapsed = 0f;
            Vector3 start = box.transform.position;

            while (elapsed < boxSpeed)
            {
                float t = boxCurve.Evaluate(elapsed / boxSpeed);

                box.transform.position = Vector3.Lerp(start, pos, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            box.transform.position = pos;

            movingBox = false;

            //Debug.Log("koniec przesuwania");
        }


        private bool CorrectPassword()
        {
            for (int i = 0; i < movingPoints.Length; i++)
            {
                if (movingPoints[i].childCount < 1)
                    return false;
            }

            Debug.Log("wszystkie movingPoints maja dzieci");

            for (int i = 0; i < movingPoints.Length; i++)
            {
                if (movingPoints[i].GetChild(0).gameObject.name != cubeParts[i].gameObject.name)
                {
                    Debug.Log(movingPoints[i].GetChild(0).gameObject.name + " !=" + cubeParts[i].gameObject.name);
                    return false;
                }
                    
            }

            return true;
        }

        #endregion

    }
}

