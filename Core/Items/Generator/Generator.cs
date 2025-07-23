using UnityEngine;
using ThePurified.AI;
using System.Collections;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez generator ktory sluzy do odcinania zasilania w placowce.
    /// </summary>
    public class Generator : GameItem
    {
        public static bool allGeneratorsAreOff = false;

        public bool active = true;

        [Header("Dzwignia")]
        [Tooltip("Dzwignia ktora bedzie opadac gdy generator sie wylaczy")]
        [SerializeField] Transform lever;
        [Tooltip("kat ktory ma dzwignia gdy generator jest wlaczony")]
        [SerializeField] private float onAngle;
        [Tooltip("kat ktory ma dzwignia gdy generator jest wylaczony")]
        [SerializeField] private float offAngle;

        [Tooltip("jak dlugo dzwignia sie podnosi? (sekundy)")]
        [SerializeField] private float leverAnimationTime = 1f;
        private float elapsed = 0f;


        [Header("materialy lampek - czerwonej i zielonej ktore pokazuja czy generator jest wlaczony czy nie")]
        [SerializeField] Material redMaterial;
        [SerializeField] Material greenMaterial;

        private Material defaultMaterial;

        [Tooltip("Lampka ktora sie zapala gdy generator jest wlaczony")]
        [SerializeField] Renderer onLamp;
        [Tooltip("odwrotnie niz wyzej")]
        [SerializeField] Renderer offLamp;

        private static float waitForOtherGeneratorsTime = 50f;


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

        /// <summary>
        /// wlacza generator
        /// </summary>
        public void TurnOn()
        {
            active = true;

            onLamp.material = greenMaterial;
            offLamp.material = defaultMaterial;

            StartCoroutine(HandleLever(Quaternion.Euler(lever.transform.rotation.x, transform.rotation.y, onAngle)));
        }

        /// <summary>
        /// wylacza generator
        /// </summary>
        private void TurnOff()
        {
            active = false;
            onLamp.material = defaultMaterial;
            offLamp.material = redMaterial;

            StartCoroutine(HandleLever(Quaternion.Euler(lever.transform.rotation.x, transform.rotation.y, offAngle)));
        }

        /// <summary>
        /// obsluguje rotacje dzwigni
        /// </summary>
        /// <param name="end">rotacja do ktorej interpoluje dzwignia</param>
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
        /// <summary>
        /// funkcja czekajaca na inne generatory. Jesli gracz nie zdazy wszystkich wylaczyc po sobie w odpowiednim odstepie czasu to generator wlaczy sie spowrotem.
        /// </summary>
        /// <returns></returns>
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

