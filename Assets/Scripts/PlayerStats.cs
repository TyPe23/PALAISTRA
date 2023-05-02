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
    public int DashCost;
    public int SpinCost;
    public int LariatCost;
    public int PileDriverCost;
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
            PlayerPrefs.SetFloat("DashDist", 3);
            PlayerPrefs.SetInt("SpinCost", 5);
            PlayerPrefs.SetInt("DashCost", 5);
            PlayerPrefs.SetInt("LariatCost", 10);
            PlayerPrefs.SetInt("PileDriverCost", 10);
            PlayerPrefs.SetInt("currency", 10);
            PlayerPrefs.SetFloat("maxStamina", 100);
            PlayerPrefs.SetFloat("staminaRecovery", Time.fixedDeltaTime * 3);
            PlayerPrefs.SetFloat("SpinHoldCost", PlayerPrefs.GetFloat("staminaRecovery") * 2);
        }

        SpinMoveSpeed = PlayerPrefs.GetFloat("SpinMoveSpeed");
        MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
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
        staminaRecovery = PlayerPrefs.GetFloat("staminaRecovery");
        SpinHoldCost = PlayerPrefs.GetFloat("SpinHoldCost");
    }   
}
