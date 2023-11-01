using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
	public Transform TargetTransform
	{
		get;
		set;
	}

	public WeaponStat WeaponStat
	{
		get;
		set;
	}

	public void SetWeaponStat(WeaponStat weaponStat)
	{
		WeaponStat = weaponStat;
	}

	public void SetTargetTrm(Transform target)
	{
		TargetTransform = target;
	}
	public void SetDirection(Vector3 direction)
	{

	}

	public void SetPlayer(GameObject player);

}
