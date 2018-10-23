using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
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

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    bool firstSetup = true;

    public void Setup()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

            SetDefaults();

        }       
  
        CmdBroadcastPlayserSetup();
    }

    [Command]
    private void CmdBroadcastPlayserSetup()
    {
        RpcSetupPLayerOnAllClient();
    }

    [ClientRpc]
    private void RpcSetupPLayerOnAllClient()
    {
       /* if (firstSetup)
        {*/
            hasEnable = new bool[disableOnDeath.Length];

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                hasEnable[i] = disableOnDeath[i].enabled;
            }

            SetDefaults();

            firstSetup = false;
        /*}*/
       
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown("k"))
        {
            RpcTakeDamage(10, GetComponent<Player>().name);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        
        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
        yield return new WaitForSeconds(0.1f);
        Setup();
    }

    private void SetDefaults()
    {
        health = maxHealth;
        _isDead = false;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            Debug.Log(string.Format("element: {0} - {1}", i, disableOnDeath[i]));
            disableOnDeath[i].enabled = true;
        }

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider collider = GetComponent<Collider>();

        if(collider != null)
        {
            collider.enabled = true;
        }

        GameObject gfxInst = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(gfxInst, 3f);
    }

    public float GetHealth()
    {
        return (float)health / (float)maxHealth;
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage, string shooterPLayerId)
    {
        isDead = false;
        if (_isDead)
            return;
        health -= damage;
        if (health < 0)
            health = 0;

        if(health == 0)
        {
            Die();
            GameManager.GetPlayerScore(shooterPLayerId).IncrementFrags();
            Debug.Log("Frags :" + GameManager.GetPlayerScore(shooterPLayerId).GetFrags());
            GameManager.GetPlayerScore(GetComponent<Player>().name).IncrementDeaths();
        }
    }

    private void Die()
    {
        _isDead = true;
        health = 0;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }


        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            collider.enabled = false;
        }

        GameObject gfxInst = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gfxInst, 3f);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        StartCoroutine(Respawn()); 
    }
}
