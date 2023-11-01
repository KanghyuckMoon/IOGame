using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientHUD : MonoBehaviour
{
	[SerializeField] private Image expImage;
	[SerializeField] private TextMeshProUGUI levelText;

	public void UpdateExp(int exp, int level)
	{
		expImage.fillAmount = (float)exp / (level * level);
	}

	public void UpdateLevel(int level)
	{
		levelText.text = $"LV {level}";
	}
}
