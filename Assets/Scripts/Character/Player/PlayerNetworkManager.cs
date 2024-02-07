using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace AG
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager player = null;

        [Header("Player Info")]
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Factions> faction = new NetworkVariable<Factions>(Factions.NONE, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<int> playerCharacterNumber = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> endGameReached = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Runtime Values")]
        public NetworkVariable<int> cash = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Utilities")]
        public NetworkVariable<float> cameraUpDownAngle = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            faction.OnValueChanged += OnFactionChanged;
        }
        
        public void SetCharacterActionHand()
        {
            isUsingRightHand.Value = true;
        }

        public void OnPlayerCharacterChanged(int previousIndex, int newIndex)
        {
            if(!IsOwner)
            {
                player.RefreshPlayerCharacterVisual(newIndex);
            }
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void OnFactionChanged(Factions oldFaction, Factions newFaction)
        {
            if(!IsOwner)
            {
                return;
            }

            if(newFaction == Factions.NONE)
            {
                WorldGameManager.instance.networkGameFinished.OnValueChanged -= PlayerUIManager.instance.playerUIEndGameResult.OnEndGameChanged;
            }
            else
            {
                WorldGameManager.instance.networkGameFinished.OnValueChanged += PlayerUIManager.instance.playerUIEndGameResult.OnEndGameChanged;
            }

            PlayerUIManager.instance.playerUIHUDManager.ShowFaction(newFaction);
        }

        [ClientRpc]
        public void HideWaitingPlayerClientRpc()
        {
            if (IsOwner)
            {
                PlayerUIManager.instance.playerUIHUDManager.ShowWaitingPlayers(false);
            }
        }

        [ClientRpc]
        public void SyncQuestClientRpc(int questID)
        {
            if (IsOwner)
            {
                player.AddQuest(questID);
            }
        }

        [ClientRpc]
        public void SetRandomSeedClientRpc(int seed)
        {
            if (IsOwner)
            {
                Random.InitState(seed);
                PlayerUIManager.instance.playerUIHUDManager.ShowSeed(seed);
            }
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            if(newID == -1)
            {
                player.playerInventoryManager.currentRightHandWeapon = null;
                return;
            }

            WeaponItem newWeapon = (WeaponItem)Instantiate(WorldItemsManager.instance.GetItemWithID(newID));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadRightWeapon();
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            if (newID == -1)
            {
                return;
            }

            WeaponItem newWeapon = (WeaponItem)Instantiate(WorldItemsManager.instance.GetItemWithID(newID));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
        }

        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if(IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            if(clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemACtionWithID(actionID);

            if(weaponAction)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemsManager.instance.GetWeaponWithID(weaponID));
            }
            else
            {
                Debug.LogError("ACTION IS NULL, CANNOT BE PERFORMED");
            }
        }
    }
}