using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {


    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    Text status;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    Transform roomListParent;

    private NetworkManager networkManager;

	// Use this for initialization
	void Start () {
        networkManager = NetworkManager.singleton;

        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
	}
	
	public void RefreshRoomList()
    {
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);

        status.text = "Refreshing ...";
        ClearRoomList();
    }

    private void ClearRoomList()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        if(matchList == null)
        {
            status.text = "Unable to find rooms.";
            return;
        }       

        foreach(MatchInfoSnapshot matchInfo in matchList)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem listItem = (RoomListItem)roomListItemGO.GetComponent<RoomListItem>();

            if(listItem != null)
            {
                listItem.Setup(matchInfo, JoinRoom);
            }

            roomList.Add(roomListItemGO);
        }

        if(roomList.Count == 0)
        {
            status.text = "No room available.";
        }
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();

        status.text = "Joining game ...";
    }

}
