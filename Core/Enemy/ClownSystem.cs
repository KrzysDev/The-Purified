using System.Collections;
using ThePurified.Items;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using ThePurified.Ai;
using UnityEngine.Events;

namespace ThePurified.AI
{   
    /// <summary>
    /// klasa obslugujaca logike AI Robo-Clown'a.
    /// </summary>
    [RequireComponent(typeof(ClownRiddle))]
    public class ClownSystem : MonoBehaviour
    {
        [Header("NavMesh Agent")]
        [SerializeField] NavMeshAgent clownAgent;
        [Header("Player: ")]
        [Tooltip("PlayerBody za ktorym AI podaza")]
        [SerializeField] Transform target;

        public static bool activated = true;

        public static ClownSystem instance;

        Coroutine currentCoroutine;


        [Header("Czas deaktywacji")]
        [Tooltip("najkrotszy czas przez jaki clown moze byc wylaczony")]
        [SerializeField] float minTime = 20f;
        [Tooltip("najdluzszy czas przez jaki clown moze byc wylaczony")]
        [SerializeField] float maxTime = 40f;

        private bool isDeactivating = false;


        [Tooltip("wszystkie generatory na mapie")]
        [SerializeField] List<Generator> generators = new List<Generator>();

        private ClownRiddle riddle; //zagadka na clownie

        [Header("Schowki z kostkami")]
        [SerializeField] StorageCompartment[] compartments;

        [Header("Gdy generatory sie wlacza: ")]
        [SerializeField] UnityEvent onAllGeneratorsOn;
        [Header("Gdy wszystkie generatory sie wlacza: ")]
        [SerializeField] UnityEvent onAllGeneratorsOff;


        void Start()
        {
            instance = this;

            riddle = GetComponent<ClownRiddle>();

            //ActivateAI();
        }

        public void ActivateAI()
        {
            currentCoroutine = StartCoroutine(ActivateClown());
        }

        private void Update()
        {
            if (!Generator.allGeneratorsAreOff)
            {
                //Debug.Log("sprawdzam czy wszystkie generatory sa wylaczone");
                Generator.allGeneratorsAreOff = AllGeneratorsAreOff();
                //Debug.Log("sprawdzilem");
            }

            if (Generator.allGeneratorsAreOff && activated && !isDeactivating)
            {
                //Debug.Log("dezaktywuje clown'a");
                StartCoroutine(Deactivate(Random.Range(minTime, maxTime)));
                SetStorageCompartments(true);
            }
        }

        /// <summary>
        /// Sprawdza czy wszystkie generatory sa wylaczone
        /// </summary>
        /// <returns> prawda jesli wszystkie generatory sa wylaczone </returns>
        bool AllGeneratorsAreOff()
        {
            for (int i = 0; i < generators.Count; i++)
            {
                if (generators[i].active)
                {
                    //Debug.Log("ktorys generator jest ON (zwracam falsz)");
                    return false;
                }

            }
            //Debug.Log("wszystkie sa OFF");
            return true;
        }

        /// <summary>
        /// ustawia cel podazania AI
        /// </summary>
        /// <param name="position"> pozycja za ktora AI podaza </param>
        private void SetDestination(Transform position)
        {
            clownAgent.SetDestination(position.position);
        }

        /// <summary>
        /// korutyna aktywujaca clowna. 
        /// </summary>
        private IEnumerator ActivateClown()
        {
            yield return new WaitForSeconds(2f); //TODO: tutaj jakas animacja wstawania budzenia sie czy cos takiego.

            if (riddle.inInteraction)
            {
                StartCoroutine(riddle.ZoomOut());
            }

            while (activated)
            {
                SetDestination(target);
                yield return new WaitForSeconds(0.1f);
            }
        }
        ///<summary>
        ///korutyna usypiajaca clowna
        ///</summary>
        public IEnumerator Deactivate(float seconds)
        {
            clownAgent.isStopped = true;
            clownAgent.ResetPath();

            isDeactivating = true;

            Debug.Log($"Clown deaktywowany na {seconds}");

            onAllGeneratorsOff.Invoke();

            activated = false;

            yield return new WaitForSeconds(seconds);

            onAllGeneratorsOn.Invoke();

            activated = true;

            //Debug.Log("koniec deaktywacji powrot....");
            clownAgent.isStopped = false;
            clownAgent.ResetPath();

            ActivateAllGenerators();
            SetStorageCompartments(false);

            currentCoroutine = StartCoroutine(ActivateClown());

            isDeactivating = false;
        }

        /// <summary>
        /// aktywuje wszystkie generatory spowrotem
        /// </summary>
        private void ActivateAllGenerators()
        {
            Generator.allGeneratorsAreOff = false;
            for (int i = 0; i < generators.Count; i++)
            {
                generators[i].TurnOn();
            }
        }

        /// <summary>
        /// otwiera lub zamyka schowki z cube'ami.
        /// </summary>
        /// <param name="value"></param>
        private void SetStorageCompartments(bool value)
        {
            if (value)
            {
                foreach (StorageCompartment comp in compartments)
                {
                    comp.Open();
                }
            }
            else
            {
                foreach (StorageCompartment comp in compartments)
                {
                    comp.Close();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StopCoroutine(currentCoroutine);
                //Debug.Log("zlapal gracza!");
            }
        }
    }
}
