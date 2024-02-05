using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponItem : ItemDefinition
    {
        [Header("Weapon Base Damage")]
        public float physicalDamage = 0;
        public float fireDamage = 0;

        [Header("Stamina Cost")]
        public float baseStaminaCost = 20;

        [Header("Modifiers")]
        public float lightAttackModifier = 1;

        [Header("Actions")]
        public WeaponItemAction LMB_Action = null;
    }
}