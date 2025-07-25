using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    public class Lamp : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] Material onMaterial;
        [SerializeField] Material offMaterial;

        [SerializeField] GameObject lampsLight;

        private Renderer rend;
        void Start()
        {
            audioSource = AudioManager.instance.GetAndPlaySoundInPosition("Lamp", transform.position);

            rend = GetComponent<MeshRenderer>();
        }

        public void TurnOn()
        {
            Debug.Log("wywołano turn on w Lamp");
            audioSource.UnPause();
            rend.material = onMaterial;
            lampsLight.SetActive(true);
        }

        public void TurnOff()
        {
            //Debug.Log("wywołano turn off w Lamp");
            audioSource.Pause();
            //Debug.Log("zapałzowano audiosource");
            rend.material = offMaterial;
            //Debug.Log("zmieniono material");
            lampsLight.SetActive(false);
            //Debug.Log("wylaczono swiatlo");
        }
    }
}

