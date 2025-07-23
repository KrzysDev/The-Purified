using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThePurified.AudioSystem
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        public string tag;
        public string name;
        public float volume;
        public float pitch;
        public bool loop;

        public float maxDistance, minDistance;

    }
    public class AudioManager : MonoBehaviour
    {
        [Header("Sounds: ")]
        [SerializeField] List<Sound> sounds;

        public static AudioManager instance;

        private int previousIndex = 0;

        public void Awake()
        {
            instance = this;
        }


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

            int randomIndex = Random.Range(0, matching.Count);

            PlaySound(matching[randomIndex].name);
        }

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

            do
            {
                randomIndex = Random.Range(0, matching.Count);
            }
            while (randomIndex == previousIndex);

            PlaySoundInPosition(matching[randomIndex].name, pos);
        }

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

            do
            {
                randomIndex = Random.Range(0, matching.Count);
            }
            while (randomIndex == previousIndex);

            float randomPitch = Random.Range(minPitch, maxPitch);

            matching[randomIndex].pitch = randomPitch;

            PlaySoundInPosition(matching[randomIndex].name, pos);
        }
    }
}


