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
			StartCoroutine(Delay());
		}

		IEnumerator Delay()
		{
			int count = weaponStat.weaponStatList[level].amount;
			for (int i = 0; i < count; ++i)
			{
				SpawnBone();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	[Command]
	private void SpawnBone()
	{
		var weapon = ((GumyzNetworkManager)NetworkManager.singleton).SpawnWithOutSpawn("Bone", playerTrm.position);
		IWeapon iWeapon = weapon.GetComponent<IWeapon>();
		iWeapon.SetTargetTrm(playerTrm);
		iWeapon.SetPlayer(playerTrm.gameObject);
		iWeapon.SetWeaponStat(weaponStat.weaponStatList[level]);
		NetworkServer.Spawn(weapon);
	}
}
