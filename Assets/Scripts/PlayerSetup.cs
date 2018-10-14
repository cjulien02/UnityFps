using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    private Behaviour[] componentToDisable;

    [SerializeField]
    private string remotePlayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemotePlayer();
        }
        else
        {
            sceneCamera = Camera.main;

            if(sceneCamera != null)
            {
                sceneCamera.transform.gameObject.SetActive(false);
            }
           
        }

        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in componentToDisable)
            component.enabled = false;
    }

    private void AssignRemotePlayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remotePlayerName);
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.transform.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
