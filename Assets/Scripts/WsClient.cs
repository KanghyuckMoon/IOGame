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
        //�������� ������ ��Ʈ�� �־��ݴϴ�.


        ws.Connect();
        //�����մϴ�.
        //ws.OnMessage += (byte[] msg) => 
        //{
        //    Call(msg);
        //}; 
    }

    void Call(byte[] msg)
    {
        Debug.Log("������ : " + Encoding.UTF8.GetString(msg));
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
            //�����͸� �����ϴ� �����̱� ������ "abcd" �� �����ϴ�
        }
    }
}