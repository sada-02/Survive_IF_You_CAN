using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stamina_sys : MonoBehaviour
{   
    // refrences to stamina bar
    public Slider staminaBar;
    public Gradient gradient;
    public Image fill;
    
    public void Setstamina(float stamina)
    {
        staminaBar.value = stamina;
        fill.color = gradient.Evaluate(staminaBar.normalizedValue);
    }

    public void SetMaxStamina(float stamina)
    {
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;

        fill.color = gradient.Evaluate(1f);
    }

}
