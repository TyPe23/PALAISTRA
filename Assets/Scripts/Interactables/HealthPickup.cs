using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private PlayerStats ps;
    private void Start()
    {
        ps = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }
    [SerializeField] private int amount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
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
