using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovementHandler : NetworkBehaviour
{
	Vector2 inputDirection = Vector2.zero;
	ushort size = 5;

	//Components
	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidbody2D_;

	void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rigidbody2D_ = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		inputDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
	}

	private void Update()
	{
		if (!IsOwner) return;
		inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void FixedUpdate()
	{
		if (!IsOwner) return;
		Vector2 movementDirection = ClampMoveDirection();
		movementDirection.Normalize();
		float movementSpeed = (size / Mathf.Pow(size, 1.1f)) * 2;
		rigidbody2D_.AddForce(movementDirection * movementSpeed, ForceMode2D.Impulse);
		if (rigidbody2D_.velocity.magnitude > movementSpeed)
		{
			rigidbody2D_.velocity = rigidbody2D_.velocity.normalized * movementSpeed;
		}
	}

	private Vector2 ClampMoveDirection()
	{
		Vector2 movementDirection = inputDirection;
		if (transform.position.x < Utils.GetPlayFieldSize() / 2f * -1 + spriteRenderer.transform.localScale.x / 2f && movementDirection.x < 0)
		{
			movementDirection.x = 0;
			rigidbody2D_.velocity = new Vector2(0, rigidbody2D_.velocity.y);
		}
		if (transform.position.x > Utils.GetPlayFieldSize() / 2f + spriteRenderer.transform.localScale.x / 2f && movementDirection.x < 0)
		{
			movementDirection.x = 0;
			rigidbody2D_.velocity = new Vector2(0, rigidbody2D_.velocity.y);
		}
		if (transform.position.y < Utils.GetPlayFieldSize() / 2f * -1 + spriteRenderer.transform.localScale.y / 2f && movementDirection.y < 0)
		{
			movementDirection.y = 0;
			rigidbody2D_.velocity = new Vector2(rigidbody2D_.velocity.x, 0);
		}
		if (transform.position.y > Utils.GetPlayFieldSize() / 2f + spriteRenderer.transform.localScale.y / 2f && movementDirection.y < 0)
		{
			movementDirection.y = 0;
			rigidbody2D_.velocity = new Vector2(rigidbody2D_.velocity.x, 0);
		}

		return movementDirection;
	}

	private void LateUpdate()
	{
		if (IsOwner)
		{
			float aspectRatio = Camera.main.aspect;
			float orthorSize = (spriteRenderer.transform.localScale.x + 7) / aspectRatio;

			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthorSize, Time.deltaTime * 0.1f);
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y, -10), Time.deltaTime);
		}
	}
}
