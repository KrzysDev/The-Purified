using UnityEngine;

namespace ThePurified.Items
{
    public class StorageCompartment : MonoBehaviour
    {
        [Tooltip("drzwiczki ktore beda znikac / otwierac sie gdy wszystkie generatory sa wylaczone")]
        [SerializeField] GameObject door;

        public void Open()
        {
            door.SetActive(false);
        }

        public void Close()
        {
            door.SetActive(true);
        }
    }
}


