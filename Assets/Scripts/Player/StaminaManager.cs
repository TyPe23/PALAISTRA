using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public float stamina { get; private set; }

    private PlayerStats stats;
    private MomentumManager momentum;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        momentum = GetComponent<MomentumManager>();
        stats = GetComponent<PlayerStats>();
        stamina = stats.maxStamina;
        color = fill.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stamina < 100)
        {
            stamina += momentum.recovery;

            if (momentum.recovery == stats.exhaustedRecovery)
            {
                fill.color = new Color(color.r, color.g, color.b, Mathf.Sin(Time.time * 20));
            }
            else
            {
                fill.color = color;
            }
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
