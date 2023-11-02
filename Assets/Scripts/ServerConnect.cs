using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerConnect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.singleton.StartServer();
    }
}
