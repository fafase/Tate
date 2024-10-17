using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadNextScenePopup : Popup
{
    [SerializeField] private Button m_playButton;
    [SerializeField] private Scene m_scene;

    [Inject] private ISceneLoading m_sceneLoading;

    private void Start()
    {
        m_playButton.onClick.AddListener(() => Load());
    }

    private void Load()
    {
        OnClose += () => m_sceneLoading.LoadScene(m_scene.ToString());
        Close();
    }

    enum Scene { Meta, Core }
}
