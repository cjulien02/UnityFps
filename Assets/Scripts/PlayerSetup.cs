using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    private Behaviour[] componentToDisable;

    [SerializeField]
    private string remotePlayerName = "RemotePlayer";

    [SerializeField]
    private string DontDrawLayerName = "DontDraw";

    [SerializeField]
    private GameObject physicGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemotePlayer();
        }
        else
        {
            SetLayerRecursively(physicGraphics, LayerMask.NameToLayer(DontDrawLayerName));
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI playerUI = playerUIInstance.GetComponent<PlayerUI>();

            playerUI.SetPlayerController(GetComponent<PlayerController>());
            playerUI.SetPlayer(GetComponent<Player>());
            GetComponent<Player>().Setup();
            
        }

    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
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
        Destroy(playerUIInstance);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);

            GameManager.UnregisterPlayer(transform.name);
        }       
    }
}
