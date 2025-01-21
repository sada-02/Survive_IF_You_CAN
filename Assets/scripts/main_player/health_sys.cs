using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_sys : MonoBehaviour
{
    // refrences to health bar
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;
    public void SetHealth(float health)
    {
        healthBar.value = health;
        fill.color = gradient.Evaluate(healthBar.normalizedValue);
    }

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    
}
