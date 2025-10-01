using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThePurified.AudioSystem
{
    [System.Serializable]
    ///<summary>
    //Klasa przechowujaca wszystkie wlasciwosci dzwieku w grze.
    ///</summary>
    public class Sound
    {
        public AudioClip clip;
        public bool onAwake = false;
        public string name;
        public string tag;
        public float volume;
        public float pitch;
        public bool loop;

        public float maxDistance, minDistance;

    }

    public class AudioManager : MonoBehaviour
    {
        [Header("Sounds: ")]
        [Tooltip("List of sounds in the game")]
        [SerializeField] List<Sound> sounds;

        public static AudioManager instance;

        public void Awake()
        {
            instance = this; //gra jest skonstruowana w ten sposob, ze audiomanager w sumie nie potrzebowal miec singletonu.

            ActivateOnAwakeSounds();
        }


        ///<summary>
        //puszcza dzwiek o nazwie {name}. Dzwiek puszczony ta funkcja nie jest przestrzenny.
        ///</summary>
        public void PlaySound(string name)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            Sound s = sounds.Find(s => s.name == name);

            if (s == null)
            {
                Debug.LogError($"There is no sound called {name}");
                return;
            }

            GameObject soundObject = new GameObject($"SFX_{name}");

            AudioSource audio = soundObject.AddComponent<AudioSource>();

            audio.clip = s.clip;

            audio.volume = s.volume;

            audio.pitch = s.pitch;

            audio.loop = s.loop;

            audio.Play();

            if (!s.loop)
                Destroy(soundObject, s.clip.length);

        }

        public void PlaySound(string name, float minPitch, float maxPitch)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            Sound s = sounds.Find(s => s.name == name);

            if (s == null)
            {
                Debug.LogError($"There is no sound called {name}");
                return;
            }

            GameObject soundObject = new GameObject($"SFX_{name}");

            AudioSource audio = soundObject.AddComponent<AudioSource>();

            audio.clip = s.clip;

            audio.volume = s.volume;

            audio.pitch = Random.Range(minPitch, maxPitch);

            audio.loop = s.loop;

            audio.Play();

            if (!s.loop)
                Destroy(soundObject, s.clip.length);

        }

        ///<summary>
        //Odtwarza dzwiek o nazwie {name} w pozycji {position}
        ///</summary>
        public void PlaySoundInPosition(string name, Vector3 position)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            Sound s = sounds.Find(s => s.name == name);

            if (s == null)
            {
                Debug.LogError($"There is no sound called {name}");
                return;
            }

            GameObject soundObject = new GameObject($"SFX_{name}");

            AudioSource audio = soundObject.AddComponent<AudioSource>();

            audio.clip = s.clip;

            audio.volume = s.volume;

            audio.pitch = s.pitch;

            audio.loop = s.loop;

            audio.spatialBlend = 1;

            audio.maxDistance = s.maxDistance;
            audio.minDistance = s.minDistance;

            soundObject.transform.position = position;

            audio.Play();

            if (!audio.loop)
                Destroy(soundObject, audio.clip.length);
        }
        /// <summary>
        /// Odtwarza dzwiek w pozycji {position} z losowym pitchem.
        /// </summary>
        /// <param name="name">nazwa dzwieku</param>
        /// <param name="position">pozycja dzwieku w swiecie gry</param>
        /// <param name="minPitch">najmniejsza wartosc losowa pitcha</param>
        /// <param name="maxPitch">najwieksza wartosc losowa pitcha</param>
        public void PlaySoundInPosition(string name, Vector3 position, float minPitch, float maxPitch)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            Sound s = sounds.Find(s => s.name == name);

            if (s == null)
            {
                Debug.LogError($"There is no sound called {name}");
                return;
            }

            GameObject soundObject = new GameObject($"SFX_{name}");

            AudioSource audio = soundObject.AddComponent<AudioSource>();

            audio.clip = s.clip;

            audio.volume = s.volume;

            audio.pitch = Random.Range(minPitch, maxPitch);

            audio.loop = s.loop;

            audio.spatialBlend = 1;

            audio.maxDistance = s.maxDistance;
            audio.minDistance = s.minDistance;

            soundObject.transform.position = position;

            audio.Play();

            if (!audio.loop)
                Destroy(soundObject, audio.clip.length);
        }

        /// <summary>
        /// Odtwarza dzwiek o nazwie {nazwa} na pozycji {position}
        /// </summary>
        /// <param name="name">nazwa dzwieku</param>
        /// <param name="position">pozycja dzwieku</param>
        /// <returns>AudioSource z tym dzwiekiem</returns>
        public AudioSource GetAndPlaySoundInPosition(string name, Vector3 position)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return null;
            }

            Sound s = sounds.Find(s => s.name == name);

            if (s == null)
            {
                Debug.LogError($"There is no sound called {name}");
                return null;
            }

            GameObject soundObject = new GameObject($"SFX_{name}");

            AudioSource audio = soundObject.AddComponent<AudioSource>();

            audio.clip = s.clip;

            audio.volume = s.volume;

            audio.pitch = s.pitch;

            audio.loop = s.loop;

            if (!audio.loop)
            {
                Debug.LogError("Cannot get AudioSource that is not looped for safety reasons.");
            }

            audio.spatialBlend = 1;

            audio.maxDistance = s.maxDistance;
            audio.minDistance = s.minDistance;

            soundObject.transform.position = position;

            audio.Play();

            return audio;

        }

        public AudioSource GetPlayingSoundWithName(string name)
        {
            AudioSource[] audioObjects = FindObjectsOfType<AudioSource>();

            foreach (AudioSource a in audioObjects)
            {
                if(a.gameObject.name == $"SFX_{name}")
                {
                    return a;
                }
            }

            return null;
        }
        ///<summary>
        //Puszcza losowy dzwiek z tagiem {tag}.
        ///</summary>
        int previousRandomIndex = 0;
        public void PlayRandomWithTag(string tag)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            var matching = sounds.Where(s => s.tag == tag).ToList();

            if (matching.Count == 0)
            {
                Debug.LogWarning($"There are no sounds with tag {tag}");
                return;
            }

            int randomIndex = 0;

            do
            {
                randomIndex = Random.Range(0, matching.Count);
            } while (previousRandomIndex == randomIndex);
            previousRandomIndex = randomIndex;

            PlaySound(matching[randomIndex].name);
        }

        ///<summary>
        //Puszcza losowy dzwiek z tagiem {tag} na pozycji {pos}
        ///</summary>
        public void PlayRandomWithTag(string tag, Vector3 pos)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            var matching = sounds.Where(s => s.tag == tag).ToList();

            if (matching.Count == 0)
            {
                Debug.LogWarning($"There are no sounds with tag {tag}");
                return;
            }

            int randomIndex;
            int previousIndex = 0;

            do
            {
                randomIndex = Random.Range(0, matching.Count);
            }
            while (randomIndex == previousIndex);

            previousIndex = randomIndex;

            PlaySoundInPosition(matching[randomIndex].name, pos);
        }

        ///<summary>
        //Puszcza losowy dzwiek z tagiem {tag} na pozycji {pos} z losową wysokością.
        ///</summary>
        public void PlayRandomWithTag(string tag, Vector3 pos, float minPitch, float maxPitch)
        {
            if (instance == null)
            {
                Debug.LogWarning("there is no AudioManager in this scene");
                return;
            }

            var matching = sounds.Where(s => s.tag == tag).ToList();

            if (matching.Count == 0)
            {
                Debug.LogWarning($"There are no sounds with tag {tag}");
                return;
            }

            int randomIndex;
            int previousIndex = 0;

            do
            {
                randomIndex = Random.Range(0, matching.Count);
            }
            while (randomIndex == previousIndex);

            float randomPitch = Random.Range(minPitch, maxPitch);

            matching[randomIndex].pitch = randomPitch;

            PlaySoundInPosition(matching[randomIndex].name, pos);
        }

        public void ActivateOnAwakeSounds()
        {
            foreach (var sound in sounds)
            {
                if (sound.onAwake)
                {
                    PlaySound(sound.name);
                }
            }
        }
    }
}


