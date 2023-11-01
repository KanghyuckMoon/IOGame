using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<Monster>().Hit(weaponStat.damage);
		}
		else if (other.CompareTag("Player"))
		{
			//other.GetComponent<Player>();
		}
	}

	void FixedUpdate()
	{
		if (!isServer) return;
		Vector3 direction = (targetTrm.position - transform.position);
		direction.y = 0;
		direction = direction.normalized;
		transform.LookAt(targetTrm);
		transform.Translate(direction * weaponStat.speed * Time.fixedDeltaTime);
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
}
