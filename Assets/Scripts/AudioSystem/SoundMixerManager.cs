using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AudioSystem
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button closePanelButton;
        [SerializeField] private AudioClip closeSound;

        private Animator _animator;
        private void Awake()
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
            closePanelButton.onClick.AddListener(ClosePanel);
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.Play("AudioPanelPopIn", 0);
            SoundManager.Instance.PlaySfx(closeSound, transform, 1f);
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("masterVolume", volume);
            PlayerPrefs.SetFloat("MasterVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("musicVolume", volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("soundEffectsVolume", volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        private void ClosePanel()
        {
            SoundManager.Instance.PlaySfx(closeSound, transform, 1f);
            gameObject.SetActive(false);
        }
    }
}
