using UnityEngine;
using ThePurified.PlayerSystem;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez itemy z ktorymi mozna wejsc w interkacje po inspekcji innego itemu.
    /// Np gracz wchodzi w interakcje z nosem klauna na poczatku gry -> inspeckja + obrot nosa -> klika na klucz myszka -> interakcja 
    /// </summary>
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

        /// <summary>
        /// Gdy gracz najedzie myszka
        /// </summary>
        public virtual void OnHover() { }
        /// <summary>
        /// gdy gracz nacisnie myszke na obiekcie
        /// </summary>
        public virtual void OnPressed() { }
        /// <summary>
        /// gdy gracz przestanie najezdzac myszka
        /// </summary>
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

