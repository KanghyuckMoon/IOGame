using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ChatBehaviour : NetworkBehaviour
{
	[SerializeField] private Canvas chatUI;
	[SerializeField] private TMP_Text chatText;
	[SerializeField] private TMP_InputField inputField;

	private static event Action<string> OnMessage;

	public override void OnStartAuthority()
	{
		chatUI.gameObject.SetActive(true);

		OnMessage += HandleNewMessage;
	}

	private void HandleNewMessage(string str)
	{
		chatText.text += str;
	}

	[ClientCallback]
	private void OnDestroy()
	{
		if (!isOwned) return;
		OnMessage -= HandleNewMessage;
	}

	[Client]
	public void Send(string message)
	{
		if (!Input.GetKeyDown(KeyCode.Return)) return;
		if (string.IsNullOrEmpty(message)) return;

		CmdSendMessage(inputField.text);
		inputField.text = string.Empty;
	}

	[Command]
	private void CmdSendMessage(string message)
	{
		RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
	}

	[ClientRpc]
	private void RpcHandleMessage(string message)
	{
		OnMessage?.Invoke($"\n{message}");
	}
}
