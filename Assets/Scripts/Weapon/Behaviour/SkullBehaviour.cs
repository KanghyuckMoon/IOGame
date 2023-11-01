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
	private ModelHandler modelHandler;
	private Vector3 lastPoint;
	private Vector3 currentDirection;

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
		else if (other.CompareTag("Player"))
		{
			//other.GetComponent<Player>();
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
			DestoryWeaponRpc();
		}
	}

	[ClientRpc]
	private void DestoryWeaponRpc()
	{
		gameObject.SetActive(false);
	}
}
