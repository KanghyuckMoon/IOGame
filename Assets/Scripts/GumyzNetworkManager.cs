using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class GumyzNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        //Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        // spawn ball if two players
        //if (numPlayers == 2)
        //{
        //    ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
        //    NetworkServer.Spawn(ball);
        //}
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // destroy ball
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
}
