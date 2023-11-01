using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientHUD : MonoBehaviour
{
	[SerializeField] private Image expImage;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private GameObject levelUpPanel;
	
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
		levelUpPanel.SetActive(true);
	}

	public void HideLevelUpPanel()
	{
		levelUpPanel.SetActive(false);
	}
}
