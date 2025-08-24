using System.Collections;
using ThePurified.Items;
using UnityEngine;

using ThePurified.PlayerSystem;
using UnityEngine.Events;
using ThePurified.AI;
using System.Linq;

namespace ThePurified.Ai
{
    ///<summary>
    ///klasa przechowujaca logike obslugiwania zagadki znajdującej sie na clown'ie
    ///</summary>
    public class ClownRiddle : GameItem
    {
        [Tooltip("glowa gracza (obiekt ktory sledzi cinemachineCamera)")]
        [SerializeField] Transform playerHead;

        private Vector3 defaultPos;

        [Tooltip("Pozycja gdzie glowa bedzie się znajdywać gdy przybliży się ona do zagadki.")]
        [SerializeField] Transform zoomPos;

        [Tooltip("czas trwania przyblizania glowy")]
        [SerializeField] private float zoomTime = 1f;

        [Tooltip("krzywa animacji przyblizania")]
        [SerializeField] AnimationCurve zoomCurve;
        [Tooltip("krzywa animacji przesuwania kratki pokazujacej jakie pole zagadki gracz wybral")]
        [SerializeField] AnimationCurve boxCurve;
        public bool inInteraction = false;

        private Coroutine currentCoroutine;

        [Header("Kontrolowanie zagadki")]
        [Tooltip("punkty po ktorych przesuwa sie kratka pokazujacej jakie pole zagadki gracz wybral. Powinny byc to srodki wglebien na kostki w zagadce na clownie. Kratka bedzie tych pozycji uzywac do interpolacji swojej pozycji")]
        [SerializeField] Transform[] movingPoints;
        private bool[] slots; //tablica przechowujaca informacje o tym ktore pola sa wolne a ktore nie.

        [Header("Cube Parts - czesci zagadki")]
        [Tooltip("Kostki ktore wyswietlaja sie gdy gracz w trakcie zagadki naciska strzalke w dol oraz strzalke w gore. Umiesc je na tej liscie tak, zeby tworzyly haslo. Np: jesli haslo to czerwone, zielone, zolte, niebieskie to lista powinna od gory wygladac tak: czerwone, zielone, zolte, niebieskie.")]
        [SerializeField] GameObject[] cubeParts;
        [Tooltip("srodek kratki pokazujacej jakie pole gracz wybral")]
        [SerializeField] Transform centre;
        [Tooltip("kratka pokazujaca jakie pole zagadki gracz wybral")]
        [SerializeField] Transform box;

        [Tooltip("jak szybko kratka pokazujaca jakie pole gracz wybral porusza sie miedzy polami?")]
        [SerializeField] float boxSpeed;

        private bool movingBox = false;

        int currentIndex = 0;

        int currentCubePartIndex = 0;

        [Header("On Correct Password")]
        [Tooltip("Event ktory wywoluje sie gdy poprawne haslo zostalo ulozone przez gracza")]
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
            if (!inInteraction && (!ClownSystem.activated || ClownSystem.instance.generators.Length == 0))
            {
                StartCoroutine(ZoomIn());
            }
           /* else
            {
                if (inInteraction) Debug.Log("jest juz w interakcji!");
                if (ClownSystem.activated) Debug.Log("system clowna jest aktywowany");
            } */
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
                //Debug.Log("haslo poprawne!");
                StartCoroutine(Zoom(defaultPos));
                onCorrectPasswordEvent.Invoke();
                enabled = false; //wylacz ten skrypt skoro haslo poprawne
            }
        }

        ///<summary>
        ///Sluzy do oddalnia kamery od pozycji przyblizenia do pozycji przed przyblizeniem.
        ///</summary>
        public IEnumerator ZoomOut()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            yield return currentCoroutine = StartCoroutine(Zoom(defaultPos));
        }

        ///<summary>
        ///Sluzy do przyblizenia kamery od pozycji aktualnej do pozycji przyblizonej
        ///</summary>
        private IEnumerator ZoomIn()
        {
            PlayerMovement.movementEnabled = false;
            PlayerHeadBob.headBobEnabled = false;

            defaultPos = playerHead.position;
            inInteraction = true;
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            yield return currentCoroutine = StartCoroutine(Zoom(zoomPos.position));
        }

        ///<summary>
        ///Sluzy do przyblizania {playerHead} (obiektu sledzonego przez cinemachine) do pozycji {pos}
        ///</summary>
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

        ///<summary>
        ///Obsluguje poruszanie sie kwadratu / kratki w trakcie rozwiazywania zagadki za pomoca strzalek.
        ///</summary>
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

        /// <summary>
        /// Obsluguje umieszczanie sześciennych czesci w zagadce.
        /// </summary>
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
        /// <summary>
        /// porusza kwadracikiem po zagadce. Kwadracik pokazuje aktualnie wybrane pole.
        /// </summary>
        /// <param name="pos"> pozycja do ktorej ma ruszyć się kwadracik </param>
        /// <returns></returns>
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

        /// <summary>
        /// sprawdza czy aktualnie ulozone haslo jest poprawne
        /// </summary>
        /// <returns>to czy haslo jest poprawnie wlozone do robota </returns>
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

