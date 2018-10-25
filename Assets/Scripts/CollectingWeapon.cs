using UnityEngine;
using UnityEngine.UI;

public class CollectingWeapon : MonoBehaviour {
    [SerializeField]
    private PlayerWeapon weapon;


    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.SetInfoText("Press 'F' Key to interact");
            player.canInteract = true;

            player.interactableObject = weapon;
            player.interactableObjectContainer = this.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.SetInfoText("");
            player.canInteract = false;
            player.interactableObject = null;
            player.interactableObjectContainer = null;
        }
    }

}
