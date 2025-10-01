using System.Collections;
using ThePurified.AudioSystem;
using ThePurified.PlayerSystem;
using UnityEngine;

namespace ThePurified.Items 
{
    /// <summary>
    /// klasa obslugujaca logike nosa misia na ktory mozna kliknac zeby otworzyc klapkê z kluczykiem.
    /// </summary>
    public class TeddyNose : InspectionItem
    {
        [Tooltip("koncowa rotacja po nacisnieciu na nosek")]
        [SerializeField] Quaternion endRotation;
        [SerializeField] float rotationDuration;

        [SerializeField] Transform teddyDoor;
        [SerializeField] Quaternion endDoorRotation;
       
        public override void OnPressed()
        {
            AudioManager.instance.PlaySoundInPosition("teddy nose", transform.position);
            StartCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            float elapsed = 0;

            Quaternion start = transform.localRotation;
            while (elapsed < rotationDuration)
            {
                transform.localRotation = Quaternion.Lerp(start, endRotation, elapsed);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = endRotation;

            yield return StartCoroutine(OpenTeddy());

        }

        private IEnumerator OpenTeddy()
        {
            //TODO: jakis dzwiêk otwierania misia
            float elapsed = 0;

            Quaternion start = teddyDoor.localRotation;
            while (elapsed < rotationDuration)
            {
                teddyDoor.localRotation = Quaternion.Lerp(start, endDoorRotation, elapsed); 
                elapsed += Time.deltaTime;
                yield return null;
            }

            teddyDoor.localRotation = endDoorRotation;
        }
    }
}


