using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID = -1;

        public virtual void ProcessEffect(CharacterManager character)
        {

        }
    }
}