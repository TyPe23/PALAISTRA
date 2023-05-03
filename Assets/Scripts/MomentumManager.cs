using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MomentumManager : MonoBehaviour
{
    private PlayerStats stats;
    private CinemachineImpulseSource shake;

    public TMP_Text grade;
    public Slider slider;
    public float recovery;
    private float momentum;
    private float momentumDecay;

    // Start is called before the first frame update
    void Start()
    {
        grade.text = "";
        stats = GetComponent<PlayerStats>();
        shake = GetComponent<CinemachineImpulseSource>();

        recovery = stats.staminaRecovery;
        momentum = 0;
        momentumDecay = Time.fixedDeltaTime * 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (slider.value <= 0)
        {
            if (grade.text == "S")
            {
                grade.text = "A";
                momentum = 75;
                momentumDecay = Time.fixedDeltaTime * 25;
                recovery = stats.staminaRecovery * 1.75f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "A")
            {
                grade.text = "B";
                momentum = 75;
                momentumDecay = Time.fixedDeltaTime * 20;
                recovery = stats.staminaRecovery * 1.5f;
            }
            else if (grade.text == "B")
            {
                grade.text = "C";
                momentum = 75;
                momentumDecay = Time.fixedDeltaTime * 15;
                recovery = stats.staminaRecovery * 1.25f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "C")
            {
                grade.text = "";
                momentum = 75;
                momentumDecay = Time.fixedDeltaTime * 10;
                recovery = stats.staminaRecovery * 1.1f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "")
            {
                momentumDecay = Time.fixedDeltaTime * 5;
                recovery = stats.staminaRecovery * 1f;
            }
        }
        else
        {
            momentum -= momentumDecay;
        }

        slider.value = momentum;
    }

    public void addMomentum(float amount)
    {
        momentum += amount;

        if (momentum > 100)
        {
            if (grade.text == "S")
            {
                momentum = 100;
                momentumDecay = Time.fixedDeltaTime * 30;
                recovery = stats.staminaRecovery * 2f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "A")
            {
                grade.text = "S";
                momentum = 50;
                momentumDecay = Time.fixedDeltaTime * 25;
                recovery = stats.staminaRecovery * 1.75f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "B")
            {
                grade.text = "A";
                momentum = 50;
                momentumDecay = Time.fixedDeltaTime * 20;
                recovery = stats.staminaRecovery * 1.5f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "C")
            {
                grade.text = "B";
                momentum = 50;
                momentumDecay = Time.fixedDeltaTime * 15;
                recovery = stats.staminaRecovery * 1.25f;
                shake.GenerateImpulseWithForce(0.1f);
            }
            else if (grade.text == "")
            {
                grade.text = "C";
                momentum = 50;
                momentumDecay = Time.fixedDeltaTime * 10;
                recovery = stats.staminaRecovery * 1.1f;
                shake.GenerateImpulseWithForce(0.1f);
            }
        }
    }
}
