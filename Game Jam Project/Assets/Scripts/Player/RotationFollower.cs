using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = target.rotation;
    }
}
