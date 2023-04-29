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
    [SerializeField] Boons boonAdvantage;
    [SerializeField] float increase =1;
    [SerializeField] Boons boonDisadvantage;
    [SerializeField] float decrease =1;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature");
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var stats = player.GetComponent<PlayerStats>();
            switch(boonAdvantage)
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
                default:
                    break;
            }
            switch (boonDisadvantage)
            {
                case Boons.speed:
                    stats.MoveSpeed *= decrease;
                    break;
                case Boons.spin:
                    stats.SpinMoveSpeed += decrease;
                    break;
                case Boons.power:
                    stats.LariatDuration *= decrease;
                    break;
                default:
                    break;


            }
            var destroythis = GameObject.Find("Boons");
            Destroy(destroythis);
        }

        
    }
}
