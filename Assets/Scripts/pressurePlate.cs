using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class pressurePlate : MonoBehaviour
{
    public GameObject[] doors;
    public bool active = false;
    private float targetY;
    private float origY;
    public GameObject PressurePlate;
    private bool puzzle = true;
    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        targetY = PressurePlate.transform.position.y - 0.25f;
        origY = PressurePlate.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (PressurePlate.transform.position.y > targetY)
            {
                PressurePlate.transform.Translate(Vector3.down * Time.deltaTime);
            }
            else
            {
                activated = true;
            }
        }
        else
        {
            if (PressurePlate.transform.position.y < origY)
            {
                PressurePlate.transform.Translate(Vector3.up * Time.deltaTime);
            }
            else
            {
                activated = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !puzzle)
        {
            active = false;
        }
    }
}
