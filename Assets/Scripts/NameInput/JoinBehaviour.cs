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
	private NetworkConnectionToClient conn;
	private static JoinBehaviour joinBehaviour;
	
	private void Start()
	{
		if (isLocalPlayer)
		{
			joinCanvas.gameObject.SetActive(true);
			joinBehaviour = this;
		}
	}

	public void SetClient(NetworkConnectionToClient conn)
	{
		this.conn = conn;
	}

	[Command]
	public void Retry()
	{
		AddPlayer();
		Player player = ((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary[conn.connectionId].GetComponent<Player>();
		player.Retry();
		dieCanvas.gameObject.SetActive(false);
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
		player.GetComponent<Player>().OnDie += ShowDieCanvas;
		RpcHandleMessage(message);
	}

	[ClientRpc]
	private void RpcHandleMessage(string message)
	{

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
		Player player = ((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary[conn.connectionId].GetComponent<Player>();
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Remove(player);
	}

	[Command]
	private void AddPlayer()
	{
		Player player = ((GumyzNetworkManager)GumyzNetworkManager.singleton).playerDictionary[conn.connectionId].GetComponent<Player>();
		((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Add(player);
	}
}
