using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG.Challenge
{
    [System.Serializable]
    public class CommandWait : Command {
        public CommandWait (Character character)
        {
            this.Name = "Wait";
            this.Character = character;
        }

        public override bool Execute ()
        {
            return true;
        }

        public override void Undo ()
        {
            
        }
    }
}
