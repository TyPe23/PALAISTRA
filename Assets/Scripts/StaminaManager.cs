using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public Slider slider;
    public float stamina { get; private set; }

    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        stamina = stats.maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (stamina < 100)
        {
            stamina += stats.staminaRecovery;
        }

        slider.value = stamina;
    }

    public void spendStamina(float cost)
    {
        stamina -= cost;
        
        if (stamina < 0)
        {
            stamina = 0;
        }
    }
}
