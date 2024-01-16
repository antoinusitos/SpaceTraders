using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.TextCore.Text;

namespace AG
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager player = null;

        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Factions> faction = new NetworkVariable<Factions>(Factions.NONE, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            faction.OnValueChanged += OnFactionChanged;
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
    }
}