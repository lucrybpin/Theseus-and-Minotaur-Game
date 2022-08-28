//using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SLG.Challenge
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElement : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Start ()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public async UniTask Show (float desiredAlpha = 1, float time = 1f, UnityAction OnFinish = null)
        {
            await AsyncHideShow( desiredAlpha, time, OnFinish );
        }

        public async UniTask Hide (float desiredAlpha = 0, float time = 1f, UnityAction OnFinish = null)
        {
            await AsyncHideShow( desiredAlpha, time, OnFinish );
        }

        public async UniTask AsyncHideShow (float desiredAlpha = 1, float time = 1f, UnityAction OnFinish = null)
        {
            float totalTime = time;
            float elapsedTime = 0;

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            float initialAlpha = canvasGroup.alpha;

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp( initialAlpha, desiredAlpha, elapsedTime / totalTime );
                await UniTask.Yield();
            }
            canvasGroup.alpha = desiredAlpha;
            OnFinish?.Invoke();
        }
    }
}
