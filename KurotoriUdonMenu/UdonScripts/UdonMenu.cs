
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonMenu : UdonSharpBehaviour
{
    public GameObject[] menuItems = null;

    [Space(10)]

    [SerializeField]
    string[] menuNames_JP = null;
    [SerializeField]
    string[] menuNames_EN = null;
    
    [Space(10)]

    [SerializeField]
    string[] moderatorList = null;
    [SerializeField]
    GameObject[] moderatorItems = null;

    [Space(10)]

    public GameObject TabMenuPrefab = null;

    private MenuActivater[] menuActivaterList = null;

    void Start()
    {
        menuActivaterList = new MenuActivater[menuItems.Length];

        for(int i = 0; i < menuItems.Length;++i)
        {
            bool isModelatorItem = false;
            foreach(var modeletorItem in moderatorItems)
            {
                if(menuItems[i].Equals(modeletorItem))
                {
                    isModelatorItem = true;
                    break;
                }
            }

            var activateButton = VRCInstantiate(TabMenuPrefab);


            activateButton.transform.parent = TabMenuPrefab.transform.parent;
            activateButton.transform.localPosition = TabMenuPrefab.transform.localPosition;
            activateButton.transform.localRotation = TabMenuPrefab.transform.localRotation;
            activateButton.transform.localScale = TabMenuPrefab.transform.localScale;

            var button = activateButton.GetComponentInChildren<Button>();
            var buttonText = activateButton.GetComponentInChildren<Text>();
            var menuActivater = activateButton.GetComponent<MenuActivater>();

            buttonText.text = menuNames_JP[i];

            if (menuActivater != null)
            {
                menuActivater.udonMenu = this;
                menuActivater.targetObject = menuItems[i];
                menuActivater.id = i;
                menuActivater.isModelatorItem = isModelatorItem;
            
                activateButton.SetActive(true);

                if(i == 0)
                {
                    menuActivater.Activate();
                }
                else
                {
                    menuActivater.SetDisable();
                }

                menuActivaterList[i] = menuActivater;
            }
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        // モデレーターでない場合は、モデレーター用メニューボタンを非アクティブにする。
        if(player.Equals(Networking.LocalPlayer))
        {
            bool isModeletor = IsModeletor(player);

            if (!isModeletor)
            {
                foreach (var menuActivater in menuActivaterList)
                {
                    if (menuActivater.isModelatorItem)
                    {
                        menuActivater.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private bool IsModeletor(VRCPlayerApi player)
    {
        foreach(var modeletorName in moderatorList)
        {
            if (modeletorName.Equals(player.displayName))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 外部呼出し用
    /// </summary>
    public void SetJapanese()
    {
        for (int i = 0; i < menuActivaterList.Length; ++i)
        {
            var buttonText = menuActivaterList[i].gameObject.GetComponentInChildren<Text>();
            buttonText.text = menuNames_JP[i];
        }
    }

    /// <summary>
    /// 外部呼出し用
    /// </summary>
    public void SetEnglish()
    {
        for (int i = 0; i < menuActivaterList.Length; ++i)
        {
            var buttonText = menuActivaterList[i].gameObject.GetComponentInChildren<Text>();
            buttonText.text = menuNames_EN[i];
        }
    }

    public void SetDisableOther(int id)
    {
        for(int i = 0; i < menuActivaterList.Length; ++i)
        {
            if(i != id)
            {
                menuActivaterList[i].SetDisable();
            }
        }
    }

    void Update()
    {
        var localPlayer = Networking.LocalPlayer;

        if (localPlayer != null)
        {
            if (Input.GetKey(KeyCode.M) || 
                (Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.9f && Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.9f)
               )
            {
                var head = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

                transform.position = head.position + head.rotation * Vector3.forward;

                transform.LookAt(head.position, Vector3.up);
            }
        }
    }
}
