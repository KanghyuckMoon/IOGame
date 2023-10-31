using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class GumyzNetworkManager : NetworkManager
{
    public readonly List<Transform> playerList = new List<Transform>();
    [SerializeField] MonsterSpawner monsterSpawner;
    private int monsterCount;
    private float delay = 0f;

    public override void OnStartServer()
    {
        StartCoroutine(SpawnMonster());
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<JoinBehaviour>().SetClient(conn);
        NetworkServer.AddPlayerForConnection(conn, player);

        foreach(var playerTrm in playerList)
		{
			try
            {
                Player playerObj = playerTrm.GetComponent<Player>();
                playerObj.SetNickName();
            }
            catch(System.Exception e)
			{
                Debug.LogError(e);
			}
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }

    private IEnumerator SpawnMonster()
	{
        while(true)
		{
            yield return new WaitForSeconds(1f);
            Spawn();
        }
	}

    public void Spawn()
    {
        monsterCount++;
        GameObject monsterInstance = Instantiate(NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Monster"));
        monsterInstance.transform.position = Utils.GetRandomPosition();
        NetworkServer.Spawn(monsterInstance);
    }
}
