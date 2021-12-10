
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonCom : UdonSharpBehaviour
{
    int playerID = 0;
    public int comID;

    bool isEnableNode = false;

    public bool isConnect = false;

    public UdonLogSystem logSystem = null;

    [SerializeField]
    Text playerName = null;

    public UdonPhone phone = null;

    void Start()
    {
        
    }

    private void AddLog(string log)
    {
        if (logSystem != null)
        {
            logSystem.AddLog(log);
        }
    }

    public bool isManagedPlayer(VRCPlayerApi player)
    {
        if (player == null) return false;
        return player.playerId == playerID;
    }

    public void SetPlayerID(int id)
    {
        playerID = id;


        if (Networking.LocalPlayer == null)
        {
            playerName.text = "None Player" + playerID.ToString();
        }
        else
        {
            var targetPlayer = VRCPlayerApi.GetPlayerById(playerID);
            if (targetPlayer != null)
            {
                playerName.text = string.Format("{0} : {1}", targetPlayer.playerId, targetPlayer.displayName);
            }
            else
            {
                Debug.LogError("TargetPlayer Notfound");
            }
        }
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public void OnClick()
    {
        if (isConnect)
        {
            // 接続状態なら
            phone.Disconnect(playerID);
        }
        else
        {
            phone.Connect(playerID);
        }
    }

    public void SetIsConnect(bool connect)
    {
        AddLog(string.Format("SetIsConnect: {0} : {1}", playerID, connect));
        if(connect)
        {
            playerName.color = Color.red;
            isConnect = true;
        }else
        {
            playerName.color = Color.black;
            isConnect = false;
        }
    }

    public bool IsEnable()
    {
        return isEnableNode;
    }

    public void SetEnable(bool flag)
    {
        isEnableNode = flag;
        gameObject.SetActive(flag);
        
        if(flag)
        {
            transform.SetAsLastSibling();
        }
        else
        {
            SetIsConnect(false);
            SetPlayerID(0);
        }
    }

}
