
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Kurotori
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class UdonMenu : UdonSharpBehaviour
    {
        [SerializeField] Transform playerScale;
        [SerializeField] Transform tabButtonParent;
        [SerializeField] Animator openAnim;

        [Header("Prefabs")]
        [SerializeField] GameObject tabButton;
        
        [Header("MenuItem")]
        [SerializeField] GameObject[] menuItems;

        [Header("Setting")]
        [SerializeField] float menuPopTime = 2.0f;

        int activeMenuItem = 0;
        Kurotori.MenuActivater[] activaters;

        float menuPopTimer = 0.0f;

        void Start()
        {
            var itemNum = menuItems.Length;

            activaters = new Kurotori.MenuActivater[itemNum];

            for(int i = 0; i < itemNum;++i)
            {
                var tb = VRCInstantiate(tabButton);
                tb.transform.parent = tabButtonParent;

                tb.transform.localPosition = Vector3.zero;
                tb.transform.localRotation = Quaternion.identity;
                tb.transform.localScale = Vector3.one;

                tb.SetActive(true);

                var activater = tb.GetComponent<Kurotori.MenuActivater>();
                
                activaters[i] = activater;

                activater.udonMenu = this;
                activater.menuId = i;
                activater.SetLabel(menuItems[i].name);

                if (i == activeMenuItem)
                {
                    menuItems[i].SetActive(true);
                    activater.SetActiveButton(true);
                }
                else
                {
                    menuItems[i].SetActive(false);
                    activater.SetActiveButton(false);
                }

                
            }
        }

        /// <summary>
        /// メニューのスケール調整
        /// </summary>
        void UpdateMenuSize()
        {
            var scale = playerScale.localScale;

            scale.x = 1.0f / scale.x;
            scale.y = 1.0f / scale.y;
            scale.z = 1.0f / scale.z;

            transform.localScale = scale;
        }

        private void Update()
        {
            UpdateMenuSize();

            UpdatePopMenu();
        }

        /// <summary>
        /// 指定したIDのメニューを開き、それ以外を閉じます
        /// </summary>
        /// <param name="id"></param>
        public void SetActiveMenuItem(int id)
        {
            for(int i = 0; i < activaters.Length; ++i)
            {
                if(i == id)
                {
                    menuItems[i].SetActive(true);
                    activaters[i].SetActiveButton(true);
                }
                else
                {
                    menuItems[i].SetActive(false);
                    activaters[i].SetActiveButton(false);
                }
            }
        }

        /// <summary>
        /// メニューの呼び出し処理
        /// </summary>
        public void UpdatePopMenu()
        {
            if (!Utilities.IsValid(Networking.LocalPlayer)) return;

            if(Networking.LocalPlayer.IsUserInVR())
            {
                // VRModeでの呼び出し

                // 両手トリガー長押し
                if(Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.9f && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.9f)
                {
                    menuPopTimer += Time.deltaTime;
                }
                else
                {
                    menuPopTimer = 0.0f;
                }

                
            }
            else
            {
                if(Input.GetKey(KeyCode.M))
                {
                    menuPopTimer += Time.deltaTime;
                }
                else
                {
                    menuPopTimer = 0.0f;
                }
            }

            if (menuPopTimer > menuPopTime)
            {
                PopMenu();
            }
            else
            {
                OpenAnimation();
            }
        }

        void PopMenu()
        {
            if (Networking.LocalPlayer.IsUserInVR())
            {
                var headTrackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                var pos = Networking.LocalPlayer.GetPosition();
                var rot = Networking.LocalPlayer.GetRotation();

                transform.position = pos + Vector3.up * transform.localScale.x + rot * Vector3.forward * transform.localScale.x;

                transform.LookAt(headTrackingData.position, Vector3.up);
            }
            else
            {
                var headTrackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

                transform.position = headTrackingData.position + headTrackingData.rotation * Vector3.forward * transform.localScale.x;

                transform.LookAt(headTrackingData.position, Vector3.up);
            }
        }

        void OpenAnimation()
        {
            if (Networking.LocalPlayer.IsUserInVR())
            {
                var headTrackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                var pos = Networking.LocalPlayer.GetPosition();
                var rot = Networking.LocalPlayer.GetRotation();

                openAnim.transform.position = pos + Vector3.up * transform.localScale.x + rot * Vector3.forward * transform.localScale.x;

                openAnim.transform.LookAt(headTrackingData.position, Vector3.up);
            }
            else
            {
                var headTrackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

                openAnim.transform.position = headTrackingData.position + headTrackingData.rotation * Vector3.forward * transform.localScale.x;
                openAnim.transform.LookAt(headTrackingData.position, Vector3.up);
            }
            openAnim.SetFloat("progress", menuPopTimer / menuPopTime);
        }
    }
}