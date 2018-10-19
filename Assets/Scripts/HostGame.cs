using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {
    [SerializeField]
    private uint roomSize = 6;

    [SerializeField]
    private GameObject lobbyUI;

    private string roomName = "Default";

    private NetworkManager networkManager;

    public void Start()
    {
        networkManager = NetworkManager.singleton;
        lobbyUI.SetActive(false);
        networkManager.matchMaker = null;
        /*if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }*/
    }


    public void SetRoomSize(uint size)
    {
        roomSize = size;
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void  CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Création de la partie " + roomName + "- " + roomSize + " slots");

            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
