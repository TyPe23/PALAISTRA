using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasePhysics : MonoBehaviour
{
    [Tooltip("Input the possible collidable items tags that can break")]
    [SerializeField] private List<string> collidables;
    [SerializeField] private List<GameObject> spawns;
    private bool sprint;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<StarterAssetsInputs>(out StarterAssetsInputs com))
        {
            sprint = com.sprint;
        }
        foreach(string possible in collidables)
        {
            if (other.transform.CompareTag(possible)&&sprint)
            {
                int chance = Random.Range(1, 5);
                print(chance);
                if(chance == 4) {
                    Instantiate(spawns[0], transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
