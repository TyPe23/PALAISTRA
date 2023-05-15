using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 origin;

    private void Start()
    {
        origin = player.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = origin;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
