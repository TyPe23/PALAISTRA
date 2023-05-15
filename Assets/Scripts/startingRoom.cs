using UnityEngine;

public class StartingRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetFloat("currentHealth", PlayerPrefs.GetFloat("maxHealth"));
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        PlayerPrefs.SetInt("Score", 999 + (int)Time.time);
    }
}
