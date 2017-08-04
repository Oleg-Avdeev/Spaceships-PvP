using UnityEngine;

public class ClockMonoBehaviour : MonoBehaviour
{
    public float TickLength;
    private float _lastTickTime;
    
    public void Update()
    {
        if (Time.time - _lastTickTime >= TickLength)
        {
            _lastTickTime = Time.time;
            OnTick();
        }
    }

    public virtual void OnTick()
    {

    }
}