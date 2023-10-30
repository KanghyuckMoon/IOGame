using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using WebSocketSharp;

// Use plugin namespace
using HybridWebSocket;
    
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    private void Start()
    {
        //
        ws = WebSocketFactory.CreateInstance("ws://localhost:7777");
        //서버에서 설정한 포트를 넣어줍니다.


        ws.Connect();
        //연결합니다.
        //ws.OnMessage += (byte[] msg) => 
        //{
        //    Call(msg);
        //}; 
    }

    void Call(byte[] msg)
    {
        Debug.Log("데이터 : " + Encoding.UTF8.GetString(msg));
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send(Encoding.UTF8.GetBytes("abcd"));
            //데이터를 보냅니다 예제이기 때문에 "abcd" 를 보냅니다
        }
    }
}