using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientConnect : MonoBehaviour
{
    void Start()
    {
        NetworkManager.singleton.StartClient();
    }
}
