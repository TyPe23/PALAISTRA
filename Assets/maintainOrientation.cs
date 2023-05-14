using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintainOrientation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, -1, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
