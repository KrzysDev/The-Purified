using UnityEngine;
using ThePurified.AI;
using System.Collections;

namespace ThePurified.Items
{
    public class Generator : GameItem
    {

        public static bool allGeneratorsAreOff = false;

        public bool active = true;

        [Header("Lever")]
        [SerializeField] Transform lever;
        [SerializeField] private float onAngle;
        [SerializeField] private float offAngle;

        [SerializeField] private float leverAnimationTime = 1f;
        private float elapsed = 0f;


        [Header("Lamp materials")]
        [SerializeField] Material redMaterial;
        [SerializeField] Material greenMaterial;

        private Material defaultMaterial;

        [SerializeField] Renderer onLamp;
        [SerializeField] Renderer offLamp;

        private static float waitForOtherGeneratorsTime = 25f;


        public override void ItemStart()
        {
            defaultMaterial = offLamp.material;
            onLamp.material = greenMaterial;
        }

        public override void OnItemInteract()
        {
            if (active)
            {
                TurnOff();
            }
        }

        public void TurnOn()
        {
            active = true;

            onLamp.material = greenMaterial;
            offLamp.material = defaultMaterial;

            StartCoroutine(HandleLever(Quaternion.Euler(lever.transform.rotation.x, transform.rotation.y, onAngle)));
        }

        private void TurnOff()
        {
            active = false;
            onLamp.material = defaultMaterial;
            offLamp.material = redMaterial;

            StartCoroutine(HandleLever(Quaternion.Euler(lever.transform.rotation.x, transform.rotation.y, offAngle)));
        }

        private IEnumerator HandleLever(Quaternion end)
        {
            Quaternion start = lever.transform.rotation;

            elapsed = 0f;
            
            while (elapsed < leverAnimationTime)
            {
                elapsed += Time.deltaTime;
                lever.transform.rotation = Quaternion.Lerp(start, end, elapsed);
                yield return null;
            }

            StartCoroutine(WaitForOtherGenerators());
        }

        private IEnumerator WaitForOtherGenerators()
        {
            yield return new WaitForSeconds(waitForOtherGeneratorsTime);

            if (!allGeneratorsAreOff)
            {
                TurnOn();
            }
        }
    }
}

