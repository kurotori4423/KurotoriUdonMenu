
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// Animatorの再生位置をスライダーで指定します。
/// </summary>
public class UdonMenuAnimatorSlider : UdonSharpBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] public Animator animator;
    [SerializeField] public float initValue;


    void Start()
    {
        animator.speed = 0.0f;
        animator.SetFloat("blend", initValue);
        slider.SetValueWithoutNotify(initValue);
    }

    public void OnValueChange()
    {
        animator.SetFloat("blend", slider.value);
    }
}
