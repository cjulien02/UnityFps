using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		if(cam == null)
        {
            Debug.LogError("Pas de caméra");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
	}

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
           
        }
       
    }

    [Command]
    private void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    private void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        
        Destroy(hitEffect, 2f);
    }

    [Command]
    private void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    private void RpcDoShootEffect()
    {
        weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();
    }

    [Client]
    private void Shoot()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit _hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if(_hit.collider.tag == "Player")
            {
                CmdPlayerShoot(_hit.collider.name, currentWeapon.damage);
            }
        }

        CmdOnHit(_hit.point, _hit.normal);
    }

    [Command]
	private void CmdPlayerShoot(string playerID, int damage)
    {
        Debug.Log(playerID + " a été touché.");

        Player player = GameManager.GetPlayer(playerID);

        player.RpcTakeDamage(damage);
    }
}
