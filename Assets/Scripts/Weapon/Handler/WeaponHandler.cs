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

	public Transform playerTrm;
	
	private float time = 0f;
	private bool isEquip;
		
	protected virtual void FixedUpdate()
	{
		if (isOwned)
		{
			currentCooldown -= Time.fixedDeltaTime;
			if (currentCooldown <= 0f)
			{
				Attack();
			}
		}
	}

	protected virtual void Attack()
	{
		currentCooldown = weaponStat.weaponStatList[level].cooldownDuration;
	}

	public void LevelUp()
	{
		if(!isEquip)
		{
			isEquip = true;
			gameObject.SetActive(true);
		}
		else
		{
			level++;
		}
	}
}
