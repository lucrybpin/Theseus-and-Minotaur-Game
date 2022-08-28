
using UnityEngine;
using static SLG.Challenge.Character;

namespace SLG.Challenge
{
    [System.Serializable]
    public abstract class Command
    {

        [SerializeField] Character character;

        [SerializeField] private string name;

        public string Name { get => name; set => name =  value ; }
        public Character Character { get => character; set => character =  value ; }

        public abstract bool Execute ();

        public abstract void Undo ();

        public static Command GetCommandFromDirection(Direction direction, Character character)
        {
            Command returnCommand = new CommandWait( character );
            switch (direction)
            {
                case Direction.Left:
                returnCommand = new CommandLeft( character );
                break;

                case Direction.Right:
                returnCommand = new CommandRight( character );
                break;

                case Direction.Up:
                returnCommand = new CommandUp ( character );
                break;

                case Direction.Down:
                returnCommand = new CommandDown( character );
                break;

                case Direction.None:
                returnCommand = new CommandWait( character );
                break;

            }
            return returnCommand;
        }
    }
}
