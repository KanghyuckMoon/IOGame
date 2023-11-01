using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
	public string weaponKey;
	public int maxLevel = 10;
	public WeaponStat[] weaponStatList = new WeaponStat[10];
}

[System.Serializable]
public class WeaponStat
{
	public string weaponKey;
	public float damage = 10;
	public float speed = 10;
	public float cooldownDuration = 1;
	public int pierce = 1;
}
