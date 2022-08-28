using Cysharp.Threading.Tasks;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;

namespace SLG.Challenge {
    public class CharacterTheseus : Character {

        //[SerializeField] Vector2 targetPosition;

        public CharacterTheseus (Dictionary<Vector2, Cell> grid, Vector2 exitPosition) : base( grid, true )
        {
            //this.targetPosition = exitPosition;
        }


        public override async UniTask<Direction> HandleInput ()
        {
            UIManager uiManagerInstance = UIManager.Instance;
            while (true)
            {
                if (InputManager.Instance.MovementRight())
                    return Direction.Right;
                else if (InputManager.Instance.MovementLeft())
                    return Direction.Left;
                else if (InputManager.Instance.MovementUp())
                    return Direction.Up;
                else if (InputManager.Instance.MovementDown())
                    return Direction.Down;
                else if (InputManager.Instance.WaitAction() || uiManagerInstance.WaitActionTriggered())
                    return Direction.None;
                else if (InputManager.Instance.RollbackAction())
                    return Direction.Rollback;
                await UniTask.Yield();
            }
        }
    }
}
