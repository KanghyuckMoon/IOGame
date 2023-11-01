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
			int index = 0;
			int count = weaponStat.weaponStatList[level].amount;
			for (int i = 0; i < count; ++i)
			{
				index++;
				SpawnSkull(index);
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	[Command]
	private void SpawnSkull(int index)
	{
		Collider[] colliders = Physics.OverlapSphere(playerTrm.position, 10);
		if (colliders.Length < 2) return;
		Transform target = colliders.OrderBy(x => (playerTrm.position - x.transform.position).magnitude).ElementAt((index % colliders.Length) + 1).transform;
		if (target == null) return;

		var weapon = ((GumyzNetworkManager)NetworkManager.singleton).SpawnWithOutSpawn("Skull", playerTrm.position);
		IWeapon iWeapon = weapon.GetComponent<IWeapon>();
		iWeapon.SetTargetTrm(target);
		iWeapon.SetWeaponStat(weaponStat.weaponStatList[level]);
		NetworkServer.Spawn(weapon);
	}
}
