using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public Slider slider;
    public float stamina { get; private set; }

    private PlayerStats stats;
    private MomentumManager momentum;

    // Start is called before the first frame update
    void Start()
    {
        momentum = GetComponent<MomentumManager>();
        stats = GetComponent<PlayerStats>();
        stamina = stats.maxStamina;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stamina < 100)
        {
            stamina += momentum.recovery;
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
