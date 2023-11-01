using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BoneHandler : WeaponHandler
{
	protected override void Attack()
	{
		base.Attack();
		if(isOwned)
		{
			SpawnBone();
		}
	}

	[Command]
	private void SpawnBone()
	{
		var weapon = Instantiate(NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Bone"));
		weapon.GetComponent<IWeapon>().SetTargetTrm(targetTrm);
		weapon.GetComponent<IWeapon>().SetWeaponStat(weaponStat.weaponStatList[level]);
		NetworkServer.Spawn(weapon);
	}
}
