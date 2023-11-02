using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LeaderBoard : NetworkBehaviour
{
	public TextMeshProUGUI text;

	[Server]
	public void UpdateLeaderBoard()
	{
		string[] reusltString = new string[10];
		List<Player> playerList = ((GumyzNetworkManager)GumyzNetworkManager.singleton).GetHighPlayerList();
		int i = 0;
		foreach(var player in playerList)
		{
			reusltString[i++] = $"{i}.{player.Name} LV {player.Level}";
		}
		UpdateLeaderBoardRpc(reusltString);
	}

	[ClientRpc]
	public void UpdateLeaderBoardRpc(string[] players)
	{
		text.text = "";
		for (int i = 0; i < 10; ++i)
		{
			text.text += players[i];
			text.text += '\n';
		}
	}
}
