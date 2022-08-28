using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG.Challenge
{
    public class CommandRight : Command
    {
        public CommandRight (Character character)
        {
            this.Name = "Right";
            this.Character = character;
        }

        public override bool Execute ()
        {
            if (LevelManager.Instance.Grid.TryGetValue( this.Character.CurrentPosition, out Cell currentCell ))
            {
                Vector2 nextCellPosition = this.Character.CurrentPosition + Vector2.right;
                if (LevelManager.Instance.Grid.TryGetValue( nextCellPosition, out Cell nextCell ))
                {
                    currentCell.IsOccupied = false;
                    if (this.Character.CurrentPosition == LevelManager.Instance.CurrentLevelData.ExitCellPosition)
                        currentCell.ChangeCellColor( Color.green );
                    else
                        currentCell.ChangeCellColor( Color.white );
                    nextCell.IsOccupied = true;
                    nextCell.ChangeCellColor( this.Character.Color );
                    this.Character.CurrentPosition = nextCellPosition;
                    return true;
                }
            }
            return false;
        }

        public override void Undo ()
        {
            if (LevelManager.Instance.Grid.TryGetValue( this.Character.CurrentPosition, out Cell currentCell ))
            {
                Vector2 nextCellPosition = this.Character.CurrentPosition + Vector2.left;
                if (LevelManager.Instance.Grid.TryGetValue( nextCellPosition, out Cell nextCell ))
                {
                    currentCell.IsOccupied = false;
                    if (this.Character.CurrentPosition == LevelManager.Instance.CurrentLevelData.ExitCellPosition)
                        currentCell.ChangeCellColor( Color.green );
                    else
                        currentCell.ChangeCellColor( Color.white );
                    nextCell.IsOccupied = true;
                    nextCell.ChangeCellColor( this.Character.Color );
                    this.Character.CurrentPosition = nextCellPosition;
                }
            }
        }
    }
}
