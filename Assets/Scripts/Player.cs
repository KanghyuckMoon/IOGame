using System;
using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JamesFrowen.Spawning;

public class Player : NetworkBehaviour
{
    public string Name;
    public int Level => level;
    public float speed = 30;
    public Rigidbody rigid;
    public Vector3 LastDirection => lastDirection;
    [SerializeField] ModelHandler modelHandler;
    [SerializeField] private ClientHUD clientHud;
    [SerializeField] private TextMeshProUGUI nickName;
    [SerializeField] private Image hpImage;
    [SerializeField] private InventoryBehaviour inventoryBehaviour;
    private int level = 1;
    private int currentLevel = 1;
    private int exp = 0;
    private Vector3 lastDirection = Vector3.right;
    private int hp = 20;

    public Action<int, int> OnExpChange;
    public Action<int, int> OnLevelChange;
    public Action OnDie;

    private void Start()
    {
        hp = 20;
        level = 1;
        exp = 0;
        modelHandler = GetComponentInChildren<ModelHandler>();
        SetNickName();
        if(isOwned)
		{
            clientHud.gameObject.SetActive(true);
            OnExpChange += clientHud.UpdateExp;
            OnLevelChange += clientHud.UpdateLevel;
            OnExpChange.Invoke(exp, level);
            OnLevelChange.Invoke(currentLevel, level);
            
        }
    }

    public void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ((GumyzNetworkManager)GumyzNetworkManager.singleton).Spawn("Exp", transform.position);
            ActiveRpc(false);
        }
        else
        {
            HitRpc(hp, lastDirection);
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exp"))
		{
			AddExp(1);
            UnSpawnExpObj(other.gameObject.GetComponent<PrefabPoolBehaviour>());

            ExpObjectDelete(other.gameObject);
        }
        else if(other.CompareTag("Enemy"))
		{
            Hit(1);
		}
    }

    [Server]
    private void UnSpawnExpObj(PrefabPoolBehaviour expObj)
    {
        GumyzNetworkManager.expCount--;
        expObj.Unspawn();
    }

    [ClientRpc]
    private void ExpObjectDelete(GameObject expObj)
	{
        expObj.SetActive(false);
    }

    private void AddExp(int add)
    {
        exp+= add;
        if (exp >= level * level)
        {
            exp = 0;
            level++;
            ((GumyzNetworkManager)GumyzNetworkManager.singleton).LeaderBoard.UpdateLeaderBoard();
        }
        AddExpRpc(exp, level);
    }

    [ClientRpc]
    private void AddExpRpc(int exp, int level)
	{
        this.exp = exp;
        this.level = level;
        OnLevelChange?.Invoke(currentLevel, level);
        OnExpChange?.Invoke(exp, level);
    }


    [Command]
    public void LevelUp()
	{
        currentLevel++;
        LevelUpRpc(currentLevel);
    }

    [ClientRpc]
    private void LevelUpRpc(int currentLv)
    {
        currentLevel = currentLv;
        OnLevelChange?.Invoke(currentLevel, level);
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
            ConsiderFieldLimit();
            RotateModel(direction);

            if(Input.GetKeyDown(KeyCode.Space))
			{
                AddExp(1);
            }
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
        lastDirection = direction;
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
        string name = PlayerPrefs.GetString("Player");
        SetNickName(name);
    }

    [Command]
    private void SetNickName(string text)
	{
        Name = text;
        SetNickNameRpc(text);
    }

    [ClientRpc]
    private void SetNickNameRpc(string text)
    {
        nickName.text = text;
    }

    [ClientRpc]
    private void ActiveRpc(bool b)
    {
        gameObject.SetActive(b);
        OnDie?.Invoke();
    }

    [ClientRpc]
    private void HitRpc(int currentHp, Vector3 lastDirection)
    {
        hp = currentHp;
        transform.Translate(-lastDirection * 1);
        hpImage.fillAmount = (float)hp / 20;
        modelHandler.HitChangeMaterial();
    }

    public void Retry()
	{
        RetryServer();
    }

    [Command]
    private void RetryServer()
    {
        transform.position = Utils.GetRandomPosition();
        level = 1;
        currentLevel = 1;
        hp = 20;
        exp = 0;
        RetryRpc(level, currentLevel, exp, hp);
    }

    [ClientRpc]
    private void RetryRpc(int level, int currentLevel, int exp, int hp)
    {
        this.level = level;
        this.currentLevel = currentLevel;
        this.exp = exp;
        this.hp = hp;
        hpImage.fillAmount = (float)hp / 20;
        gameObject.SetActive(true);
        inventoryBehaviour.Retry();
        OnExpChange?.Invoke(exp, level);
        OnLevelChange?.Invoke(currentLevel, level);
    }
}
