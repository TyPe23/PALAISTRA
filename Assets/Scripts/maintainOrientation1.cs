using UnityEngine;

public class maintainOrientation1 : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
