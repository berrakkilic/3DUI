using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
    }
}
