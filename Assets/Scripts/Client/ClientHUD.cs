using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientHUD : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private Image expImage;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private GameObject levelUpPanel;
	[SerializeField] private InventoryBehaviour inventoryBehaviour;
	[SerializeField] private UpgradeButton[] upgradeButton = new UpgradeButton[3];

	public void UpdateExp(int exp, int level)
	{
		expImage.fillAmount = (float)exp / (level * level);
	}

	public void UpdateLevel(int currentLevel, int level)
	{
		levelText.text = $"LV {level}";

		if (currentLevel == level)
		{
			HideLevelUpPanel();
		}
		else
		{
			ShowLevelUpPanel();
		}
	}

	public void ShowLevelUpPanel()
	{
		List<WeaponHandler> weaponHandlerList = inventoryBehaviour.GetRandomWeaponList();

		if (weaponHandlerList.Count == 0) return;

		levelUpPanel.SetActive(true);

		for (int i = 0; i < upgradeButton.Length; ++i)
		{
			int j = i;
			if (j < weaponHandlerList.Count)
			{
				upgradeButton[j].SetUpgradeButton(weaponHandlerList[i].WeaponStatSO, weaponHandlerList[i].Level, () => {
					player.LevelUp();
					weaponHandlerList[j].LevelUp();
				});
				upgradeButton[j].gameObject.SetActive(true);
			}
			else
			{
				upgradeButton[j].gameObject.SetActive(false);
			}
		}

	}

	public void HideLevelUpPanel()
	{
		levelUpPanel.SetActive(false);
	}
}
