using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;

namespace SLG.Challenge
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        public static UIManager Instance { get => instance; }

        [SerializeField] UIPanelMessage panelMessage;
        [SerializeField] UIButtonWait buttonWait;

        private void Awake ()
        {
            SingletonSetup();
            if (buttonWait == null)
                buttonWait = FindObjectOfType<UIButtonWait>();
        }

        public void ShowWinMessage()
        {
            _ = ShowWinMessageAsync();
        }

        public void ShowLoseMessage ()
        {
            _ = ShowLoseMessageAsync();
        }

        public async UniTask ShowRestartMessage ()
        {
            await panelMessage.DisplayMessage( "Lets try this again", true );
        }

        public async UniTask ShowMinotaurStuckMessage ()
        {
            await panelMessage.DisplayMessage( "I think I've got it", true );
        }

        public async UniTask ShowWinMessageAsync ()
        {
            await panelMessage.DisplayMessage( "I Won!" ); //I18N?
        }

        public async UniTask ShowLoseMessageAsync ()
        {
            await panelMessage.DisplayMessage( "RAAAAWWWWWRR (I Lost)" ); //I18N?
        }

        public bool WaitActionTriggered()
        {
            return buttonWait.Pressed;
        }

        private void SingletonSetup ()
        {
            if (UIManager.Instance == null)
                instance = this;

            if (UIManager.Instance != null)
                if (UIManager.Instance != this)
                    Destroy( this );
        }

        public async UniTask ShowLevelMessage (string levelMessage)
        {
            await panelMessage.DisplayMessage( levelMessage, true ); //I18N?
        }
    }
}
