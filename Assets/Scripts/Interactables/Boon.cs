using UnityEngine;

enum Boons
{
    speed,
    lariatcost,
    spin,
    piledrivercost,
    dashcost,
    spincost,
    maxhealth,
    maxstam,
}

enum TypeofBoon
{
    purchase,
    ending,
}

public class Boon : MonoBehaviour
{
    private GameObject player;
    private PlayerStats ps;
    private StarterAssetsInputs sai;
    [SerializeField] Boons boonAdvantage;
    [SerializeField] float increase =1;
    [SerializeField] Boons boonDisadvantage;
    [SerializeField] float decrease =1;
    [SerializeField] TypeofBoon type;
    [SerializeField] int cost;
    [SerializeField] TextMesh description;

    #region LifeSpan
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        ps = player.GetComponent<PlayerStats>();
        sai = player.GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        int count = Boons.GetValues(typeof(Boons)).Length - 1;
        boonAdvantage = (Boons)Random.Range(0, count);
        increase = (float)System.Math.Round(Random.Range(1.1f, 1.3f), 2);
        boonDisadvantage = (Boons)Random.Range(0, count);
        while (boonAdvantage == boonDisadvantage)
        {
            boonDisadvantage = (Boons)Random.Range(0, count);
        }
        decrease = (float)System.Math.Round(Random.Range(0.8f, 0.9f),2);
        if (type == TypeofBoon.purchase) { 
            cost = 5 * Random.Range(5, 8);
            #region Text
            string advStr = "";
            switch (boonAdvantage)
            {
                case Boons.speed:
                    advStr = "Speed";
                    break;
                case Boons.spin:
                    advStr = "Spin Movement";
                    break;
                case Boons.dashcost:
                    advStr = "Dash Cost";
                    break;
                case Boons.lariatcost:
                    advStr = "Lariat Cost";
                    break;
                case Boons.spincost:
                    advStr = "Spin Cost";
                    break;
                case Boons.piledrivercost:
                    advStr = "Pile Driver Cost";
                    break;
                case Boons.maxhealth:
                    advStr = "Max Health";
                    break;
                case Boons.maxstam:
                    advStr = "Max Stamina";
                    break;
                default:
                    break;
            }
            string disStr = "";
            switch (boonDisadvantage)
            {
                case Boons.speed:
                    disStr = "Speed";
                    break;
                case Boons.spin:
                    disStr = "Spin Movement";
                    break;
                case Boons.dashcost:
                    disStr = "Dash Cost";
                    break;
                case Boons.lariatcost:
                    disStr = "Lariat Cost";
                    break;
                case Boons.spincost:
                    disStr = "Spin Cost";
                    break;
                case Boons.piledrivercost:
                    disStr = "Pile Driver Cost";
                    break;
                case Boons.maxhealth:
                    disStr = "Max Health";
                    break;
                case Boons.maxstam:
                    disStr = "Max Stamina";
                    break;
                default:
                    break;
            }
            #endregion Text
            string s = ("{0} is increased by {1}%\n{2} is decreased by {3}%\nCost: '{4}'");
            string des = string.Format(s, advStr,increase*100,disStr,decrease*100,cost);
            description.text = des;
        }
    }
    #endregion LifeSpan
    private void OnTriggerStay(Collider other)
    {
        if (sai.interact && other.transform.CompareTag("Player"))
        {
            if (type == TypeofBoon.purchase)
            {
                if (ps.currency >= cost)
                {
                    #region AdvDisPurchase
                    switch (boonAdvantage)
                    {
                        case Boons.speed:
                            ps.MoveSpeed *= increase;
                            break;
                        case Boons.spin:
                            ps.SpinMoveSpeed *= increase;
                            break;
                        case Boons.dashcost:
                            ps.DashCost = (int)(ps.DashCost * increase); 
                            break;
                        case Boons.lariatcost:
                            ps.LariatCost = (int)(ps.LariatCost * increase);
                            break;
                        case Boons.spincost:
                            ps.SpinCost = (int)(ps.SpinCost * increase);
                            break;
                        case Boons.piledrivercost:
                            ps.PileDriverCost = (int)(ps.PileDriverCost * increase);
                            break;
                        case Boons.maxhealth:
                            ps.maxHealth *= increase;
                            break;
                        case Boons.maxstam:
                            ps.maxStamina *= increase;
                            break;
                        default:
                            break;
                    }
                    switch (boonDisadvantage)
                    {
                        case Boons.speed:
                            ps.MoveSpeed *= decrease;
                            break;
                        case Boons.spin:
                            ps.SpinMoveSpeed *= decrease;
                            break;
                        case Boons.dashcost:
                            ps.DashCost = (int)(ps.DashCost * increase);
                            break;
                        case Boons.lariatcost:
                            ps.LariatCost = (int)(ps.LariatCost * decrease);
                            break;
                        case Boons.spincost:
                            ps.SpinCost = (int)(ps.SpinCost * decrease);
                            break;
                        case Boons.piledrivercost:
                            ps.PileDriverCost = (int)(ps.PileDriverCost * decrease);
                            break;
                        case Boons.maxhealth:
                            ps.maxHealth *= decrease;
                            break;
                        case Boons.maxstam:
                            ps.maxStamina *= decrease;
                            break;
                        default:
                            break;
                    }
                    #endregion AdvDisPurchase
                    ps.currency -= cost;
                    Destroy(gameObject);
                }
            }
            else if (type == TypeofBoon.ending)
            {
                #region AdvDisEnding
                switch (boonAdvantage)
                {
                    case Boons.speed:
                        ps.MoveSpeed *= increase;
                        break;
                    case Boons.spin:
                        ps.SpinMoveSpeed *= increase;
                        break;
                    case Boons.dashcost:
                        ps.DashCost = (int)(ps.DashCost * increase);
                        break;
                    case Boons.lariatcost:
                        ps.LariatCost = (int)(ps.LariatCost * increase);
                        break;
                    case Boons.spincost:
                        ps.SpinCost = (int)(ps.SpinCost * increase);
                        break;
                    case Boons.piledrivercost:
                        ps.PileDriverCost = (int)(ps.PileDriverCost * increase);
                        break;
                    case Boons.maxhealth:
                        ps.maxHealth *= increase;
                        break;
                    case Boons.maxstam:
                        ps.maxStamina *= increase;
                        break;
                    default:
                        break;
                }
                switch (boonDisadvantage)
                {
                    case Boons.speed:
                        ps.MoveSpeed *= decrease;
                        break;
                    case Boons.spin:
                        ps.SpinMoveSpeed *= decrease;
                        break;
                    case Boons.dashcost:
                        ps.DashCost = (int)(ps.DashCost * increase);
                        break;
                    case Boons.lariatcost:
                        ps.LariatCost = (int)(ps.LariatCost * decrease);
                        break;
                    case Boons.spincost:
                        ps.SpinCost = (int)(ps.SpinCost * decrease);
                        break;
                    case Boons.piledrivercost:
                        ps.PileDriverCost = (int)(ps.PileDriverCost * decrease);
                        break;
                    case Boons.maxhealth:
                        ps.maxHealth *= decrease;
                        break;
                    case Boons.maxstam:
                        ps.maxStamina *= decrease;
                        break;
                    default:
                        break;
                }
                #endregion AdvDisEnding
                Destroy(gameObject);
                endofLevelBoons();

            }
        }
        
    }
    public void endofLevelBoons()
    {
        var destroythis = GameObject.Find("Boons");
        Destroy(destroythis);
    }
}
