
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

namespace Kurotori.UdonMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class MenuActivater : UdonSharpBehaviour
    {
        public UdonMenu udonMenu;
        public int menuId;

        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Button button;

        public void OnClick()
        {
            udonMenu.SetActiveMenuItem(menuId);
        }

        public void SetActiveButton(bool flag)
        {
            button.interactable = !flag;
        }

        public void SetLabel(string name)
        {
            if(label != null)
            {
                label.text = name;
            }
        }

    }
}
