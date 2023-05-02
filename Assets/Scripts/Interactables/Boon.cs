using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Boons
{
    speed,
    power,
    spin,
}

enum TypeofBoon
{
    purchase,
    ending,
}

public class Boon : MonoBehaviour
{
    private GameObject player;
    [SerializeField] Boons boonAdvantage;
    [SerializeField] float increase =1;
    [SerializeField] Boons boonDisadvantage;
    [SerializeField] float decrease =1;
    [SerializeField] TypeofBoon type;
    [SerializeField] int cost;

    private void Awake()
    {
        if(cost == default)
        {
            cost = 0;
        }
        player = GameObject.Find("PlayerArmature");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")&&type==TypeofBoon.purchase)
        {
            var stats = player.GetComponent<PlayerStats>();
            if (stats.currency >= cost)
            {

                switch (boonAdvantage)
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
                stats.currency -= cost;
                Destroy(gameObject);
            }   
        }
        else if(other.transform.CompareTag("Player") && type == TypeofBoon.ending)
        {
            var stats = player.GetComponent<PlayerStats>();
            switch (boonAdvantage)
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
            Destroy(gameObject);
            endofLevelBoons();

        }

        
    }
    public void endofLevelBoons()
    {
        var destroythis = GameObject.Find("Boons");
        Destroy(destroythis);
    }
}
