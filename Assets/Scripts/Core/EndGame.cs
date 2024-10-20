using System.Collections.Generic;
using Rx;
using System;
using Tatedrez.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class EndGameService 
{
    private CancellationTokenSource m_source = new CancellationTokenSource();
    public void Dispose() => m_source.Cancel();
    public IObservable<Unit> EndLevelSequence(List<IPawn> pawns, Turn winningTurn) 
    {
        return Observable.Create<Unit>(observer => 
        {
            List<IPawn> winningPawn = pawns.FindAll(pawn => pawn.PawnTurn.Equals(winningTurn));
            List<IPawn> losingPawn = pawns.FindAll(pawn => !pawn.PawnTurn.Equals(winningTurn));
            RemoveSequence(losingPawn).Forget();
            BlobSequence(winningPawn).Forget();
            TimerSequence()
                .ContinueWith(() => 
                {

                    observer.OnNext(Unit.Default);
                    observer.OnCompleted();
                });
            return Disposable.Empty;
        });
    }

    private async UniTask RemoveSequence(List<IPawn> losingPawns) 
    {
        float period = 0.5f;
        float elapsedTime = 0f;
        Vector3 startScale = losingPawns[0].Transform.localScale;
        Vector3 target = Vector3.zero;

        while (elapsedTime < period)
        {
            elapsedTime += Time.deltaTime / period;
            Vector3 temp = Vector3.Lerp(startScale, target, elapsedTime);
            losingPawns.ForEach(pawn => pawn.Transform.localScale = temp);
            await UniTask.Yield();
        }
        losingPawns.ForEach((pawn) => pawn.Transform.localScale = Vector3.zero);
    }

    private async UniTask BlobSequence(List<IPawn> winningPawns)
    {
        float period = 0.5f;
        float amplitude = 0.1f;
        float elapsedTime = 0f;
        Vector3 startScale = winningPawns[0].Transform.localScale;

        while (true)
        {
            if (m_source.IsCancellationRequested)
            {
                return;
            }
            elapsedTime += Time.deltaTime;
            float sine = Mathf.Sin((elapsedTime / period) * Mathf.PI * 2);
            float scale = 1f + (sine * amplitude);
            winningPawns.ForEach((pawn) => pawn.Transform.localScale = startScale * scale);
            await UniTask.Yield();
        }
    }

    private async UniTask TimerSequence() => await UniTask.Delay(2000);
}
