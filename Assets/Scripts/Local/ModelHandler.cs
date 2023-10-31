using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ModelHandler : MonoBehaviour
{
	public void SetRotate(Vector3 direction)
	{
		transform.forward = direction;
	}
}
