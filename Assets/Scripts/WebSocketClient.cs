using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using System.Text;

public class WebSocketClient : MonoBehaviour
{
	private readonly string IP = "127.0.0.1";
	private readonly string PORT = "127.0.0.1";
	//Server Service's Name
	private readonly string SERVICE_NAME = "/Server";

	public WebSocketSharp.WebSocket m_Socket = null;

	private void Start()
	{
		try
		{
			m_Socket = new WebSocketSharp.WebSocket("ws://" + IP + ":" + PORT + SERVICE_NAME);
			m_Socket.OnMessage += Recv;
			m_Socket.OnClose += CloseConnect;
		}
		catch
		{

		}
	}

	public void Connect()
	{
		try
		{
			if (m_Socket == null || !m_Socket.IsAlive)
				m_Socket.Connect();
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	public void CloseConnect(object sender, CloseEventArgs e)
	{
		DisConnectServer();
	}

	public void DisConnectServer()
	{
		try
		{
			if (m_Socket == null)
				return;

			if (m_Socket.IsAlive)
				m_Socket.Close();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	public void SendMessage(string msg)
	{
		if (!m_Socket.IsAlive)
			return;
		try
		{
			m_Socket.Send(Encoding.UTF8.GetBytes(msg));
		}
		catch(Exception e)
		{
			throw;
		}
	}

	public void Recv(object sender, MessageEventArgs e)
	{
		Debug.Log(e.Data);

		Debug.Log(e.RawData);
	}

	private void OnApplicationQuit()
	{
		DisConnectServer();
	}

}
