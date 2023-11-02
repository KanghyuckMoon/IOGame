using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryBehaviour : MonoBehaviour
{
	private readonly int maxWeapon = 3;
	[SerializeField] private List<WeaponHandler> allWeaponHandlerList = new List<WeaponHandler>();
	private List<WeaponHandler> equipWeaponList = new List<WeaponHandler>();

	public void Retry()
	{
		equipWeaponList.Clear();
		foreach (var weapon in allWeaponHandlerList)
		{
			weapon.Retry();
		}
	}

	public void AddEquipWeapon(WeaponHandler weaponHandler)
	{
		equipWeaponList.Add(weaponHandler);
	}

	public List<WeaponHandler> GetRandomWeaponList()
	{
		List<WeaponHandler> resultWeapon = new List<WeaponHandler>();

		//최대 무기수인지 확인한다
		if(equipWeaponList.Count >= maxWeapon)
		{
			//최대 무기수이면 지금 가지고 있는 무기 업그레이드만 보여준다.
			foreach (var weapon in equipWeaponList)
			{
				if (!weapon.IsMaxLevel)
				{
					resultWeapon.Add(weapon);
				}
			}
		}
		else
		{
			//그렇지 않으면 현재 가지고 있는 무기를 포함해서 모든 무기를 보여준다.
			resultWeapon = allWeaponHandlerList.OrderBy(g => Random.Range(0, 100)).Where(x => !x.IsMaxLevel).Take(3).ToList();
		}

		return resultWeapon;
	}
}
