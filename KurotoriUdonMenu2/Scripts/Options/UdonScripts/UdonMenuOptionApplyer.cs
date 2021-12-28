
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Kurotori.UdonMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class UdonMenuOptionApplyer : UdonSharpBehaviour
    {
        [SerializeField] public GameObject[] options;

        void Start()
        {
            foreach(var optionObject in options)
            {
                var optionUdon = (UdonBehaviour)optionObject.GetComponent(typeof(UdonBehaviour));

                if(Utilities.IsValid(optionUdon))
                {
                    optionUdon.SendCustomEvent("FirstSetup");
                }
            }
        }
    }
}