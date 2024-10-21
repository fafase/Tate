using Tools;
using Rx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    private ButtonSfx m_audioClip;

    private CompositeDisposable m_compositeDisposable = new();
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.OnClickAsObservable()
            .Subscribe(_ => Signal.Send(new AudioSignal(m_audioClip.ToString())))
            .AddTo(m_compositeDisposable);
    }
    private void OnDestroy()
    {
        m_compositeDisposable?.Dispose();
    }
    enum ButtonSfx { Button_Click }
}
