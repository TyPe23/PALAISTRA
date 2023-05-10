using HighScore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // temp stats (reset at start of run)
    public float MoveSpeed;
    public float ExhaustedSpeed;
    public float DashSpeed;
    public float SpinMoveSpeed;
    public float LariatSpeed;
    public float PileDriverSpeed;

    public float DashDist;

    public int DashCost;
    public int SpinCost;
    public float SpinHoldCost;
    public int LariatCost;
    public int PileDriverCost;

    public int currency;

    public float maxStamina;
    public float staminaRecovery;
    public float exhaustedRecovery;
    public float maxHealth;
    public float currentHealth;

    public int roomCount;
    public int score;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Awake()
    {
        //if (PlayerPrefs.HasKey("MoveSpeed"))
        //{
            PlayerPrefs.SetFloat("MoveSpeed", 5.25f);
            PlayerPrefs.SetFloat("ExhaustedSpeed", PlayerPrefs.GetFloat("MoveSpeed") / 2);
            PlayerPrefs.SetFloat("SpinMoveSpeed", 0.1f);
            PlayerPrefs.SetFloat("DashSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 3);
            PlayerPrefs.SetFloat("LariatSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 2);
            PlayerPrefs.SetFloat("PileDriverSpeed", PlayerPrefs.GetFloat("MoveSpeed"));
            PlayerPrefs.SetFloat("DashDist", 3);
            PlayerPrefs.SetInt("SpinCost", 5);
            PlayerPrefs.SetInt("DashCost", 5);
            PlayerPrefs.SetInt("LariatCost", 10);
            PlayerPrefs.SetInt("PileDriverCost", 10);
            PlayerPrefs.SetInt("currency", 10);
            PlayerPrefs.SetFloat("maxStamina", 100);
            PlayerPrefs.SetFloat("maxHealth", 20);
            PlayerPrefs.SetFloat("currentHealth", 20);
            PlayerPrefs.SetFloat("staminaRecovery", Time.fixedDeltaTime * 4);
            PlayerPrefs.SetFloat("exhaustedRecovery", PlayerPrefs.GetFloat("staminaRecovery") * 4);
            PlayerPrefs.SetFloat("SpinHoldCost", PlayerPrefs.GetFloat("staminaRecovery") * 2);
            PlayerPrefs.SetInt("Score", 1000);
        //}

        SpinMoveSpeed = PlayerPrefs.GetFloat("SpinMoveSpeed");
        MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        ExhaustedSpeed = PlayerPrefs.GetFloat("ExhaustedSpeed");
        DashSpeed = PlayerPrefs.GetFloat("DashSpeed");
        LariatSpeed = PlayerPrefs.GetFloat("LariatSpeed");
        PileDriverSpeed = PlayerPrefs.GetFloat("PileDriverSpeed");
        DashDist = PlayerPrefs.GetFloat("DashDist");
        SpinCost = PlayerPrefs.GetInt("SpinCost");
        DashCost = PlayerPrefs.GetInt("DashCost");
        LariatCost = PlayerPrefs.GetInt("LariatCost");
        PileDriverCost = PlayerPrefs.GetInt("PileDriverCost");
        currency = PlayerPrefs.GetInt("currency");
        maxStamina = PlayerPrefs.GetFloat("maxStamina");
        maxHealth = PlayerPrefs.GetFloat("maxHealth");
        currentHealth = PlayerPrefs.GetFloat("currentHealth");
        staminaRecovery = PlayerPrefs.GetFloat("staminaRecovery");
        exhaustedRecovery = PlayerPrefs.GetFloat("exhaustedRecovery");
        SpinHoldCost = PlayerPrefs.GetFloat("SpinHoldCost");
        roomCount = PlayerPrefs.GetInt("roomCount");
    }

    void FixedUpdate()
    {
        healthSlider.value = currentHealth; 
        score = PlayerPrefs.GetInt("Score");
    }

    public void adjustScore(int scoreChange)
    {
        print(scoreChange);
        score += scoreChange;
        PlayerPrefs.SetInt("score", score);
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        PlayerPrefs.SetFloat("currentHealth", currentHealth);
    }
}
