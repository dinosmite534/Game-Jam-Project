using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransform.position + offset;
    }
}
