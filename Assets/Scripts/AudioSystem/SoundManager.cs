using UnityEngine;

namespace AudioSystem
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSourcePrefab;
        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip gameMusic;
    
        public static SoundManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySfx(AudioClip clip, Transform spawnTransform, float volume = 1.0f)
        {
            var audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }

        public void SetMenuMusic()
        {
            backgroundMusic.clip = menuMusic;
            backgroundMusic.Play();
        }

        public void SetGameMusic()
        {
            backgroundMusic.clip = gameMusic;
            backgroundMusic.Play();
        }
    }
}
