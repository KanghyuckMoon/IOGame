using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawner : NetworkBehaviour
{
	[Header("Player")]
	[SerializeField]
	NetworkPlayer playerPrefab;


	public override void OnNetworkSpawn()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
		}
	}

	public void OnPlayerJoined(ulong id)
	{
		if (NetworkManager.Singleton.IsServer)
		{
			NetworkPlayer spawnedPlayer = Instantiate(playerPrefab, Utils.GetRandomSpawnPosition(), Quaternion.identity);
			spawnedPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(id);
			Debug.Log($"Spawn Player {id}");
		}
	}
}
