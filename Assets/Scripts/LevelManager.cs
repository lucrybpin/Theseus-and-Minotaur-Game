using NaughtyAttributes;
using System;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace SLG.Challenge {
    public class LevelManager : MonoBehaviour {

        private static LevelManager instance;

        public static LevelManager Instance { get => instance; }
        public bool GameIsOver { get => gameIsOver; }
        public LevelData CurrentLevelData { get => currentLevelData; }
        public Dictionary<Vector2, Cell> Grid { get => grid; }

        Dictionary<Vector2, Cell> grid = new Dictionary<Vector2, Cell>();

        [SerializeField] List<Character> characters;

        [SerializeField] GameObject cellPrefab;

        [SerializeField] LevelData currentLevelData;

        [SerializeField] [ReadOnly] bool gameIsOver = false;

        public UnityEvent OnWin;
        public UnityEvent OnLose;

        [SerializeField] private int levelIndex = 0;

        void Awake ()
        {
            SingletonSetup();
        }

        void Start ()
        {
            LoadLevelData();
            GenerateGrid( currentLevelData );
            HandleGame();
            _ = UIManager.Instance.ShowLevelMessage( currentLevelData.InitialMessage );
        }

        private async void HandleGame ()
        {
            gameIsOver = false;
            await HandleTurns();
        }

        private async UniTask HandleTurns ()
        {
            while (!gameIsOver)
            {
                foreach (Character character in characters)
                {
                    for (int i = 0; i < character.NumberOfTurns; i++)
                        await character.ExecuteTurn();
                }
                CheckEndOfGame();
                await UniTask.Yield();
            }
        }

        public void RestartGame ()
        {
            ClearGrid( currentLevelData );
            LoadLevelData();
            GenerateGrid( currentLevelData );
            HandleGame();
            _ = UIManager.Instance.ShowRestartMessage();
        }

        public void LoadNextLevel ()
        {
            ClearGrid( currentLevelData );
            ++levelIndex;
            LoadLevelData();
            GenerateGrid( currentLevelData );
            HandleGame();
            _ = UIManager.Instance.ShowLevelMessage(currentLevelData.InitialMessage);
            _ = CameraController.Instance.SetPosition( currentLevelData.CameraPosition );
            _ = CameraController.Instance.SetOrthographicSize( currentLevelData.OrthographicSize );
        }

        public void LoadPreviousLevel ()
        {
            ClearGrid( currentLevelData );
            --levelIndex;
            LoadLevelData();
            GenerateGrid( currentLevelData );
            HandleGame();
            _ = UIManager.Instance.ShowLevelMessage( currentLevelData.InitialMessage );
            _ = CameraController.Instance.SetPosition( currentLevelData.CameraPosition );
            _ = CameraController.Instance.SetOrthographicSize( currentLevelData.OrthographicSize );
        }
        

        private void CheckEndOfGame ()
        {
            foreach (Character character in characters)
            {
                CheckWinCondition( character );
                CheckLoseCondition( character );
            }
        }

        private void CheckWinCondition (Character character)
        {
            if (character is CharacterTheseus)
            {
                CharacterTheseus theseus = ( CharacterTheseus ) character;
                if (theseus.CurrentPosition == currentLevelData.ExitCellPosition)
                {
                    gameIsOver = true;
                    OnWin?.Invoke();
                }
            }
        }

        private void CheckLoseCondition (Character character)
        {
            if (character is CharacterMinotaur)
            {
                CharacterMinotaur minotaur = ( CharacterMinotaur ) character;
                if (minotaur.CurrentPosition == minotaur.Target.CurrentPosition)
                {
                    gameIsOver = true;
                    OnLose?.Invoke();
                }
            }
        }

        private void LoadLevelData ()
        {
            if (levelIndex > ( int ) Levels.Level3)
            {
                this.levelIndex = ( int ) Levels.Level3;
                return;
            }


            if (levelIndex < 0)
            {
                this.levelIndex = 0;
                return;
            }

            if (currentLevelData == null)
                currentLevelData = LevelData.GetLevelData( Levels.Level1 );
            else
                currentLevelData = LevelData.GetLevelData( ( Levels ) levelIndex );
        }

        private void GenerateGrid (LevelData levelData)
        {
            Dictionary<Vector2, Cell> generatedGrid;
            generatedGrid = GenerateClearGrid( levelData );
            generatedGrid = FillGridWalls( levelData, generatedGrid );
            generatedGrid = FillExitCell( levelData, generatedGrid );
            generatedGrid = FillWithCharacters( levelData, generatedGrid );

            this.grid = generatedGrid;
            this.characters = CreateCharacters( levelData );
        }

        private List<Character> CreateCharacters (LevelData levelData)
        {
            List<Character> characters = levelData.Characters;
            List<Character> charactersList = new List<Character>();
            CharacterTheseus characterTheseus = new CharacterTheseus( this.grid, levelData.ExitCellPosition );
            CharacterMinotaur characterMinotaur = new CharacterMinotaur( this.grid );

            foreach (Character character in characters)
            {
                if (character.IsPlayer)
                {
                    characterTheseus.Name = character.Name;
                    characterTheseus.Color = character.Color;
                    characterTheseus.StartPosition = character.StartPosition;
                    characterTheseus.NumberOfTurns = character.NumberOfTurns;
                    charactersList.Add( characterTheseus );
                }
                else
                {
                    characterMinotaur.Name = character.Name;
                    characterMinotaur.Color = character.Color;
                    characterMinotaur.StartPosition = character.StartPosition;
                    characterMinotaur.Target = characterTheseus;
                    characterMinotaur.NumberOfTurns = character.NumberOfTurns;
                    charactersList.Add( characterMinotaur );
                }
            }

            return charactersList;
        }

        private Dictionary<Vector2, Cell> GenerateClearGrid (LevelData levelData)
        {
            Dictionary<Vector2, Cell> generatedGrid = new Dictionary<Vector2, Cell>();
            for (int y = 0; y < levelData.YDimension; y++)
            {
                for (int x = 0; x < levelData.XDimension; x++)
                {
                    Cell newCell = Instantiate( cellPrefab, new Vector3( x, 0, y ), Quaternion.identity ).GetComponent<Cell>();
                    newCell.name = $"[{x}][{y}]";
                    generatedGrid.Add( new Vector2( x, y ), newCell );
                }
            }
            return generatedGrid;
        }

        private void ClearGrid (LevelData levelData)
        {
            if (grid == null)
                return;

            for (int y = 0; y < levelData.YDimension; y++)
                for (int x = 0; x < levelData.XDimension; x++)

                    if (grid.TryGetValue( new Vector2( x, y ), out Cell cellFound ))
                        Destroy( cellFound.gameObject );

            grid.Clear();
        }

        private Dictionary<Vector2, Cell> FillGridWalls (LevelData levelData, Dictionary<Vector2, Cell> generatedGrid)
        {
            
            if (levelData.FillBorders)
            {
                for (int x = 0; x < levelData.XDimension; x++)
                {
                    if (generatedGrid.TryGetValue( new Vector2( x, 0 ), out Cell cellFound ))
                    {
                        cellFound.IsOccupied = true;
                        cellFound.ChangeCellColor( Color.black );
                    }

                    if (generatedGrid.TryGetValue( new Vector2( x, levelData.YDimension - 1 ), out Cell otherCellFound ))
                    {
                        otherCellFound.IsOccupied = true;
                        otherCellFound.ChangeCellColor( Color.black );
                    }
                }

                for (int y = 1; y < levelData.YDimension-1; y++)
                {
                    if (generatedGrid.TryGetValue( new Vector2( 0, y ), out Cell cellFound ))
                    {
                        cellFound.IsOccupied = true;
                        cellFound.ChangeCellColor( Color.black );
                    }

                    if (generatedGrid.TryGetValue( new Vector2( levelData.XDimension-1, y ), out Cell otherCellFound ))
                    {
                        otherCellFound.IsOccupied = true;
                        otherCellFound.ChangeCellColor( Color.black );
                    }
                }
            }

            foreach (Vector2 wallCellPosition in levelData.WallCellsPosition)
            {
                Cell wallCell;
                if (generatedGrid.TryGetValue( wallCellPosition, out wallCell ))
                {
                    wallCell.IsOccupied = true;
                    wallCell.ChangeCellColor( Color.black );
                }
            }
            return generatedGrid;
        }

        private Dictionary<Vector2, Cell> FillExitCell (LevelData levelData, Dictionary<Vector2, Cell> generatedGrid)
        {
            Cell exitCell;
            if (generatedGrid.TryGetValue( levelData.ExitCellPosition, out exitCell ))
            {
                exitCell.IsOccupied = false;
                exitCell.ChangeCellColor( Color.green );
            }
            return generatedGrid;
        }

        private Dictionary<Vector2, Cell> FillWithCharacters (LevelData levelData, Dictionary<Vector2, Cell> generatedGrid)
        {
            Cell characterCell;
            foreach (Character character in levelData.Characters)
            {
                if (generatedGrid.TryGetValue( character.StartPosition, out characterCell ))
                {
                    characterCell.IsOccupied = true;
                    characterCell.ChangeCellColor( character.Color );
                }
            }
            return generatedGrid;
        }

        private void SingletonSetup ()
        {
            if (LevelManager.Instance == null)
                instance = this;

            if (LevelManager.Instance != null)
                if (LevelManager.Instance != this)
                    Destroy( this );
        }

        [Button("Display Character Commands")]
        public void DisplayCharacterCommands ()
        {
            foreach (Character character in characters)
            {
                foreach (Command command in character.Commands)
                {
                    Debug.Log( character.Name + ":" + command.Name );
                }
            }
        }

        
        public void UndoInstant()
        {
            List<Character> charactersReversed = new List<Character>( characters );
            charactersReversed.Reverse();
            foreach (Character character in charactersReversed)
            {
                for (int i = 0; i < character.NumberOfTurns; i++)
                {
                    if (character.Commands.Count > 0)
                        character.Commands.Pop().Undo();
                }
                //foreach (Command command in character.Commands)
                //{
                //    Debug.Log( character.Name + ":" + command.Name );
                //}
            }
        }

        [Button( "Undo" )]
        public void Undo ()
        {
            _ = UndoAsync();
        }

        public async UniTask UndoAsync ()
        {
            List<Character> charactersReversed = new List<Character>( characters );
            charactersReversed.Reverse();
            foreach (Character character in charactersReversed)
            {
                for (int i = 0; i < character.NumberOfTurns; i++)
                {
                    if (character.Commands.Count > 0)
                    {
                        character.Commands.Pop().Undo();
                        await UniTask.Delay( TimeSpan.FromSeconds( .21f ) );
                    }
                        
                }
                //foreach (Command command in character.Commands)
                //{
                //    Debug.Log( character.Name + ":" + command.Name );
                //}
            }
        }

    }
}
