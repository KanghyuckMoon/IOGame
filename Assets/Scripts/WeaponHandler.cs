using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponHandler : NetworkBehaviour
{
	//�÷��̾� ����� ��, �ش� �÷��̾��� �̸� ȣ���� ��.
	public Transform targetTrm;
	[SerializeField] private float distance = 2f;
	[SerializeField] private float speed = 50f;
	
	private float time = 0f;

	[ServerCallback]
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Enemy"))
		{
			other.GetComponent<Monster>();
		}
		else if(other.CompareTag("Player"))
		{
			//other.GetComponent<Player>();
		}
	}

	//�ڽ��� �÷��̾ �������� 360�� ȸ���� ��.

	void FixedUpdate()
	{
		if (!isServer) return;
		time += Time.fixedDeltaTime * speed;
		transform.position = targetTrm.transform.position + new Vector3(Mathf.Cos(time), 0, Mathf.Sin(time) * distance);
	}

	//������ ���� ũ�� ����
}
