using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Flare Action")]
    public class FlareWeaponItemAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
            {
                return;
            }

            PerformFlareAction(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformFlareAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                FlareWeaponManager flareWeaponManager = playerPerformingAction.playerEquipmentManager.rightHandSlot.currentWeaponModel.GetComponent<FlareWeaponManager>();
                if(!flareWeaponManager.isActive)
                {
                    flareWeaponManager.Activate();
                    playerPerformingAction.playerNetworkManager.NotifyTheServerOfFlareActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponPerformingAction.LMB_Action.actionID, weaponPerformingAction.itemID);
                }
                else
                {
                    flareWeaponManager.Throw(playerPerformingAction.playerFPSCamera.playerCamera.transform.forward);
                    playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = -1;
                }
            }
        }
    }
}