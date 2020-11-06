
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class MenuActivater : UdonSharpBehaviour
{ 
    public GameObject targetObject;
    public UdonMenu udonMenu;

    public Button button;
    public int id;

    void Start()
    {
    }

    public void Activate()
    {
        targetObject.SetActive(true);
        button.interactable = false;
    }

    public void SetDisable()
    {
        targetObject.SetActive(false);
        button.interactable = true;
    }

    public void OnClick()
    {
        Activate();
        udonMenu.SetDisableOther(id);
    }
}
