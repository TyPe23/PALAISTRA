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
    public float DashDuration;
    public float SpinMoveDuration;
    public float LariatDuration;
    public float PileDriverDuration;
    public float inputTimeout;
    public int currency;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MoveSpeed"))
        {
            PlayerPrefs.SetFloat("MoveSpeed", 5.25f);
            PlayerPrefs.SetFloat("SpinMoveSpeed", 0f);
            PlayerPrefs.SetFloat("DashSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 3);
            PlayerPrefs.SetFloat("LariatSpeed", PlayerPrefs.GetFloat("MoveSpeed") * 2);
            PlayerPrefs.SetFloat("PileDriverSpeed", PlayerPrefs.GetFloat("MoveSpeed"));
            PlayerPrefs.SetFloat("SpinMoveDuration", 5f);
            PlayerPrefs.SetFloat("DashDuration", 0.25f);
            PlayerPrefs.SetFloat("LariatDuration", 0.25f);
            PlayerPrefs.SetFloat("PileDriverDuration", 0.5f);
            PlayerPrefs.SetFloat("inputTimeout", 0.5f);
            PlayerPrefs.SetInt("currency", 10);
        }

        SpinMoveSpeed = PlayerPrefs.GetFloat("SpinMoveSpeed");
        MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        DashSpeed = PlayerPrefs.GetFloat("DashSpeed");
        LariatSpeed = PlayerPrefs.GetFloat("LariatSpeed");
        PileDriverSpeed = PlayerPrefs.GetFloat("PileDriverSpeed");
        DashDuration = PlayerPrefs.GetFloat("DashDuration"); ;
        SpinMoveDuration = PlayerPrefs.GetFloat("SpinMoveDuration");
        LariatDuration = PlayerPrefs.GetFloat("LariatDuration");
        PileDriverDuration = PlayerPrefs.GetFloat("PileDriverDuration");
        inputTimeout = PlayerPrefs.GetFloat("inputTimeout");
        currency = PlayerPrefs.GetInt("currency");
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
