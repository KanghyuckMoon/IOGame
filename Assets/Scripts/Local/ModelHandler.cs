using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ModelHandler : MonoBehaviour
{
	[SerializeField] private Material originMat;
	[SerializeField] private Material hitMat;
	[SerializeField] private Renderer meshRenderer;

	public void SetRotate(Vector3 direction)
	{
		transform.forward = direction;
	}

	public void HitChangeMaterial()
	{
		StartCoroutine(Delay());
		IEnumerator Delay()
		{
			meshRenderer.material = hitMat;
			yield return new WaitForSeconds(0.1f);
			meshRenderer.material = originMat;
		}
	}
}
