using UnityEngine;

namespace PointClickTemplate
{
    public sealed class AudioManager : MonoBehaviour
    {
        private const string MusicVolumeKey = "SETTINGS_MUSIC_VOLUME";
        private const string SfxVolumeKey = "SETTINGS_SFX_VOLUME";
        private const string MuteKey = "SETTINGS_MUTED";

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField, Range(0f, 1f)] private float defaultMusicVolume = 0.7f;
        [SerializeField, Range(0f, 1f)] private float defaultSfxVolume = 1f;

        private float musicVolume;
        private float sfxVolume;
        private bool muted;

        private void Awake()
        {
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume);
            sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, defaultSfxVolume);
            muted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
            ApplyVolumes();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicSource == null || clip == null)
            {
                return;
            }

            if (musicSource.clip == clip && musicSource.isPlaying)
            {
                return;
            }

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            if (sfxSource == null || clip == null)
            {
                return;
            }

            sfxSource.PlayOneShot(clip);
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            ApplyVolumes();
        }

        public void SetSfxVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
            ApplyVolumes();
        }

        public void SetMuted(bool value)
        {
            muted = value;
            PlayerPrefs.SetInt(MuteKey, muted ? 1 : 0);
            ApplyVolumes();
        }

        private void ApplyVolumes()
        {
            float muteMultiplier = muted ? 0f : 1f;
            if (musicSource != null) musicSource.volume = musicVolume * muteMultiplier;
            if (sfxSource != null) sfxSource.volume = sfxVolume * muteMultiplier;
        }
    }
}
