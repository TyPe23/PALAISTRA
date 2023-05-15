using HighScore;
using UnityEngine;

public class HighScoreInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HS.Clear(this);
        HS.Init(this, "PALAISTRA");

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
        PlayerPrefs.SetInt("Score", 999);
    }
}
