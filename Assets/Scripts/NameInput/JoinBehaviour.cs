using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class JoinBehaviour : NetworkBehaviour
{
	[SerializeField] private Canvas joinCanvas;
	[SerializeField] private TMP_InputField inputField;
	private NetworkConnectionToClient conn;

	private void Start()
	{
		if (isLocalPlayer)
		{
			joinCanvas.gameObject.SetActive(true);
		}
	}

	public void SetClient(NetworkConnectionToClient conn)
	{
		this.conn = conn;
	}

	[Client]
	public void Join()
	{
		CmdSendMessage(inputField.text);
		gameObject.SetActive(false);
	}

	[Command]
	private void CmdSendMessage(string message)
	{
		GameObject player = Instantiate(NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Player"), Vector3.zero, Quaternion.identity);
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Add(player.GetComponent<Player>());
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary.Add(conn.connectionId, player.transform);
		NetworkServer.Spawn(player, gameObject);
		RpcHandleMessage(message);
	}

	[ClientRpc]
	private void RpcHandleMessage(string message)
	{

	}
}
