using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;

namespace SLG.Challenge
{
    [System.Serializable]
    public class Character
    {
        [SerializeField] string name;
        [SerializeField] Color color;
        [SerializeField] int numberOfTurns;
        [SerializeField] bool isPlayer = false;
        [SerializeField] Vector2 startPosition;
        [SerializeField] Vector2 currentPosition;
        [SerializeField] Stack<Command> commands = new Stack<Command>();
        [SerializeField] protected Dictionary<Vector2, Cell> grid;
        

        public Character (Dictionary<Vector2, Cell> grid, bool isPlayer)
        {
            this.grid = grid;
            this.isPlayer = isPlayer;
        }

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        public bool IsPlayer { get => isPlayer; }
        public Vector2 StartPosition { get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
                currentPosition = value;
            }
        }
        public Vector2 CurrentPosition { get => currentPosition; set => currentPosition =  value ; }
        public int NumberOfTurns { get => numberOfTurns; set => numberOfTurns =  value ; }
        public Stack<Command> Commands { get => commands; protected set => commands = value;  }

        public async UniTask ExecuteTurn ()
        {
            bool moved = false;
            while (moved == false)
            {
                if (LevelManager.Instance.GameIsOver == true)
                    return;

                Direction direction = await HandleInput();
                moved = await Move( direction );
            }
        }

        public virtual async UniTask<Direction> HandleInput() {
            await UniTask.Delay( TimeSpan.FromSeconds( 1f ) );
            return Direction.None;
        }

        public virtual async UniTask<bool> Move(Direction direction)
        {
            var successfullyMoved = false;
            Vector2 nextCellPosition = new Vector2();
            Cell currentCell;
            Cell nextCell;

            await UniTask.Yield();

            if (grid.TryGetValue( this.currentPosition, out currentCell ))
            {
                switch (direction)
                {
                    case Direction.Left:
                    nextCellPosition = this.currentPosition + Vector2.left;
                    break;
                    case Direction.Right:
                    nextCellPosition = this.currentPosition + Vector2.right;
                    break;
                    case Direction.Up:
                    nextCellPosition = this.currentPosition + Vector2.up;
                    break;
                    case Direction.Down:
                    nextCellPosition = this.currentPosition + Vector2.down;
                    break;
                    case Direction.None:
                    successfullyMoved = true;
                    commands.Push( new CommandWait( this ) );
                    return successfullyMoved;

                    case Direction.Rollback:
                    if (commands.Count > 0)
                        commands.Pop().Undo();
                    break;
                }

                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                {
                    if (nextCell.IsOccupied == true)
                    {
                        successfullyMoved = false;
                    } else
                    {
                        Command command = Command.GetCommandFromDirection( direction, this );
                        successfullyMoved = command.Execute();
                        if (successfullyMoved)
                            commands.Push( command );
                        //currentCell.IsOccupied = false;
                        //currentCell.ChangeCellColor( Color.white );
                        //nextCell.IsOccupied = true;
                        //nextCell.ChangeCellColor( this.color );
                        //this.currentPosition = nextCellPosition;
                        //successfullyMoved = true;
                    }
                }

            } else
            {
                return false;
            }

            return successfullyMoved;

        }

        public enum Direction {
            Left,
            Right,
            Up,
            Down,
            None,
            Rollback
        }


    }
}
