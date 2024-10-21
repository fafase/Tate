using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools
{
    public class SceneLoading :  IDisposable
    {
        private string m_current;
        private CancellationToken m_cancellationToken;
        private CancellationTokenSource m_source;
        private bool m_disposed;

        public SceneLoading()
        {
            m_current = "Meta";
            m_source = new CancellationTokenSource();
            m_cancellationToken = m_source.Token;
        }

        //public virtual async UniTask LoadMeta(IProgress<float> progress = null) 
        //    => await Load("Meta", progress);
  
        //public virtual async UniTask LoadCore(IProgress<float> progress = null) 
        //    => await Load("Core", progress);


        public async UniTask LoadScene(string scene, IProgress<float> progress = null)
            => await Load(scene, progress);

        public async UniTask Load(string scene, IProgress<float> progress) 
        {
            if (string.IsNullOrEmpty(scene)) 
            {
                throw new ArgumentNullException();
            }
            var asyncOperation = SceneManager.LoadSceneAsync(scene);
            while (!asyncOperation.isDone) 
            {
                if (m_cancellationToken.IsCancellationRequested)
                {
                    Debug.LogWarning("Scene loading canceled.");
                    return; 
                }
                progress?.Report(asyncOperation.progress);
                await UniTask.Yield();
            }
            string temp = m_current;
            m_current = scene;
            Debug.Log($"Changing scene from {temp} to {scene}");
            Signal.Send(new SceneChangeSignal(scene, temp));
            progress?.Report(1.0f);
        }

        public void Dispose()
        {
            if (m_disposed) 
            {
                return;
            }
            m_disposed = true;
            m_source?.Cancel();
        }
    }


    public class SceneChangeSignal : SignalData 
    {
        public readonly string NextScene;
        public readonly string PreviousScene;

        public SceneChangeSignal(string nextScene, string previousScene)
        {
            NextScene = nextScene;
            PreviousScene = previousScene;
        }
    }
}
