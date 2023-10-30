using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkStarter : MonoBehaviour
{
	public void Start()
	{
#if UNITY_EDITOR
		NetworkManager.Singleton.StartHost();
#elif UNITY_SERVER
		NetworkManager.Singleton.StartServer();
#else
		NetworkManager.Singleton.StartClient();
#endif

	}
}
