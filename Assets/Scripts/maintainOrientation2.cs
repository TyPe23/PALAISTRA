using UnityEngine;

public class maintainOrientation2 : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
