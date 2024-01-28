using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage = null;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float fireDamage = 0;

        [Header("Finale Damage")]
        private float finalDamageDealt = 0;

        [Header("Animation")]
        public bool playDamageAnimatio = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation = "";

        [Header("SFX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX = null;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom = 0;
        public Vector3 contactPoint = Vector3.zero;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if(character.isDead.Value)
            {
                return;
            }

            CalculateDamage(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            if(!character.IsOwner)
            {
                return;
            }

            if(characterCausingDamage != null)
            {

            }

            finalDamageDealt = physicalDamage + fireDamage;
            
            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        }
    }
}