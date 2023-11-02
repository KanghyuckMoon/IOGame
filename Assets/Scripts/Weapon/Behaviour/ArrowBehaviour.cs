using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using JamesFrowen.Spawning;

public class ArrowBehaviour : NetworkBehaviour, IWeapon
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
	private Vector3 direction;
	private GameObject player;

	private void OnEnable()
	{
		if (isServer)
		{
			ActiveRpc(true);
			SetDirectionRpc(direction);
		}
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
		if (!isServer) return;
		transform.Translate(direction * weaponStat.speed * Time.fixedDeltaTime, Space.World);
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

	void IWeapon.SetDirection(Vector3 direction)
	{
		this.direction = direction;
		transform.forward = direction;
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
	[ClientRpc]
	private void SetDirectionRpc(Vector3 direction)
	{
		this.direction = direction;
		transform.forward = direction;
	}
}
