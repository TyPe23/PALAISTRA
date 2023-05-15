using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int amount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var ps = other.GetComponent<PlayerStats>();
            var ch = ps.currentHealth+amount;
            if(ch > ps.maxHealth)
            {
                ch = ps.maxHealth;
            }
            ps.currentHealth = ch;
            PlayerPrefs.SetFloat("currentHealth", ps.currentHealth);
            Destroy(gameObject);
        }
    }
}
