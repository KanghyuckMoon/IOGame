using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
	public static NetworkPlayer Local {get; set;}

	public override void OnNetworkSpawn()
	{
		Debug.Log($"Player Spawned, auth {IsOwner}");

		if (IsOwner)
		{
			Local = this;
		}

		transform.name = $"P_{OwnerClientId}";
	}

	public void PlayerLeft()
	{

	}
}
