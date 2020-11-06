
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerTelepoter : UdonSharpBehaviour
{
    int playerID;

    [SerializeField]
    Text playerName;

    bool isEnabledNode = false;

    void Start()
    {
        
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
                Debug.LogError("TargetPlayer not found");
            }
        }
    }

    public void Teleport()
    {
        var targetPlayer = VRCPlayerApi.GetPlayerById(playerID);

        if(targetPlayer != null)
        {
            Networking.LocalPlayer.TeleportTo(targetPlayer.GetPosition(), targetPlayer.GetRotation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool isManagedPlayer(VRCPlayerApi player)
    {
        if (player == null) return false;
        return player.playerId == playerID;
    }

    public bool IsEnabled()
    {
        return isEnabledNode;
    }

    public void SetEnabled(bool flag)
    {
        isEnabledNode = flag;
        gameObject.SetActive(flag);

        if(flag)
        {
            transform.SetAsLastSibling();
        }
        else
        {
            SetPlayerID(0);
        }
    }

}
