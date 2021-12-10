
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestException : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    public override void Interact()
    {
        GameObject gameObject = null;

        var test = gameObject.transform;
    }
}
