using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
    

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;
    
    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentWeaponGraphics;

    private void Start () {
        EquipeWeapon(primaryWeapon);
	}

    public PlayerWeapon GetCurrentWeapon()
    {
        if (currentWeapon == null)
            currentWeapon = primaryWeapon;
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentWeaponGraphics()
    {
        
        return currentWeaponGraphics;
    }


    private void EquipeWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;        

        GameObject weaponInst = (GameObject)Instantiate(weapon.weaponGraphics, weaponHolder.position, weaponHolder.rotation);
        weaponInst.transform.SetParent(weaponHolder);

        currentWeaponGraphics = weaponInst.GetComponent<WeaponGraphics>();

        if (isLocalPlayer)
        {
            weaponInst.layer = LayerMask.NameToLayer(weaponLayerName);
        }

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponInst, LayerMask.NameToLayer(weaponLayerName));
        }
    } 
	
}
