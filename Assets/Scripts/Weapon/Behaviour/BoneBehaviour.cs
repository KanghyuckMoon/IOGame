using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BoneBehaviour : NetworkBehaviour, IWeapon
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

	private WeaponStat weaponStat;
	private Transform targetTrm;
	private float time = 0f;
	private GameObject player;

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
		time += Time.fixedDeltaTime * weaponStat.speed;
		transform.position = targetTrm.transform.position + new Vector3(Mathf.Cos(time), 0, Mathf.Sin(time) * weaponStat.range);
	}

	void IWeapon.SetWeaponStat(WeaponStat weaponStat)
	{
		WeaponStat = weaponStat;
		StartCoroutine(Delay());
		IEnumerator Delay()
		{
			yield return new WaitForSeconds(weaponStat.duration);
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
}
