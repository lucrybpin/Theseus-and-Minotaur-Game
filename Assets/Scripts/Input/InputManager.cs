using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLG.Challenge
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;

        public static InputManager Instance { get => instance; }

        [SerializeField] InputActionAsset inputActionAsset;
        InputAction movementInput;
        InputAction waitInput;
        InputAction rollBackInput;

        private void OnEnable ()
        {
            inputActionAsset.Enable();
        }

        private void OnDisable ()
        {
            inputActionAsset.Disable();
        }

        private void Awake ()
        {
            SingletonSetup();
            Setup();
        }

        private void Setup ()
        {
           InputActionMap gameplayInputActionMap = inputActionAsset.FindActionMap( "Gameplay" );
            movementInput = gameplayInputActionMap.FindAction( "Movement" );
            waitInput = gameplayInputActionMap.FindAction( "Wait" );
            rollBackInput = gameplayInputActionMap.FindAction( "Rollback" );
        }

        public bool MovementRight()
        {
            if (movementInput.ReadValue<Vector2>() == Vector2.right)
                return true;
            return false;
        }

        public bool MovementLeft ()
        {
            if (movementInput.ReadValue<Vector2>() == Vector2.left)
                return true;
            return false;
        }

        public bool MovementUp ()
        {
            if (movementInput.ReadValue<Vector2>() == Vector2.up)
                return true;
            return false;
        }

        public bool MovementDown ()
        {
            if (movementInput.ReadValue<Vector2>() == Vector2.down)
                return true;
            return false;
        }

        public bool WaitAction()
        {
            if (waitInput.triggered)
                return true;
            return false;
        }

        public bool RollbackAction()
        {
            if (rollBackInput.triggered)
                return true;
            return false;
        }

        private void SingletonSetup ()
        {
            if (InputManager.Instance == null)
                instance = this;

            if (InputManager.Instance != null)
                if (InputManager.Instance != this)
                    Destroy( this );
        }

    }
}
