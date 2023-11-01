using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
	[SerializeField] private Image itemImage;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private Button button;

	public void SetUpgradeButton(WeaponSO so, int level, System.Action action)
	{
		itemImage.sprite = so.itemSprite;
		descriptionText.text = so.weaponStatList[level].description;
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => action());
	}
}
