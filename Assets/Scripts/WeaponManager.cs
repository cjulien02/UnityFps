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


    public void EquipeWeapon(PlayerWeapon weapon)
    {
        Transform currentTransform = weapon.weaponGraphics.transform;
        currentWeapon = weapon;

        currentWeapon.weaponGraphics.transform.rotation = weaponHolder.rotation;
        currentWeapon.weaponGraphics.transform.position = weaponHolder.position;
        if (weaponHolder.childCount > 0)
        {
            Destroy(weaponHolder.GetChild(0).gameObject);
        }
            
        GameObject weaponInst = (GameObject)Instantiate(weapon.weaponGraphics, weaponHolder.position, weaponHolder.rotation);
        currentWeaponGraphics = weaponInst.GetComponent<WeaponGraphics>();

        weaponInst.transform.SetParent(weaponHolder);

        currentWeaponGraphics = weaponInst.GetComponent<WeaponGraphics>();
        Debug.Log("Rotation : " + currentWeaponGraphics.rotationX);
        weaponInst.transform.rotation =  Quaternion.Euler(currentWeaponGraphics.rotationX, 0, 0);


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
