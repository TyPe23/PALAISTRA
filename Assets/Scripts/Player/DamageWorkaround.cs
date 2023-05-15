using UnityEngine;
using state = playerStates;

public class DamageWorkaround : MonoBehaviour
{
    public PlayerStates states;
    public PlayerStats stats;

    private void OnTriggerStay(Collider collision)
    {
        if ((collision.transform.CompareTag("enemy") || collision.transform.CompareTag("trap") || collision.transform.CompareTag("projectile")) && !states.invul && (states.state == state.MOVE || states.state == state.EXHAUSTED))
        {
            states.ChangeState(state.HIT);

            if (states.canDamage)
            {
                if (collision.transform.CompareTag("enemy") || collision.transform.CompareTag("projectile"))
                {
                    stats.takeDamage(2);
                }
                else if (collision.transform.CompareTag("trap"))
                {
                    stats.takeDamage(5);
                }
            }
        }
        else if (collision.transform.CompareTag("enemy") && states.state == state.DASH)
        {
            states.ChangeState(state.STOP);
        }
    }
}
