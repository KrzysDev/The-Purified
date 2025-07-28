using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ThePurified.PlayerSystem
{
    interface IInspection
    {
        void OnInspection();
        void WhileInspection();
        void OnInspectionEnd();
    }
    public class InspectController : MonoBehaviour
    {
        [Header("Inspect Point")]
        [Tooltip("punkt przed kamera ktory gracz bedzie rotowal uzywajac myszy")]
        public Transform inspectPoint;
        [Header("predkosc rotacji")]
        [SerializeField] float rotationSpeed = 2f;

        public static KeyCode leaveInspection = KeyCode.Tab;

        private Vector3 previousMousePos;

        public static InspectController instance;

        public static bool isInspecting = false;

        private Vector3 inspectedObjectOriginalPos;
        private Quaternion inspectedObjectOriginalRotation;

        private Transform inspectedObjectParent;

        [Header("Cinemachine Input Axis Controller")]
        [SerializeField] CinemachineInputAxisController controller;

        [SerializeField] private float depthNormalValue;
        [SerializeField] private float depthInspectedValue;

        [Header("Czas interpolacji")]
        [Tooltip("jak szybko obiekt interpoluje swoja pozycje do inspect point")]
        [SerializeField] float interpolationSpeed = 2f;

        //przyblizanie obiektu
        private float mouseWheelInput;
        private Vector3 inspectPointOriginalPos;
        [Header("Przyblizanie obiektu inspektowanego")]
        [SerializeField] float minzoom = -2f;
        [SerializeField] float  maxZoom = 2f;
        [SerializeField] float zoomSpeed = 2f;

        private float currentZoom = 0f;

        private void Start()
        {
            if (instance == null)
                instance = this;

            inspectPointOriginalPos = inspectPoint.localPosition;
        }


        /// <summary>
        /// ustawia GameObject do inspekcji
        /// </summary>
        /// <param name="item"> gameobject ktory ma byc ustawiony do inspekcji </param>
        /// <param name="destroyObject"> jesli prawda, zniszczy obiekt po wyjsciu z inspekcji </param>
        public void SetItemToInspect(GameObject item, bool destroyObject = false)
        {
            controller.enabled = false;
            PlayerMovement.movementEnabled = false;

            Cursor.lockState = CursorLockMode.None;

            inspectedObjectOriginalPos = item.transform.position;

            inspectedObjectOriginalRotation = item.transform.rotation;

            inspectedObjectParent = item.transform.parent;

            StartCoroutine(InterpolateToPoint(item, inspectPoint.position, inspectPoint));

            StartCoroutine(ObjectInspecting(destroyObject));
        }

        /// <summary>
        /// interpoluje pozyycje gameobjectu do punktu
        /// </summary>
        /// <param name="item"> gameobject ktorego pozycja jest interpolowana </param>
        /// <param name="point"> punkt do ktorego pozycja jest interpolowana </param>
        /// <param name="parent"> rodzic gameobjectu po interpolacji </param>
        /// <returns></returns>
        private IEnumerator InterpolateToPoint(GameObject item, Vector3 point, Transform parent)
        {
            item.transform.SetParent(parent, true);

            float elapsed = 0f;

            while (elapsed < 1f)
            {
                item.transform.position = Vector3.Lerp(item.transform.position, point, elapsed);

                elapsed += Time.deltaTime * interpolationSpeed;

                yield return null;
            }

            if (parent != null)
            {
                item.transform.localPosition = new Vector3(0, 0, 0);
            }

            item.transform.position = point;

        }

        /// <summary>
        /// przechowuje logike inspekcji przedmiotu
        /// </summary>
        /// <param name="destroyObject">jesli prawda, niszczy obiekt po zakonczeniu inspekcji</param>
        /// <returns></returns>
        private IEnumerator ObjectInspecting(bool destroyObject = false)
        {

            IInspection inspection = null;

            if (inspectPoint.transform.GetChild(0).TryGetComponent(out IInspection i))
            {
                inspection = i;
                inspection.OnInspection();
            }

            isInspecting = true;

            while (!Input.GetKeyDown(leaveInspection))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    previousMousePos = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 deltaMousePosition = Input.mousePosition - previousMousePos;
                    float rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
                    float rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

                    Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

                    inspectPoint.localRotation = rotation * inspectPoint.localRotation;

                    previousMousePos = Input.mousePosition;
                }

                if (inspection != null)
                    inspection.WhileInspection();

                HandleInspectPointZooming();

                yield return null;
            }

            if (inspection != null)
                inspection.OnInspectionEnd();


            controller.enabled = true;

            if (destroyObject)
            {
                Destroy(inspectPoint.GetChild(0).gameObject);
            }
            else
            {
                inspectPoint.GetChild(0).gameObject.transform.rotation = inspectedObjectOriginalRotation;
                StartCoroutine(InterpolateToPoint(inspectPoint.GetChild(0).gameObject, inspectedObjectOriginalPos, inspectedObjectParent));
            }

            PlayerMovement.movementEnabled = true;

            Cursor.lockState = CursorLockMode.Locked;

            ResetInspectPoint();

            isInspecting = false;
        }


        private void HandleInspectPointZooming()
        {
            mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(mouseWheelInput) > 0.01f)
            {
                currentZoom += mouseWheelInput * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minzoom, maxZoom);
                inspectPoint.localPosition = inspectPointOriginalPos + Vector3.forward * currentZoom;
            }
                
        }

        private void ResetInspectPoint()
        {
            inspectPoint.localPosition = inspectPointOriginalPos;
        }
        
    }
}

