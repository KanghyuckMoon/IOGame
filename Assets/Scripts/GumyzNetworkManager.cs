using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using JamesFrowen.Spawning;

[AddComponentMenu("")]
public class GumyzNetworkManager : NetworkManager
{
    public LeaderBoard LeaderBoard => leaderBoard;
    public readonly List<Player> playerList = new List<Player>();
    public readonly Dictionary<int, Transform> playerDic = new Dictionary<int, Transform>();
    private readonly Dictionary<string, MirrorPrefabPool> mirrorPrefabPoolDic = new Dictionary<string, MirrorPrefabPool>();
    public static int expCount;
    [SerializeField] LeaderBoard leaderBoard;

    public override void OnStartServer()
    {
        StartCoroutine(SpawnMonster());
        NetworkServer.Spawn(leaderBoard.gameObject);
        leaderBoard.gameObject.SetActive(true);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<JoinBehaviour>().SetClient(conn);
        NetworkServer.AddPlayerForConnection(conn, player);
        foreach (var playerObj in playerList)
		{
			try
            {
                playerObj.SetNickName();
            }
            catch(System.Exception e)
			{
                Debug.LogError(e);
			}
        }
        leaderBoard.UpdateLeaderBoard();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        leaderBoard.UpdateLeaderBoard();
    }

    private IEnumerator SpawnMonster()
	{
        while(true)
		{
            yield return new WaitForSeconds(1f);
            if (Monster.count < 20)
            {
                MonsterSpawn();
            }
            if (expCount < 20)
            {
                ExpSpawn();
            }
        }
	}

    public void MonsterSpawn()
    {
        Spawn("Monster", Utils.GetRandomPosition());
    }
    public void ExpSpawn()
    {
        expCount++;
        Spawn("Exp", Utils.GetRandomPosition());
    }

    public GameObject Spawn(string key, Vector3 pos)
	{
        GameObject instance = SpawnWithOutSpawn(key, pos);
        NetworkServer.Spawn(instance);
        return instance;
    }

    public GameObject SpawnWithOutSpawn(string key, Vector3 pos)
    {
        if (!mirrorPrefabPoolDic.ContainsKey(key))
		{
            GameObject prefab = spawnPrefabs.Find(prefab => prefab.name == key);
            mirrorPrefabPoolDic.Add(key, new MirrorPrefabPool(prefab, 30));
		}
        GameObject instance = mirrorPrefabPoolDic[key].Spawn(pos, Quaternion.identity).gameObject;
        return instance;
    }

    public List<Player> GetHighPlayerList()
    {
        List<Player> resultList;
        resultList = playerList.OrderByDescending(g => g.Level).Take(10).ToList();
        return resultList;
    }

}
