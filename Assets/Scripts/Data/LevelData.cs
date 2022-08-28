using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SLG.Challenge
{

    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {

        [SerializeField] int xDimension;
        [SerializeField] int yDimension;
        [SerializeField] Vector2 exitCellPosition;
        [SerializeField] List<Vector2> wallCellsPosition = new List<Vector2>();
        [SerializeField] List<Character> characters = new List<Character>();
        [SerializeField] Vector3 cameraPosition = new Vector3();
        [SerializeField] float orthographicSize;
        [SerializeField] bool fillBorders = false;
        [SerializeField] string initialMessage;
        
        public int XDimension { get => xDimension; }
        public int YDimension { get => yDimension; }
        public List<Vector2> WallCellsPosition { get => wallCellsPosition; }
        public Vector2 ExitCellPosition { get => exitCellPosition; }
        public List<Character> Characters { get => characters; }
        public Vector3 CameraPosition { get => cameraPosition; }
        public float OrthographicSize { get => orthographicSize; }
        public bool FillBorders { get => fillBorders; }
        public string InitialMessage { get => initialMessage; }

        public static LevelData GetLevelData (Levels level)
        {
            LevelData loadedLevelData = null;
            //Sadly AssetDatabase wont work for build
            //string [] guidArray = AssetDatabase.FindAssets( $"{level}" );

            //foreach (string guid in guidArray)
            //{
            //    string path = AssetDatabase.GUIDToAssetPath( guid );
            //    Debug.Log(path);
            //    loadedLevelData = ( LevelData ) AssetDatabase.LoadAssetAtPath( path, typeof( LevelData ) );
            //}
            string path = $"Level{( int ) level + 1}";
            loadedLevelData = Resources.Load<LevelData>( path );
            return loadedLevelData;
        }

    }

    public enum Levels {
        Level1,
        Level2,
        Level3
    }

}
