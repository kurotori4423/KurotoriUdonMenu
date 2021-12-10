
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Kurotori
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TeleportButtonManager : UdonSharpBehaviour
    {
        [SerializeField] int maxPlayerNum = 80;

        [SerializeField] GameObject ButtonPrefab;
        [SerializeField] Transform buttonParent;

        TeleportButton[] teleportButtons;

        void Start()
        {
            // ボタンオブジェクトを生成
            GenerateTeleportButtons();
        }

        /// <summary>
        /// ボタンオブジェクトを生成して非表示状態にする
        /// </summary>
        void GenerateTeleportButtons()
        {
            teleportButtons = new TeleportButton[80];

            for(int i = 0; i < maxPlayerNum; ++i)
            {
                var button = VRCInstantiate(ButtonPrefab);

                button.transform.parent = buttonParent;

                // ペアレントの設定時にTransformがおかしくなるので修正する。
                button.transform.localScale = Vector3.one;
                button.transform.localRotation = Quaternion.identity;
                button.transform.localPosition = Vector3.zero;

                var udon = button.GetComponent<TeleportButton>();

                teleportButtons[i] = udon;

                button.SetActive(false);
            }
        }

        void UpdateTeleportButton()
        {
            VRCPlayerApi[] playerList = new VRCPlayerApi[maxPlayerNum];
            VRCPlayerApi.GetPlayers(playerList);

            for(int i = 0; i < maxPlayerNum; ++i)
            {
                if(Utilities.IsValid(playerList[i]) && !playerList[i].Equals(Networking.LocalPlayer))
                {
                    teleportButtons[i].gameObject.SetActive(true);
                    teleportButtons[i].playerID = playerList[i].playerId;
                    teleportButtons[i].playerName = playerList[i].displayName;
                    teleportButtons[i].SetupUI();
                }
                else
                {
                    teleportButtons[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// プレイヤー退室時、呼ばれた段階ではVRCPlayerApiは生きているのでそれを弾くように。
        /// </summary>
        /// <param name="leftPlayer"></param>
        void UpdateTeleportButtonOnPlayerLeft(VRCPlayerApi leftPlayer)
        {
            VRCPlayerApi[] playerList = new VRCPlayerApi[maxPlayerNum];
            VRCPlayerApi.GetPlayers(playerList);

            for (int i = 0; i < maxPlayerNum; ++i)
            {
                if (Utilities.IsValid(playerList[i]) && !playerList[i].Equals(Networking.LocalPlayer) && !playerList[i].Equals(leftPlayer))
                {
                    teleportButtons[i].gameObject.SetActive(true);
                    teleportButtons[i].playerID = playerList[i].playerId;
                    teleportButtons[i].playerName = playerList[i].displayName;
                    teleportButtons[i].SetupUI();
                }
                else
                {
                    teleportButtons[i].gameObject.SetActive(false);
                }
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            UpdateTeleportButton();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            UpdateTeleportButtonOnPlayerLeft(player);
        }
    }
}