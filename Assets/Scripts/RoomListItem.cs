using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;

    private MatchInfoSnapshot matchInfo;

    public void Setup(MatchInfoSnapshot match, JoinRoomDelegate _joinRoomCallback)
    {
        matchInfo = match;
        joinRoomCallback = _joinRoomCallback;
        roomNameText.text = string.Format("{0} ({1}/{2})", match.name, match.currentSize, match.maxSize);
    }

    public void JoinGame()
    {
        joinRoomCallback.Invoke(matchInfo);
    }
}
