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
        [Header("InspectPoint")]
        [Tooltip("point in front of camera where player will rotate object using mouse")]
        public Transform inspectPoint;
        [Header("Rotation Speed")]
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

        [Header("Global volume for depth of field")]
        [Tooltip("Inspection controller uses depth of field to make blurry effect behind inspected object")]
        [SerializeField] Volume volume;
        [SerializeField] private float depthNormalValue;
        [SerializeField] private float depthInspectedValue;

        [Header("Interpolation Time")]
        [Tooltip("How fast inspected object moves to inspect point")]
        [SerializeField] float interpolationSpeed = 2f;


        private void Start()
        {
            if (instance == null)
                instance = this;
        }

        public void SetItemToInspect(GameObject item, bool destroyObject = false)
        {
            controller.enabled = false;
            PlayerMovement.movementEnabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            inspectedObjectOriginalPos = item.transform.position;

            inspectedObjectOriginalRotation = item.transform.rotation;

            inspectedObjectParent = item.transform.parent;

            StartCoroutine(InterpolateToInspectPoint(item, inspectPoint.position, inspectPoint));

            StartCoroutine(ObjectInspecting(destroyObject));
        }

        private IEnumerator InterpolateToInspectPoint(GameObject item, Vector3 point, Transform parent)
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

                if(inspection != null)
                    inspection.WhileInspection();

                yield return null;
            }

            if(inspection!=null)
                inspection.OnInspectionEnd();


            controller.enabled = true;

            if (destroyObject)
            {
                Destroy(inspectPoint.GetChild(0).gameObject);
            }
            else
            {
                inspectPoint.GetChild(0).gameObject.transform.rotation = inspectedObjectOriginalRotation;
                StartCoroutine(InterpolateToInspectPoint(inspectPoint.GetChild(0).gameObject, inspectedObjectOriginalPos, inspectedObjectParent));
            }

            PlayerMovement.movementEnabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isInspecting = false;
        }

        private void SetDepthOfField(float value)
        {
            if (volume.profile.TryGet<DepthOfField>(out DepthOfField d))
            {
                d.gaussianEnd.value = value;
            }
        }
    }
}

