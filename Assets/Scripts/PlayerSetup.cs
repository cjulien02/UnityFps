using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    private Behaviour[] componentToDisable;

    Camera sceneCamera;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            foreach( Behaviour component in componentToDisable)
                component.enabled = false;
        }
        else
        {
            sceneCamera = Camera.main;

            if(sceneCamera != null)
            {
                sceneCamera.transform.gameObject.SetActive(false);
            }
           
        }
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.transform.gameObject.SetActive(true);
        }
    }
}
