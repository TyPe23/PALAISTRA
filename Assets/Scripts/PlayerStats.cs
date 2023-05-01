using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // temp stats (reset at start of run)
    public float MoveSpeed;
    public float DashSpeed;
    public float SpinMoveSpeed;
    public float LariatSpeed;
    public float PileDriverSpeed;
    public float DashDist;
    public float SpinMoveDuration;
    public float LariatDuration;
    public float PileDriverDuration;
    public int DashCost;
    public int SpinCost;
    public int LariatCost;
    public int PileDriverCost;
    public float inputTimeout;
    public int currency;
    public float maxStamina;
    public float staminaRecovery;
    public float SpinHoldCost;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("MoveSpeed"))
        {
            PlayerPrefs.SetFloat("MoveSpeed", 5.25f);
            PlayerPrefs.SetFloat("SpinMoveSpeed", 0f);
            PlayerPrefs.SetFloat("DashSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 3);
            PlayerPrefs.SetFloat("LariatSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 2);
            PlayerPrefs.SetFloat("PileDriverSpeed", PlayerPrefs.GetFloat("MoveSpeed"));
            PlayerPrefs.SetFloat("SpinMoveDuration", 5f);
            PlayerPrefs.SetFloat("DashDist", 3);
            PlayerPrefs.SetFloat("LariatDuration", 0.25f);
            PlayerPrefs.SetFloat("PileDriverDuration", 0.5f);
            PlayerPrefs.SetInt("SpinCost", 15);
            PlayerPrefs.SetInt("DashCost", 10);
            PlayerPrefs.SetInt("LariatCost", 20);
            PlayerPrefs.SetInt("PileDriverCost", 30);
            PlayerPrefs.SetFloat("inputTimeout", 0.5f);
            PlayerPrefs.SetInt("currency", 10);
            PlayerPrefs.SetFloat("maxStamina", 100);
            PlayerPrefs.SetFloat("staminaRecovery", Time.deltaTime * 3);
            PlayerPrefs.SetFloat("SpinHoldCost", PlayerPrefs.GetFloat("staminaRecovery") * 2);
        }

        SpinMoveSpeed = PlayerPrefs.GetFloat("SpinMoveSpeed");
        MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        DashSpeed = PlayerPrefs.GetFloat("DashSpeed");
        LariatSpeed = PlayerPrefs.GetFloat("LariatSpeed");
        PileDriverSpeed = PlayerPrefs.GetFloat("PileDriverSpeed");
        DashDist = PlayerPrefs.GetFloat("DashDist"); ;
        SpinMoveDuration = PlayerPrefs.GetFloat("SpinMoveDuration");
        LariatDuration = PlayerPrefs.GetFloat("LariatDuration");
        PileDriverDuration = PlayerPrefs.GetFloat("PileDriverDuration");
        SpinCost = PlayerPrefs.GetInt("SpinCost", 15);
        DashCost = PlayerPrefs.GetInt("DashCost", 5);
        LariatCost = PlayerPrefs.GetInt("LariatCost", 15);
        PileDriverCost = PlayerPrefs.GetInt("PileDriverCost", 20);
        inputTimeout = PlayerPrefs.GetFloat("inputTimeout");
        currency = PlayerPrefs.GetInt("currency");
        maxStamina = PlayerPrefs.GetFloat("maxStamina");
        staminaRecovery = PlayerPrefs.GetFloat("staminaRecovery");
        SpinHoldCost = PlayerPrefs.GetFloat("SpinHoldCost");
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
