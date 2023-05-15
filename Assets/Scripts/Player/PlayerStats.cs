using HighScore;
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
        HS.Init(this, "PALAISTRA");

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
        staminaRecovery = PlayerPrefs.GetFloat("staminaRecovery");
        exhaustedRecovery = PlayerPrefs.GetFloat("exhaustedRecovery");
        SpinHoldCost = PlayerPrefs.GetFloat("SpinHoldCost");
        roomCount = PlayerPrefs.GetInt("roomCount");
    }

    private void Start()
    {
        currentHealth = PlayerPrefs.GetFloat("currentHealth");

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    void FixedUpdate()
    {
        healthSlider.value = currentHealth; 
        score = PlayerPrefs.GetInt("Score");
    }

    public void adjustScore(int scoreChange)
    {
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
