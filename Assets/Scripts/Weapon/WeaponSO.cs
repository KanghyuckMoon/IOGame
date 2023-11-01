using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
	public Sprite itemSprite;
	public string weaponKey;
	public int maxLevel = 10;
	public WeaponStat[] weaponStatList = new WeaponStat[10];
}

[System.Serializable]
public class WeaponStat
{
	public string weaponKey;
	public string description;
	public float range = 2f;
	public float speed = 10;
	public float duration = 1;
	public float cooldownDuration = 1;
	public int damage = 10;
	public int amount = 1;
	public int pierce = 1;
}
