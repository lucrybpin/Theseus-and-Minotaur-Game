using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG.Challenge
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer;

        [SerializeField][ReadOnly] bool isOccupied;

        public bool IsOccupied { get => isOccupied; set => isOccupied =  value ; }


        public void ChangeCellColor(Color newColor)
        {
            meshRenderer.material.color = newColor;
        }
    }
}
