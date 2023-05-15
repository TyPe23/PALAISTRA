using UnityEngine;

public class maintainOrientation : MonoBehaviour
{
    public void resetPos()
    {
        transform.localPosition = new Vector3(0, -1, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
