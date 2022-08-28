using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
//using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SLG.Challenge
{
    public class UIPanelMessage : UIElement {

        [SerializeField] TMP_Text textField;

        [Button("TextMessage")]

        public void TestMessate()
        {
            _ = DisplayMessage( "This is a test message" );
        }

        public void DisplayMessageInstant (string messageToShow)
        {
            textField.text = messageToShow;
        }

        public async UniTask DisplayMessage (string messageToShow, bool hideAfter = false)
        {
            if (textField == null)
                textField = GetComponentInChildren<TMP_Text>();

            ClearMessage();

            await this.Show(1, .52f);

            foreach (char stringCharacter in messageToShow)
            {
                textField.text += stringCharacter;
                await UniTask.Delay( TimeSpan.FromSeconds( .025f ) );
            }

            if (hideAfter)
                await this.Hide();
        }

        public void ClearMessage ()
        {
            textField.text = "";
        }
    }
}
