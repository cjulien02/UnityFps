using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	// Use this for initialization
	void Start () {
		if(cam == null)
        {
            Debug.LogError("Pas de caméra");
            this.enabled = false;
        }
	}

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit _hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if(_hit.collider.tag == "Player")
            {
                CmdPlayerShoot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
	private void CmdPlayerShoot(string playerID, int damage)
    {
        Debug.Log(playerID + " a été touché.");

        Player player = GameManager.GetPlayer(playerID);

        player.RpcTakeDamage(damage);
    }
}
