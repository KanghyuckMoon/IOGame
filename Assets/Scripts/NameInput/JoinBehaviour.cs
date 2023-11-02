using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class JoinBehaviour : NetworkBehaviour
{
	[SerializeField] private Canvas joinCanvas;
	[SerializeField] private Canvas dieCanvas;
	[SerializeField] private TMP_InputField inputField;
	private Player playObject;
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
	public void Retry()
	{
		dieCanvas.gameObject.SetActive(false);
		AddPlayer();
		playObject.Retry();
	}

	[Client]
	public void Join()
	{
		CmdSendMessage(inputField.text);
		joinCanvas.gameObject.SetActive(false);
	}

	[Command]
	private void CmdSendMessage(string message)
	{
		GameObject player = Instantiate(NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Player"), Vector3.zero, Quaternion.identity);
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Add(player.GetComponent<Player>());
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary.Add(conn.connectionId, player.transform);
		NetworkServer.Spawn(player, gameObject);
		playObject = player.GetComponent<Player>();
		RpcHandleMessage(playObject);
	}

	[ClientRpc]
	private void RpcHandleMessage(Player player)
	{
		if (isOwned)
		{
			player.OnDie += ShowDieCanvas;
			playObject = player;
		}
	}

	public void ShowDieCanvas()
	{
		if (isOwned)
		{
			RemovePlayer();
			dieCanvas.gameObject.SetActive(true);
		}
	}

	[Command]
	private void RemovePlayer()
	{
		if(((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Contains(playObject))
		{
			((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Remove(playObject);
		}
	}
	[Command]
	private void RemovePlayerDic()
	{
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary.Remove(conn.connectionId);
	}

	[Command]
	private void AddPlayer()
	{
		if (!((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Contains(playObject))
		{
			((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Add(playObject);
		}
	}

	public override void OnStopClient()
	{
		RemovePlayer();
		RemovePlayerDic();
	}
}
