using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Health Pack Action")]
    public class HealthPackAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
            {
                return;
            }

            PerformHealthPackAction(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHealthPackAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            RefillHealthCharacterEffect refillHealthEffect = Instantiate(WorldCharacterEffectsManager.instance.refillHealthEffect);
            HealConsummable consumable = playerPerformingAction.playerEquipmentManager.rightHandSlot.currentWeaponModel.GetComponent<HealConsummable>();
            refillHealthEffect.healthValue = consumable.healAmount;
            playerPerformingAction.characterEffectsManager.ProcessInstantEffect(refillHealthEffect);

            playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = -1;
            playerPerformingAction.playerNetworkManager.currentRightHandWeaponID.Value = -1;
            playerPerformingAction.playerEquipmentManager.LoadRightWeapon();
            playerPerformingAction.playerEquipmentManager.EmptyCurrentSlot();
            playerPerformingAction.playerInventoryManager.RemoveItemFromSlot(playerPerformingAction.playerEquipmentManager.GetCurrentSlotUsed());
        }
    }
}