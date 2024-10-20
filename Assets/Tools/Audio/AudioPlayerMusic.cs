using Cysharp.Threading.Tasks;
using Rx;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Tools.Audio
{

    public class AudioPlayerMusic : SimpleSingleton<AudioPlayerMusic>
    {
        [SerializeField] private float m_maxVolume = 0.8f;
        [SerializeField] private float m_volumeIncreaseSpeed = 1f;

        [Header("Main Audio")]
        [SerializeField] private List<AudioData> m_audioData;

        private AudioSource[] m_musicSources;
        private int m_currentMainAudioSource = 0;
        private CompositeDisposable m_compositeDisposable = new();
        private CancellationTokenSource m_source = new();

        private void Start()
        {
            DontDestroyOnLoad(this);
            SetMusicSources();

            //StartAmbianceMusic()
            //    .Subscribe(_ => { })
            //    .AddTo(m_compositeDisposable);
            StartAmbianceMusic().Forget();
            BindEndLevel();
            //BindSceneLoading();
        }

        private void OnDestroy()
        {
            m_compositeDisposable?.Dispose();
            m_source.Cancel();
        }

        private void SetMusicSources()
        {
            m_musicSources = new[]
            {
            gameObject.AddComponent<AudioSource>(),
            gameObject.AddComponent<AudioSource>()
        };
        }

        private async UniTask StartAmbianceMusic()
        {
            AudioSource audioSource = m_musicSources[m_currentMainAudioSource];
            audioSource.clip = m_audioData.Find(audio => audio.name.Equals("Idle")).audioClip;
            audioSource.Play();
            audioSource.volume = 0f;
            audioSource.loop = true;
            float elapsedTime = 0f;
            float fadeProgress = 0f;
            while (fadeProgress < 1f)
            {
                if (m_source.IsCancellationRequested) 
                {
                    return;
                }
                elapsedTime += Time.deltaTime;
                fadeProgress = Mathf.Clamp01(elapsedTime / m_volumeIncreaseSpeed);
                audioSource.volume = Mathf.Lerp(0f, m_maxVolume, fadeProgress);
                await UniTask.Yield();
            }
            audioSource.volume = 1f;
        }

        private void BindEndLevel()
        {
            //Signal.Connect<EndGameSignal>(Fade);
        }

        private async UniTask FadeMusic(string clip)
        {
            var audioClip = GetMusicClip(clip);
            if (audioClip == null)
            {
                Debug.LogError("Could not find clip " + clip);
                return;
            }
            await FadeMusic(audioClip);
        }

        private async UniTask FadeMusic(AudioClip nextClip)
        {
            AudioSource current = m_musicSources[m_currentMainAudioSource];
            AudioSource next = m_musicSources[(m_currentMainAudioSource == 0) ? 1 : 0];
            next.volume = 0f;
            next.loop = true;
            next.clip = nextClip;
            next.Play();
            float elapsedTime = 0f;
            float fadeProgress = 0f;
            while (fadeProgress < 1f)
            {
                if (m_source.IsCancellationRequested)
                {
                    return;
                }
                elapsedTime += Time.deltaTime;
                fadeProgress = Mathf.Clamp01(elapsedTime / m_volumeIncreaseSpeed);
                current.volume = Mathf.Lerp(m_maxVolume, 0f, fadeProgress);
                next.volume = Mathf.Lerp(0f, m_maxVolume, fadeProgress);
                await UniTask.Yield();
            }

            current.volume = 0f;
            current.Stop();
            current.volume = m_maxVolume;
            m_currentMainAudioSource = m_currentMainAudioSource == 0 ? 1 : 0;
        }

        private AudioClip GetMusicClip(string name) => m_audioData.FirstOrDefault(audio => audio.name.Equals(name)).audioClip;
    }
}