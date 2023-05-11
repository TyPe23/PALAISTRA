using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingRoom : MonoBehaviour
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
