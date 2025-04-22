using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [Range(0,1f)]
    public float SliderValue;
    public bool CurrentValue;
    protected Slider slider;

    [Header("Animation")]
    [Range(0,1f)]
    public float AnimationDuration = 0.5f;
    public AnimationCurve SlideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Events")]
    public UnityEvent ToggleOn;
    public UnityEvent ToggleOff;

    protected Coroutine animationSliderCoroutine;

    protected ToggleSwitchGroupManager toggleSwitchGroupManager;

    protected Action transitionEffect;

    protected void OnValidate()
    {
        SetupToggleComponents();

        slider.value = SliderValue;
    }

    protected virtual void Awake()
    {
        SetupToggleComponents();
    }

    protected virtual void SetupToggleComponents()
    {
        if (slider != null) return;

        SetupSliderComponent();
    }

    protected virtual void SetupSliderComponent()
    {
        slider = GetComponent<Slider>();

        if (slider == null) return;

        slider.interactable = false;
        var slideColor = slider.colors;
        slideColor.disabledColor = Color.white;
        slider.colors = slideColor;
        slider.transition = Selectable.Transition.None;
    }

    public virtual void SetupForToggleManager(ToggleSwitchGroupManager manager)
    {
        toggleSwitchGroupManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public virtual void Toggle()
    {
        if (toggleSwitchGroupManager != null)
        {
            toggleSwitchGroupManager.ToggleGroup(this);
        }
        else
        {
            SetStateAndStartAnimation(!CurrentValue);
        }
    }

    public virtual void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    protected virtual void SetStateAndStartAnimation(bool valueToSetTo)
    {
        CurrentValue = valueToSetTo;

        if (CurrentValue)
            ToggleOn?.Invoke();
        else 
            ToggleOff?.Invoke();

        if (animationSliderCoroutine != null) StopCoroutine(animationSliderCoroutine);

        animationSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    protected virtual IEnumerator AnimateSlider()
    {
        var startValue = slider.value;
        var endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (AnimationDuration > 0)
        {
            while (time < AnimationDuration)
            {
                time += Time.deltaTime;

                var lerpFactor = SlideEase.Evaluate(time / AnimationDuration);
                slider.value = SliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                InvokeTransitionEffect();

                yield return null;
            }
        }

        slider.value = endValue;
    }

    protected virtual void InvokeTransitionEffect()
    {
        transitionEffect?.Invoke();
    }

}
