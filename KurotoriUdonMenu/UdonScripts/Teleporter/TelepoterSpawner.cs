
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TelepoterSpawner : UdonSharpBehaviour
{
    public GameObject telepoterPrefab;
    public Transform context;

    PlayerTelepoter[] playerTelepoters;

    void Start()
    {
        playerTelepoters = new PlayerTelepoter[80];

        for (int i = 0; i < playerTelepoters.Length; ++i)
        {
            var telepoter = VRCInstantiate(telepoterPrefab);
            var telepoterComponent = telepoter.GetComponent<PlayerTelepoter>();

            telepoter.transform.localPosition = telepoterPrefab.transform.localPosition;
            telepoter.transform.localRotation = telepoterPrefab.transform.localRotation;
            telepoter.transform.localScale = telepoterPrefab.transform.localScale;

            telepoter.transform.SetParent(context, false);

            playerTelepoters[i] = telepoterComponent;
        }
    }

    private void AddPlayerTelepoterList(PlayerTelepoter telepoter)
    {
        for(int i = 0; i < playerTelepoters.Length; ++i)
        {
            if(playerTelepoters[i] == null)
            {
                playerTelepoters[i] = telepoter;
                break;
            }
        }
    }

    private PlayerTelepoter GetPlayerTelepoter(VRCPlayerApi player)
    {
        for(int i = 0; i < playerTelepoters.Length; ++i)
        {
            if(playerTelepoters[i].isManagedPlayer(player))
            {
                return playerTelepoters[i];
            }
        }

        return null;
    }

    private void RemovePlayerTelepoter(PlayerTelepoter telepoter)
    {
        for(int i = 0; i < playerTelepoters.Length; ++i)
        {
            if(playerTelepoters[i] != null && playerTelepoters[i].Equals(telepoter))
            {
                Debug.LogError("Destroy");
                //Destroy(playerTelepoters[i].gameObject);
                //Debug.LogError("After Destroy");
                //playerTelepoters[i] = null;

                playerTelepoters[i].SetEnabled(false);

            }
        }
    }

    private void InstantiateTelepoter(int id)
    {
        var telepoter = VRCInstantiate(telepoterPrefab);
        telepoter.SetActive(true);
        var telepoterComponent = telepoter.GetComponent<PlayerTelepoter>();
        telepoter.transform.parent = context;
        telepoter.transform.localPosition = telepoterPrefab.transform.localPosition;
        telepoter.transform.localRotation = telepoterPrefab.transform.localRotation;
        telepoter.transform.localScale = telepoterPrefab.transform.localScale;
        telepoterComponent.SetPlayerID(id);

        AddPlayerTelepoterList(telepoterComponent);
    }

    private void ActivateTelepoter(VRCPlayerApi player)
    {
        for(int i = 0; i < playerTelepoters.Length; ++i)
        {
            if (playerTelepoters[i] != null && !playerTelepoters[i].IsEnabled())
            {
                playerTelepoters[i].SetPlayerID(player.playerId);
                playerTelepoters[i].SetEnabled(true);
                return;
            }
        }
    }

    public override void Interact()
    {
        InstantiateTelepoter(0);
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        
        if (player != Networking.LocalPlayer)
        {
            //InstantiateTelepoter(player.playerId);
            ActivateTelepoter(player);
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        var telepoter = GetPlayerTelepoter(player);

        if(telepoter != null)
        {
            RemovePlayerTelepoter(telepoter);
        }
    }
}
