using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using JamesFrowen.Spawning;

public class SkullBehaviour : NetworkBehaviour, IWeapon
{
	public Transform TargetTransform
	{
		get
		{
			return targetTrm;
		}
		set
		{
			targetTrm = value;
		}
	}
	public WeaponStat WeaponStat
	{
		get
		{
			return weaponStat;
		}
		set
		{
			weaponStat = value;
		}
	}

	private Transform targetTrm;
	private WeaponStat weaponStat;
	private ModelHandler modelHandler;
	private Vector3 lastPoint;
	private Vector3 currentDirection;
	private GameObject player;

	private void OnEnable()
	{
		if (isServer)
		{
			ActiveRpc(true);
		}
	}

	private void Start()
	{
		modelHandler = GetComponentInChildren<ModelHandler>();
	}

	[ServerCallback]
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<Monster>().Hit(weaponStat.damage);
		}
		else if (other.CompareTag("Player") && other.gameObject != player)
		{
			other.GetComponent<Player>().Hit(weaponStat.damage);
		}
	}

	void FixedUpdate()
	{
		Vector3 direction = Vector3.zero;
		if (isServer)
		{
			if (targetTrm == null)
			{
				DestoryWeaponRpc();
			}
			direction = (targetTrm.position - transform.position);
			direction.y = 0;
			direction = direction.normalized;
			transform.Translate(direction * weaponStat.speed * Time.fixedDeltaTime);
			Rotate(direction);
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
	private void Rotate(Vector3 direction)
	{
		modelHandler.SetRotate(direction);
	}

	void IWeapon.SetWeaponStat(WeaponStat weaponStat)
	{
		WeaponStat = weaponStat;
		StartCoroutine(Delay());
		IEnumerator Delay()
		{
			yield return new WaitForSeconds(weaponStat.duration);
			GetComponent<PrefabPoolBehaviour>().Unspawn();
			DestoryWeaponRpc();
		}
	}

	[ClientRpc]
	private void DestoryWeaponRpc()
	{
		gameObject.SetActive(false);
	}

	public void SetPlayer(GameObject player)
	{
		this.player = player;
	}

	[ClientRpc]
	private void ActiveRpc(bool b)
	{
		gameObject.SetActive(b);
	}
}
