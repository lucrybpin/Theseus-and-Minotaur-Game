using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG.Challenge
{
    [System.Serializable]
    public class CommandUp : Command {

        public CommandUp (Character character)
        {
            this.Name = "Up";
            this.Character = character;
        }

        public override bool Execute ()
        {
            if (LevelManager.Instance.Grid.TryGetValue( this.Character.CurrentPosition, out Cell currentCell ))
            {
                Vector2 nextCellPosition = this.Character.CurrentPosition + Vector2.up;
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
                Vector2 nextCellPosition = this.Character.CurrentPosition + Vector2.down;
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
