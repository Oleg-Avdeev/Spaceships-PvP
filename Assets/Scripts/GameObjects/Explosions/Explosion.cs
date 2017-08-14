using UnityEngine;

[RequireComponent(typeof(ImageAnimator))]
public class Explosion : MonoBehaviour
{
    public void Start()
    {
        GetComponent<ImageAnimator>().EndCallback = DestroyGameObject;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}