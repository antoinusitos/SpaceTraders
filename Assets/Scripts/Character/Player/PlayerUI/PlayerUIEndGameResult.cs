using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace AG
{
    public class PlayerUIEndGameResult : MonoBehaviour
    {
        [SerializeField]
        private UI_EndGameResult_Panel uI_EndGameResult_PanelPrefab = null;

        [SerializeField]
        private Transform[] endGamePanelResults = null;

        [SerializeField]
        private GameObject container = null;

        public void PullResults()
        {
            container.SetActive(true);

            for (int i = 0; i < endGamePanelResults.Length; i++)
            {
                if (endGamePanelResults[i].childCount > 0)
                {
                    Destroy(endGamePanelResults[i].GetChild(0).gameObject);
                }
            }

            PlayerManager[] players = FindObjectsOfType<PlayerManager>();

            for(int i = 0; i < players.Length; i++)
            {
                UI_EndGameResult_Panel instantiated_uI_EndGameResult_Panel = Instantiate(uI_EndGameResult_PanelPrefab, endGamePanelResults[i]);
                instantiated_uI_EndGameResult_Panel.characterName.text = players[i].playerNetworkManager.characterName.Value.ToString();
                instantiated_uI_EndGameResult_Panel.characterState.text = players[i].playerNetworkManager.currentHealth.Value <= 0 ? "Dead" : "Alive";
                instantiated_uI_EndGameResult_Panel.characterFaction.text = players[i].playerNetworkManager.faction.Value.ToString();
            }
        }

        public void CloseResults()
        {
            container.SetActive(false);
        }

        internal void OnEndGameChanged(bool previousValue, bool newValue)
        {
            if(newValue)
            {
                PullResults();
            }
        }
    }
}