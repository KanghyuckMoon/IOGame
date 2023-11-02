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

		//�ִ� ��������� Ȯ���Ѵ�
		if(equipWeaponList.Count >= maxWeapon)
		{
			//�ִ� ������̸� ���� ������ �ִ� ���� ���׷��̵常 �����ش�.
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
			//�׷��� ������ ���� ������ �ִ� ���⸦ �����ؼ� ��� ���⸦ �����ش�.
			resultWeapon = allWeaponHandlerList.OrderBy(g => Random.Range(0, 100)).Where(x => !x.IsMaxLevel).Take(3).ToList();
		}

		return resultWeapon;
	}
}
