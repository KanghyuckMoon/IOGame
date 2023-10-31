using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterSpawner : NetworkBehaviour
{
	[SerializeField] private GameObject monsterPrefab;
	private int monsterCount;

}
