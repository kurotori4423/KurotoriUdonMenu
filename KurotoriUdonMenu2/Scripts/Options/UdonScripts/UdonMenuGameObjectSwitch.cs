
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Kurotori.UdonMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class UdonMenuGameObjectSwitch : UdonSharpBehaviour
    {
        [SerializeField] public bool isOn;
        [SerializeField] public GameObject switchObject;
        [SerializeField] Toggle toggle;

        void Start()
        {
            FirstSetup();
        }

        public void FirstSetup()
        {
            switchObject.SetActive(isOn);
            toggle.SetIsOnWithoutNotify(isOn);
        }

        public void OnValueChanged()
        {
            Debug.Log("OnValueChanged");
            switchObject.SetActive(toggle.isOn);
        }
    }
}
