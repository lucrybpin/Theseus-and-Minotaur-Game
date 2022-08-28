using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLG.Challenge
{
    public class UIButtonWait : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] bool pressed;

        public bool Pressed { get => pressed; }

        private float elapsedPressedTime = 0;

        public void OnPointerDown (PointerEventData eventData)
        {
            pressed = true;
        }

        public void OnPointerUp (PointerEventData eventData)
        {
            pressed = false;
        }

        private void Update ()
        {
            ReleaseButton();
        }

        private void ReleaseButton ()
        {
            elapsedPressedTime += Time.deltaTime;
            if (elapsedPressedTime > 0.1f)
            {
                pressed = false;
                elapsedPressedTime = 0;
            }
        }
    }
}
