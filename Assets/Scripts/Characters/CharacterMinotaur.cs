using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace SLG.Challenge {
    public class CharacterMinotaur : Character {

        Character target;

        public Character Target { get => target; set => target = value; }

        public CharacterMinotaur (Dictionary<Vector2, Cell> grid) : base( grid, false )
        {

        }


        public override async UniTask<bool> Move (Direction direction)
        {
            var successfullyMoved = true;
            Vector2 nextCellPosition = new Vector2();
            Cell currentCell;
            Cell nextCell;

            if (LevelManager.Instance.GameIsOver == true)
                return successfullyMoved;

            await UniTask.Yield();
            if (grid.TryGetValue( this.CurrentPosition, out currentCell ))
            {
                switch (direction)
                {
                    case Direction.Left:
                    nextCellPosition = this.CurrentPosition + Vector2.left;
                    break;
                    case Direction.Right:
                    nextCellPosition = this.CurrentPosition + Vector2.right;
                    break;
                    case Direction.Up:
                    nextCellPosition = this.CurrentPosition + Vector2.up;
                    break;
                    case Direction.Down:
                    nextCellPosition = this.CurrentPosition + Vector2.down;
                    break;
                }

                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                {
                    if (nextCell.IsOccupied == false || nextCellPosition == target.CurrentPosition)
                    {
                        Command command = Command.GetCommandFromDirection( direction, this );
                        successfullyMoved = command.Execute();
                        if (successfullyMoved)
                            this.Commands.Push( command );
                        await UniTask.Delay( TimeSpan.FromSeconds( .07f ) );
                    }
                    else
                    {
                        this.Commands.Push( Command.GetCommandFromDirection( Direction.None, this ) );
                        await UniTask.Delay( TimeSpan.FromSeconds( .07f ) );
                        //_ = UIManager.Instance.ShowMinotaurStuckMessage();
                    }
                } 

            }
            else
            {
                Debug.LogError( "Could not find character current cell" );
            }
            return successfullyMoved;
        }

        public override async UniTask<Direction> HandleInput ()
        {
            await UniTask.Yield();
            Cell nextCell;

            Vector2 nextCellPosition = new Vector2();

            if (this.CurrentPosition.x < target.CurrentPosition.x)
            {
                nextCellPosition = this.CurrentPosition + Vector2.right;
                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                    if (nextCell.IsOccupied == false || nextCellPosition == target.CurrentPosition )
                        return Direction.Right;

            }
            if (this.CurrentPosition.x > target.CurrentPosition.x)
            {
                nextCellPosition = this.CurrentPosition + Vector2.left;
                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                    if (nextCell.IsOccupied == false || nextCellPosition == target.CurrentPosition)
                        return Direction.Left;
            }
            if (this.CurrentPosition.y > target.CurrentPosition.y)
            {
                nextCellPosition = this.CurrentPosition + Vector2.down;
                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                    if (nextCell.IsOccupied == false || nextCellPosition == target.CurrentPosition)
                        return Direction.Down;
            }
            if (this.CurrentPosition.y < target.CurrentPosition.y)
            {
                nextCellPosition = this.CurrentPosition + Vector2.up;
                if (grid.TryGetValue( nextCellPosition, out nextCell ))
                    if (nextCell.IsOccupied == false || nextCellPosition == target.CurrentPosition)
                        return Direction.Up;
            }
            return Direction.None;
        }
    }
}
