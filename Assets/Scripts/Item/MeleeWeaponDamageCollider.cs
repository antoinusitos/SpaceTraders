using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage = null;
    }
}