using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 2;

        [Header("Character Name")]
        public string characterName = "";

        [Header("Character Name")]
        public int characterNumber = 1;

        [Header("Time Played")]
        public float secondsPlayed = 0;

        [Header("World Coordinates")]
        public float xPosition = 0;
        public float yPosition = 0;
        public float zPosition = 0;

        [Header("Stats")]
        public int vitality = 4;
        public int endurance = 10;
    }
}