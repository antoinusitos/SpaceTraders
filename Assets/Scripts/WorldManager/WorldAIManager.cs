using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance = null;

        [Header("DEBUG")]
        [SerializeField]
        private bool despawnCharacters = false;
        [SerializeField]
        private bool respawnCharacters = false;

        [Header("Characters")]
        [SerializeField]
        GameObject[] aiCharacters = null;
        [SerializeField]
        List<GameObject> spawnedInCharacters = new List<GameObject>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if(NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
            }
        }

        private void Update()
        {
            if(respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }
            if(despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while(!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }

            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach(GameObject character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedInCharacters.Add(instantiatedCharacter);
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (GameObject character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
            spawnedInCharacters.Clear();
        }

        private void DisableAllCharacters()
        {

        }
    }
}