using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public float animationSpeed = 0.5f; // Duration of the animation

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        // Animate the slider value change
        LeanTween.value(gameObject, slider.value, health, animationSpeed).setOnUpdate((float val) =>
        {
            slider.value = val;
        });
    }
}

