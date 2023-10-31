using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponHandler : NetworkBehaviour
{
	//플레이어 당았을 때, 해당 플레이어의 이름 호출할 것.
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

	//자신의 플레이어를 기준으로 360도 회전할 것.

	void FixedUpdate()
	{
		if (!isServer) return;
		time += Time.fixedDeltaTime * speed;
		transform.position = targetTrm.transform.position + new Vector3(Mathf.Cos(time), 0, Mathf.Sin(time) * distance);
	}

	//레벨에 따라 크기 증가
}
