using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

    private bool _isDead = false;

    public bool isDead
    {
        get{ return _isDead; }
        set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int health;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] hasEnable;

    public void Setup()
    {
        hasEnable = new bool[disableOnDeath.Length];

        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            hasEnable[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        /*if (Input.GetKeyDown("k"))
        {
            RpcTakeDamage(200);
        }*/
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
        Debug.Log(transform.name + " a respawn.");
    }

    private void SetDefaults()
    {
        health = maxHealth;
        _isDead = false;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = hasEnable[i];
        }

        Collider collider = GetComponent<Collider>();

        if(collider != null)
        {
            collider.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {

        if (_isDead)
            return;
        health -= damage;
        if (health < 0)
            health = 0;
        Debug.Log(transform.name + " a maintenant " + health + " points de vie.");

        if(health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        health = 0;
        Debug.Log(transform.name + " est mort.");

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            collider.enabled = false;
        }

        StartCoroutine(Respawn());
    }
}
