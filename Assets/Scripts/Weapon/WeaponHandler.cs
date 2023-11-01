using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponHandler : NetworkBehaviour
{
	[Header("Weapon Stats")]
	public WeaponSO weaponStat;
	protected float currentCooldown;
	protected int level = 0;

	public Transform targetTrm;
	
	private float time = 0f;

	protected virtual void Start()
	{
		currentCooldown = weaponStat.weaponStatList[level].cooldownDuration;
	}

	[ServerCallback]
	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Enemy"))
		{
			other.GetComponent<Monster>().Hit(1);
		}
		else if(other.CompareTag("Player"))
		{
			//other.GetComponent<Player>();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!isServer) return;
		currentCooldown -= Time.fixedDeltaTime;
		if(currentCooldown <= 0f)
		{
			Attack();
		}
	}

	protected virtual void Attack()
	{
		currentCooldown = weaponStat.weaponStatList[level].cooldownDuration;
	}
}
