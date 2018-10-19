using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

    public static bool isON = false;
    private NetworkManager networkManager;

    public void Start()
    {
        networkManager = NetworkManager.singleton;
    }
    public void LeaveRoom()
    {
        if(networkManager.matchMaker == null)
        {
            networkManager.StopHost();
            networkManager.matchMaker = null;
            return;
        }
        MatchInfo matchInfo = networkManager.matchInfo;

        networkManager.StopHost();
        
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
       

    }
}
