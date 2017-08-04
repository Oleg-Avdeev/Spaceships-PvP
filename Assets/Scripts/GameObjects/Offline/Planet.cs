using UnityEngine;

public class Planet : MonoBehaviour
{
    public int RotationSpeed;
    public void Update()
    {
        transform.Rotate(RotationSpeed, 0 ,0);
    }
}