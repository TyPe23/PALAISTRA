using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //TODO damage player
            print("Player damage needed");
        }
        if (other.transform.CompareTag("enemy"))
        {
            print("Should hurt enemy");
            other.gameObject.GetComponent<PlayerEnemyInteraction>().health -= damage;
        }
    }
}
