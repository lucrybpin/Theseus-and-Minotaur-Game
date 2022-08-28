using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;

namespace SLG.Challenge
{
    public class CameraController : MonoBehaviour
    {
        private static CameraController instance;

        public static CameraController Instance { get => instance; }

        [SerializeField] Camera camera;
        [SerializeField] Vector3 basePosition = new Vector3();

        private void Awake ()
        {
            SingletonSetup();
            camera = Camera.main;
        }

        public void SetPositionInstant (Vector3 newPosition)
        {
            camera.transform.position = newPosition;
        }

        public void SetOrthographicSizeInstant(float newOrthographicSize)
        {
            camera.orthographicSize = newOrthographicSize;
        }

        public async UniTask SetPosition (Vector3 desiredPosition, float time = 1f)
        {
            float elapsedTime = 0;
            float totalTime = time;
            Vector3 initialPosition = camera.transform.position;

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                camera.transform.position = Vector3.Lerp( initialPosition, desiredPosition, elapsedTime / totalTime );
                await UniTask.Yield();
            }
        }

        public async UniTask SetOrthographicSize (float desiredSize, float time = .52f)
        {
            float elapsedTime = 0;
            float totalTime = time;
            float initialSize = camera.orthographicSize;

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                camera.orthographicSize = Mathf.Lerp( initialSize, desiredSize, elapsedTime / totalTime );
                await UniTask.Yield();
            }
        }

        private void SingletonSetup ()
        {
            if (CameraController.Instance == null)
                instance = this;

            if (CameraController.Instance != null)
                if (CameraController.Instance != this)
                    Destroy( this );
        }
    }
}
