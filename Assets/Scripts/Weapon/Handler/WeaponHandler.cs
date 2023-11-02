using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponHandler : NetworkBehaviour
{
	public bool IsMaxLevel
	{
		get
		{
			return level >= weaponStat.maxLevel;
		}
	}
	public bool IsEquip => isEquip;

	public WeaponSO WeaponStatSO => weaponStat;
	public int Level => level;

	[SerializeField] protected WeaponSO weaponStat;
	[SerializeField] protected Transform playerTrm;
	protected float currentCooldown;
	protected int level = 0;

	[SerializeField] private InventoryBehaviour inventory;

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
			inventory.AddEquipWeapon(this);
		}
		else
		{
			level++;
		}
	}

	public void Retry()
	{
		isEquip = false;
		level = 0;
		gameObject.SetActive(false);
	}
}
