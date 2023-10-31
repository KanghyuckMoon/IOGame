using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static Vector3 GetRandomPosition()
	{
		float halfField = GetPlayField();
		Vector3 resultPos = new Vector3(Random.Range(-halfField, halfField),0, Random.Range(-halfField, halfField));
		return resultPos;
	}

	public static float GetPlayField()
	{
		return 25;
	}

	public static int GetMonsterMaxCount()
	{
		return 10;
	}
}
