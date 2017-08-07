using UnityEngine;

public class Planet : MonoBehaviour
{
    public int RotationSpeed;
    public void Update()
    {
        transform.Rotate(0, RotationSpeed ,0);
    }
}