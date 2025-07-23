using UnityEngine;
using ThePurified.PlayerSystem;

namespace ThePurified.Items
{
    public class InspectionItem : MonoBehaviour
    {
        private Camera mainCamera;
        void Start()
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            if (mainCamera == null)
            {
                Debug.LogError("There is no main camera! Are you sure you have main camera in the scene and its named properly 'Main Camera' ?");
            }
        }

        public virtual void OnHover() { }
        public virtual void OnPressed() { }
        public virtual void OnEndHover() { }

        void OnMouseEnter()
        {
            if (InspectController.isInspecting)
                OnHover();
        }

        void OnMouseExit()
        {
            if (InspectController.isInspecting)
                OnEndHover();
        }

        void Update()
        {
            HandlePressing();
        }

        private void HandlePressing()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        OnPressed();
                    }
                }
            }
        }


    }
}

