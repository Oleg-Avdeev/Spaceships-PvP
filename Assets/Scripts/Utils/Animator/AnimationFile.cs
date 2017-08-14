using UnityEngine;
using System;

[Serializable]
public class AnimationFile : ScriptableObject
{
        [SerializeField] public Sprite[] Sprites;
        [SerializeField] public float[] TimeMultiplier;
}