using UnityEngine.Networking.Match;
using UnityEngine;
using UnityEngine.UI;
public class RoomListItem : MonoBehaviour
{

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;
    public static int matchSize;
    private MatchInfoSnapshot match;
    
    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        match = _match;
        joinRoomCallback = _joinRoomCallback;
        roomNameText.text = match.name + " ("+match.currentSize + "/" + match.maxSize + ")";
        SetMatchSize(match.currentSize);
    }

    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
    }

    public static void SetMatchSize(int size)
    {
        matchSize = size;
    }
    
    public static int GetMatchSize()
    {
        return matchSize;
    }
}
