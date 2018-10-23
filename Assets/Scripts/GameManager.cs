using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il ne doit y avoir qu'un GameMAnager");
        }

        instance = this;
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);
    }

#region PLayer Traking
    private const string PLAYER_ID_PREFIX = "Player "; 

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static Dictionary<string, PlayerScore> playerScores = new Dictionary<string, PlayerScore>();
    public static Dictionary<string, PlayerScore> GetPlayerScores()
    {
        return playerScores;
    }

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;

        players.Add(playerID, player);
        player.transform.name = playerID;
        if(!playerScores.ContainsKey(playerID))
            playerScores.Add(playerID, new PlayerScore());
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static PlayerScore GetPlayerScore(string playerID)
    {
        return playerScores[playerID];
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));

        GUILayout.BeginVertical();

        foreach(string playerID in players.Keys)
        {
            GUILayout.Label(playerID + "-" + players[playerID].transform.name);
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
#endregion
}
