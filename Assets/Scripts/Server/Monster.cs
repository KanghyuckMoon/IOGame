using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Monster : NetworkBehaviour
{
	[SerializeField] private float speed = 10f;
	private Transform target;
	private ModelHandler modelHandler;
	private Vector3 lastPoint;
	private int hp = 10;
	private Vector3 currentDirection;

	private void Start()
	{
		modelHandler = GetComponentInChildren<ModelHandler>();
		hp = 10;
		//Set Target
	}

	private void FixedUpdate()
	{
		Vector3 direction = Vector3.zero;
		if (isServer)
		{
			CheckTargetNull();

			if (target != null)
			{
				direction = target.transform.position - transform.position;
				direction.y = 0;
				direction = direction.normalized;
				currentDirection = direction;
				Move(direction);
				Rotate(direction);
				lastPoint = transform.position;
			}
		}
		else if (isClient)
		{
			direction = transform.position - lastPoint;
			direction.y = 0;
			direction = direction.normalized;
			currentDirection = direction;
			if (direction != Vector3.zero)
			{
				Rotate(direction);
				lastPoint = transform.position;
			}
		}
	}

	private void CheckTargetNull()
	{
		try
		{
			if (target == null)
			{
				int count = ((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList.Count;
				if (count == 0)
				{
					target = null;
					return;
				}
				target = ((GumyzNetworkManager)GumyzNetworkManager.singleton).playerList[Random.Range(0, count)];
				//Å¸°Ù ¼³Á¤
			}
		}
		catch
		{
			target = null;
		}
	}

	private void Move(Vector3 direction)
	{
		transform.Translate(direction * speed * Time.fixedDeltaTime);
	}

	private void Rotate(Vector3 direction)
	{
		modelHandler.SetRotate(direction);
	}

	public void Hit(int damage)
	{
		hp -= damage;
		if (hp <= 0)
		{
			((GumyzNetworkManager)GumyzNetworkManager.singleton).Spawn("Exp", transform.position);
			ActiveRpc(false);
		}
		else
		{
			HitRpc(hp);
		}
	}

	[ClientRpc]
	private void ActiveRpc(bool b)
	{
		gameObject.SetActive(b);
	}

	[ClientRpc]
	private void HitRpc(int currentHp)
	{
		hp = currentHp;
		transform.Translate(-currentDirection * 1);
		modelHandler.HitChangeMaterial();
	}

}
