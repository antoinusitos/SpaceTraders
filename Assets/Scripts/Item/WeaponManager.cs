using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider = null;

        protected virtual void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            if(!meleeDamageCollider)
            {
                return;
            }
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;

            meleeDamageCollider.lightAttackModifier = weapon.lightAttackModifier;
        }
    }
}