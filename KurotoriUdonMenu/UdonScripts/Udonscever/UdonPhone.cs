
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonPhone : UdonSharpBehaviour
{
    UdonCom[] coms;

    public GameObject comPrefab;

    const int MAX_PLAYER = 80;

    [HideInInspector]
    public int receiveHandlerValue;
    [SerializeField]
    private UdonBehaviour udonComm = null;


    [SerializeField]
    private Slider voiceVolumeSlider = null;

    [SerializeField]
    UdonLogSystem logSystem = null;

    void Start()
    {
        AddLog("Test");

        coms = new UdonCom[80];

        for (int i = 0; i < coms.Length; ++i)
        {
            var com = VRCInstantiate(comPrefab);
            var comComponent = com.GetComponent<UdonCom>();
            com.transform.localPosition = comPrefab.transform.localPosition;
            com.transform.localRotation = comPrefab.transform.localRotation;
            com.transform.localScale = comPrefab.transform.localScale;

            com.transform.SetParent(comPrefab.transform.parent, false);

            if (comComponent != null)
            {
                comComponent.phone = this;
                coms[i] = comComponent;
            }
        }
    }

    public void OnVoiceVolumeChange()
    {
        SetVolumeConnectPlayer();
    }

    void SetVolumeConnectPlayer()
    {
        for(int i = 0; i < coms.Length; ++i)
        {
            if (coms[i] == null) continue;

            if(coms[i].isConnect)
            {
                var player = VRCPlayerApi.GetPlayerById(coms[i].GetPlayerID());
                if (player != null)
                {
                    SetPlayerVoiceRangeMax(player);
                }
                else
                {
                    Debug.LogError("VolumeChange player not found");
                }
            }
        }
    }

    private void AddLog(string log)
    {
        if(logSystem != null)
        {
            logSystem.AddLog(log);
        }
    }

    public void receiveHandler()
    {
        bool isConnect = (receiveHandlerValue & 0x10000) == 0x10000;

        int fromID = (receiveHandlerValue & 0x0FF00) >> 8;
        int toID = (receiveHandlerValue & 0x000FF);

        if (isConnect)
        {
            Debug.LogError("Connect from " + fromID + " to " + toID);

            ConnectFromTo(fromID, toID);
        }
        else
        {
            Debug.LogError("Disconnect from " + fromID + " to " + toID);
            DisconnectFromTo(fromID, toID);
        }
    }

    private void ActivateCom(int id)
    {
        for(int i = 0; i < coms.Length; ++i)
        {
            if(!coms[i].IsEnable())
            {
                coms[i].SetEnable(true);
                coms[i].SetPlayerID(id);
                return;
            }
        }
    }

    private void AddComList(UdonCom com)
    {
        for (int i = 0; i < coms.Length; ++i)
        {
            if (coms[i] == null)
            {
                com.comID = i;
                coms[i] = com;

                AddLog(string.Format("Add UdonCom Complete. comID:{0} playerId:{1}", coms[i].comID, coms[i].GetPlayerID()));
                return;
            }
        }

        AddLog(string.Format("Faild add UdonCom. playerID:{0}", com.GetPlayerID()));
    }

    private UdonCom GetPlayerCom(VRCPlayerApi player)
    {
        for (int i = 0; i < coms.Length; ++i)
        {
            if (coms[i].isManagedPlayer(player))
            {
                AddLog("GetPlayerCom ComID:" + i + " player:" + player.playerId);
                return coms[i];
            }
        }

        return null;
    }

    private void RemoveCom(UdonCom com)
    {
        for (int i = 0; i < coms.Length; ++i)
        {
            if (coms[i] != null && coms[i].Equals(com))
            {
                AddLog(string.Format("UdonCom Destroy. comID:{0} playerID:{1}", i, com.GetPlayerID()));

                coms[i].SetEnable(false);

                return;
            }
        }

        Debug.LogError("RemoveCom Faild : " + com.GetPlayerID());
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        Debug.LogError("PlayerJoin:" + player.playerId);

        if (player != Networking.LocalPlayer)
        {
            ActivateCom(player.playerId);
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        Debug.Log("PlayerLeft:" + player.playerId);
        var com = GetPlayerCom(player);

        if (com != null)
        {
            RemoveCom(com);
        }
        else
        {
            Debug.LogError(player.playerId + " : " + "Com not found");
        }
    }

    void SetConnect(VRCPlayerApi player, bool connect)
    {
        Debug.LogError("SetConnect "  + player.playerId + ":" + connect.ToString());

        AddLog(string.Format("SetConnect {0}:{1} : {2}", player.displayName, player.playerId, connect.ToString()));

        var com = GetPlayerCom(player);
        if (com != null)
        {
            com.SetIsConnect(connect);
        }
        else
        {
            Debug.LogError(player.playerId + ": Com not found");
        }

        AddLog(string.Format("SetConnect Complete. {0}:{1}", player.displayName, player.playerId));
    }

    void SendConnectEvent(int fromID, int toID, bool isConnect)
    {
        int code = (fromID << 8) + toID;

        string connectFlag = isConnect ? "s" : "f";

        Debug.LogError("Connect: " + fromID + " -> " + toID);
        AddLog("Connect: " + fromID + "->" + toID);
        udonComm.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, connectFlag + code.ToString());
    }

    public void Connect(int id)
    {
        int fromID = Networking.LocalPlayer.playerId;
        int toID = id;
        
        SendConnectEvent(fromID, toID, true);
        
        // 対象のプレイヤーを接続状態にする。
        var player = VRCPlayerApi.GetPlayerById(id);

        if (player != null)
        {
            SetPlayerVoiceRangeMax(player);
            SetConnect(player, true);
        }else
        {
            Debug.LogError("Connect" + id + " is not found");
        }
    }

    public void Disconnect(int id)
    {
        int fromID = Networking.LocalPlayer.playerId;
        int toID = id;

        SendConnectEvent(fromID, toID, false);
        
        var player = VRCPlayerApi.GetPlayerById(id);
        if (player != null)
        {
            ResetPlayerVoiceDefault(player);
            SetConnect(player, false);
        }
        else
        {
            Debug.LogError("Disconnect" + id + " is not found");
        }
    }

    private void SetPlayerVoiceRangeMax(VRCPlayerApi fromPlayer)
    {
        // とりあえず最大距離に設定
        var distance = 1000000.0f;

        // そのプレイヤーの声を聞こえるようにする。
        fromPlayer.SetVoiceDistanceFar(distance + 3.0f);
        fromPlayer.SetVoiceDistanceNear(distance - 3.0f);

        fromPlayer.SetVoiceGain(voiceVolumeSlider.value);
        Debug.LogError("SetVolume:" + voiceVolumeSlider.value);
    }

    private void ResetPlayerVoiceDefault(VRCPlayerApi fromPlayer)
    {
        fromPlayer.SetVoiceDistanceFar(25);
        fromPlayer.SetVoiceDistanceNear(0);
        fromPlayer.SetVoiceGain(15.0f);
    }

    public void ConnectFromTo(int from, int to)
    {
        var localPlayer = Networking.LocalPlayer;

        if (localPlayer.playerId == to)
        {
            AddLog("Connect Accept. " + from + " -> " + to);
            // 電話の受け手だったら
            var fromPlayer = VRCPlayerApi.GetPlayerById(from);
            if (fromPlayer != null)
            {
                SetPlayerVoiceRangeMax(fromPlayer);
                SetConnect(fromPlayer, true);

                AddLog("Connect Complete. from:" + fromPlayer.playerId + ":" + from);
            }
        }
        else
        {

        }
    }

    public void DisconnectFromTo(int from, int to)
    {
        var localPlayer = Networking.LocalPlayer;

        if (localPlayer.playerId == to)
        {
            // 電話の受け手だったら
            var fromPlayer = VRCPlayerApi.GetPlayerById(from);
            if (fromPlayer != null)
            {
                ResetPlayerVoiceDefault(fromPlayer);
                SetConnect(fromPlayer, false);
            }
        }
        else
        {
        }
    }

    public void AllOn()
    {
        for(int i = 0; i < coms.Length; ++i)
        {
            if(coms[i].IsEnable() && !coms[i].isConnect)
            {
                var id = coms[i].GetPlayerID();

                Connect(id);
            }
        }
    }

    public void AllOff()
    {
        for (int i = 0; i < coms.Length; ++i)
        {
            if (coms[i].IsEnable() && coms[i].isConnect)
            {
                var id = coms[i].GetPlayerID();

                Disconnect(id);
            }
        }
    }
}
