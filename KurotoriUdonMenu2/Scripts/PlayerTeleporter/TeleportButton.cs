
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

namespace Kurotori.UdonMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TeleportButton : UdonSharpBehaviour
    {
        [SerializeField] public int playerID;
        [SerializeField] public string playerName;

        [SerializeField] public TMPro.TextMeshProUGUI displayName;

        void Start()
        {
            
        }

        public void SetupUI()
        {
            displayName.text = playerName;
        }

        /// <summary>
        /// プレイヤーをターゲットにテレポートさせる。
        /// </summary>
        public void Teleport()
        {
            var targetPlayer = VRCPlayerApi.GetPlayerById(playerID);

            if(Utilities.IsValid(targetPlayer))
            {
                var targetPos = targetPlayer.GetPosition();
                var targetRot = targetPlayer.GetRotation();

                Networking.LocalPlayer.TeleportTo(targetPos, targetRot);
            }
        }

        /// <summary>
        /// uGUIボタンで呼ばれる関数
        /// </summary>
        public void OnButtonDown()
        {
            Teleport();
        }

        
    }
}