
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UdonMenuPositionResetter : UdonSharpBehaviour
{
    [SerializeField]
    Transform menu = null;

    Vector3 initPosition;
    Quaternion initRotation;

    void Start()
    {
        initPosition = menu.position;
        initRotation = menu.rotation;
    }

    public void ResetPos()
    {
        menu.position = initPosition;
        menu.rotation = initRotation;
    }
}
