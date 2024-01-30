using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponManager : MonoBehaviour
    {
        private MeleeWeaponDamageCollider meleeDamageCollider = null;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
        }
    }
}