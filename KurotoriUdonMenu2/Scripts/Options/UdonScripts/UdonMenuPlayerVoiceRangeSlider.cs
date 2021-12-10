
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonMenuPlayerVoiceRangeSlider : UdonSharpBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] public float minVoiceRange;
    [SerializeField] public float maxVoiceRange;
    [SerializeField] public float initRange;

    void Start()
    {
        slider.minValue = minVoiceRange;
        slider.maxValue = maxVoiceRange;

        slider.SetValueWithoutNotify(initRange);
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        player.SetVoiceDistanceFar(slider.value);
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {

    }

    public void OnValueChange()
    {
        VRCPlayerApi[] players = new VRCPlayerApi[80];
        VRCPlayerApi.GetPlayers(players);

        foreach(var player in players)
        {
            if(Utilities.IsValid(player))
            {
                player.SetVoiceDistanceFar(slider.value);
            }
        }
    }


}
