using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hunger_sys : MonoBehaviour
{
    // refrences to hunger bar
    public Slider hungerBar;
    public Gradient gradient;
    public Image fill;

    public void Sethunger(float hunger)
    {
        hungerBar.value = hunger;
        fill.color = gradient.Evaluate(hungerBar.normalizedValue);
    }

    public void SetMaxHunger(float hunger)
    {
        hungerBar.maxValue = hunger;
        hungerBar.value = hunger;

        fill.color = gradient.Evaluate(1f);
    }

}
