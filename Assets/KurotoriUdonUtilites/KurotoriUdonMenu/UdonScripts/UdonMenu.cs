
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonMenu : UdonSharpBehaviour
{
    public GameObject[] menuItems;

    public GameObject TabMenuPrefab;

    private MenuActivater[] menuActivaterList;

    void Start()
    {
        menuActivaterList = new MenuActivater[menuItems.Length];

        for(int i = 0; i < menuItems.Length;++i)
        {
            var activateButton = VRCInstantiate(TabMenuPrefab);


            activateButton.transform.parent = TabMenuPrefab.transform.parent;
            activateButton.transform.localPosition = TabMenuPrefab.transform.localPosition;
            activateButton.transform.localRotation = TabMenuPrefab.transform.localRotation;
            activateButton.transform.localScale = TabMenuPrefab.transform.localScale;

            var button = activateButton.GetComponentInChildren<Button>();
            var buttonText = activateButton.GetComponentInChildren<Text>();
            var menuActivater = activateButton.GetComponent<MenuActivater>();
            
            buttonText.text = menuItems[i].name;

            if (menuActivater != null)
            {
                menuActivater.udonMenu = this;
                menuActivater.targetObject = menuItems[i];
                menuActivater.id = i;
            
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
