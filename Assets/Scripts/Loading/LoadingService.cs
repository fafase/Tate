using UnityEngine;
using Tools;
using Cysharp.Threading.Tasks;
using System;

namespace Tate.Loading
{
    public class LoadingService : MonoBehaviour, ISceneLoading
    {
        private Animation m_animation;
        private SceneLoading m_sceneLoader;

        private bool m_waitForAnimation;
        private float m_currentFrame;
        private const string Core = "Core";
        private const string Meta = "Meta";
        private const string AnimName = "LoadingScreen";

        private void Start()
        {
            GetComponentInChildren<LoadingAnimation>(true).OnMiddleAnimationReach += LoadingService_OnMiddleAnimationReach;
            m_sceneLoader = new SceneLoading();
            m_animation = GetComponentInChildren<Animation>(true);
            m_animation.gameObject.SetActive(true);
            SetAndPlay(0.8f);
            Signal.Connect<SceneChangeSignal>(PlayLoading);
        }

        private void OnDestroy() 
        {
            var anim = GetComponentInChildren<LoadingAnimation>(true);
            if(anim != null)
            {
                anim.OnMiddleAnimationReach -= LoadingService_OnMiddleAnimationReach;
            }  
            m_sceneLoader?.Dispose();
        }

        private void LoadingService_OnMiddleAnimationReach()
        {
            m_waitForAnimation = false;
            m_currentFrame = m_animation[AnimName].time;
        }

        private void PlayLoading(SceneChangeSignal data)
        {
            SetAndPlay(0.0f);
        }

        private void SetAndPlay(float norm)
        {
            m_animation[AnimName].time = norm;
            m_animation.Play();
        }

        public async UniTask LoadCore(IProgress<float> progress = null)
        {
            await LoadScene(Core);
        }

        public async UniTask LoadMeta(IProgress<float> progress = null)
        {
            await LoadScene(Meta);
        }

        public async UniTask LoadScene(string scene, IProgress<float> progress = null)
        {
            SetAndPlay(0.0f);
            m_waitForAnimation = true;
            while (m_waitForAnimation)
            {
                await UniTask.Yield();
            }
            m_animation.Stop();
            await m_sceneLoader.LoadScene(scene);
            m_animation[AnimName].time = m_currentFrame;
            m_animation.Play();
            m_waitForAnimation = true;
        }
    }

    public interface ISceneLoading
    {
        UniTask LoadCore(IProgress<float> progress = null);
        UniTask LoadMeta(IProgress<float> progress = null);
        UniTask LoadScene(string scene, IProgress<float> progress = null);
    }
}
