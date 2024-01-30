using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class RandomItemSpawner : MonoBehaviour
    {
        private void Start()
        {
            if(NetworkManager.Singleton.IsServer)
            {
                GenerateItem();
            }
        }

        public void GenerateItem()
        {
            PickableItem pickableItem = WorldItemsManager.instance.GetRandomPickableItem();
            if(pickableItem)
            {
                PickableItem spawnedPickableItem = Instantiate(pickableItem);
                spawnedPickableItem.transform.position = transform.position;
                spawnedPickableItem.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}