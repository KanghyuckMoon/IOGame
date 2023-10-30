using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static Vector3 GetRandomSpawnPosition()
	{
		float value = GetPlayFieldSize() / 2f;
		return new Vector3(Random.Range(-value, value), Random.Range(-value, value), 0) * 0.9f;
	}

	public static float GetPlayFieldSize()
	{
		return 50;
	}

	public static string GetRandomName()
	{
		string[] names = { "Eddy", "Freddy", "Teddy" };

		return names[Random.Range(0, names.Length)] + Random.Range(1, 100);
	}
}
