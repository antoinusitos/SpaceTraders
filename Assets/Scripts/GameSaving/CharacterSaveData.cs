using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "";

        [Header("Time Played")]
        public float secondsPlayed = 0;

        [Header("World Coordinates")]
        public float xPosition = 0;
        public float yPosition = 0;
        public float zPosition = 0;
    }
}