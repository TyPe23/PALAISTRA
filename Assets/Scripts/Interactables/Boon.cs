using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Boons
{
    speed,
    power,
    spin,
}

public class Boon : MonoBehaviour
{
    private GameObject player;
    [SerializeField] Boons boonType;
    [SerializeField] float increase;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature");
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var stats = player.GetComponent<PlayerStats>();
            switch(boonType)
            {
                case Boons.speed:
                    stats.MoveSpeed *= increase;
                    break;
                case Boons.spin:
                    stats.SpinMoveSpeed += increase;
                    break;
                case Boons.power:
                    stats.LariatDuration *= increase;
                    break;
            }
            var destroythis = GameObject.Find("Boons");
            Destroy(destroythis);
        }

        
    }
}
