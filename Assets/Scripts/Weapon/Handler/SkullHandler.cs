using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class SkullHandler : WeaponHandler
{
	protected override void Attack()
	{
		base.Attack();
		if (isOwned)
		{
			StartCoroutine(Delay());
		}

		IEnumerator Delay()
		{
			int count = weaponStat.weaponStatList[level].amount;
			for (int i = 0; i < count; ++i)
			{
				SpawnSkull();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	[Command]
	private void SpawnSkull()
	{
		Collider[] colliders = Physics.OverlapSphere(playerTrm.position, 10);
		if (colliders == null) return;
		Transform target = colliders.OrderBy(x => (playerTrm.position - x.transform.position).magnitude).First().transform;
		if (target == null) return;

		var weapon = ((GumyzNetworkManager)NetworkManager.singleton).SpawnWithOutSpawn("Skull", playerTrm.position);
		IWeapon iWeapon = weapon.GetComponent<IWeapon>();
		iWeapon.SetTargetTrm(target);
		iWeapon.SetWeaponStat(weaponStat.weaponStatList[level]);
		NetworkServer.Spawn(weapon);
	}
}
