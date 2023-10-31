using Mirror;
using UnityEngine;
using TMPro;

public class Player : NetworkBehaviour
{
    public float speed = 30;
    public Rigidbody rigid;
    [SerializeField] ModelHandler modelHandler;
    [SerializeField] private TextMeshProUGUI nickName;

    private Vector3 _direction;

    private void Start()
    {
        modelHandler = GetComponentInChildren<ModelHandler>();
        SetNickName();
    }

    public void SetNickName()
    {
        if (isOwned)
        {
            SetNickClient();
        }
    }

	// need to use FixedUpdate for rigidbody
	void FixedUpdate()
    {
        //
        // only let the local player control the racket.
        // don't control other player's rackets
        if (isOwned)
		{
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            rigid.velocity = direction * speed * Time.fixedDeltaTime;

            RotateModel(direction);
        }
    }

    private void ConsiderFieldLimit()
    {
        if (transform.position.x > Utils.GetPlayField() / 2)
        {
            rigid.velocity = new Vector3(0, 0, rigid.velocity.z);
            transform.position = new Vector3(Utils.GetPlayField() / 2, 0, transform.position.z);
        }
        if (transform.position.x < -Utils.GetPlayField() / 2)
        {
            rigid.velocity = new Vector3(0, 0, rigid.velocity.z);
            transform.position = new Vector3(-Utils.GetPlayField() / 2, 0, transform.position.z);
        }

        if (transform.position.y > Utils.GetPlayField() / 2)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, Utils.GetPlayField() / 2);
        }
        if (transform.position.y < -Utils.GetPlayField() / 2)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, -Utils.GetPlayField() / 2);
        }
    }

	private void LateUpdate()
    {
        if (isOwned)
		{
            Camera.main.transform.position = transform.position + new Vector3(0,8,-4);
		}
    }

    private void RotateModel(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            SetDirection(direction);
        }
    }

    [Command]
    private void SetDirection(Vector3 direction)
    {
        SetDirectionRpc(direction);
    }

    [ClientRpc]
    private void SetDirectionRpc(Vector3 direction)
    {
        modelHandler.SetRotate(direction);
    }

    [Client]
    private void SetNickClient()
    {
        SetNickName(PlayerPrefs.GetString("Player"));
    }

    [Command]
    private void SetNickName(string text)
	{
        SetNickNameRpc(text);
    }

    [ClientRpc]
    private void SetNickNameRpc(string text)
    {
        nickName.text = text;
    }
}
