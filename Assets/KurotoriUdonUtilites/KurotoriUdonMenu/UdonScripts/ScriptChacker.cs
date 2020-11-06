
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ScriptChacker : UdonSharpBehaviour
{
    public UdonBehaviour target;

    void Start()
    {
        
    }

    private void Update()
    {
        if(target == null)
        {
            Debug.Log("Detect Null");
        }
    }
}
