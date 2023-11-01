using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArrowHandler : WeaponHandler
{
	[SerializeField] private Player player;

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
				SpawnArrow();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	[Command]
	private void SpawnArrow()
	{
		var weapon = ((GumyzNetworkManager)NetworkManager.singleton).SpawnWithOutSpawn("Sword", playerTrm.position);
		IWeapon iWeapon = weapon.GetComponent<IWeapon>();
		iWeapon.SetTargetTrm(playerTrm);
		iWeapon.SetDirection(player.LastDirection);
		iWeapon.SetWeaponStat(weaponStat.weaponStatList[level]);
		NetworkServer.Spawn(weapon);
	}
}
