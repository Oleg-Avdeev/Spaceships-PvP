using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SpawnButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Image _selfImage;
    private Vector2 _point0;

    public Wazzapps.UI.GameScreen GameScreen;
    public int SpawnID;

    public void Start()
    {
        _selfImage = GetComponent<Image>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _selfImage.color = new Color(1,1,1, 0.5f);
        _point0 = eventData.position;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _selfImage.color = new Color(1,1,1,1);
        _point0 = _point0 - eventData.position;

        if (_point0.y < -Mathf.Abs(_point0.x)) 
        {
            GameScreen.OnSpawnUnitClick(SpawnID, 0);
            // Spawn UP
        }

        else if (_point0.x < Mathf.Abs(_point0.y)) 
        {
            GameScreen.OnSpawnUnitClick(SpawnID, 1);
            // Spawn RIGHT
        }

        else if (_point0.x > -Mathf.Abs(_point0.y)) 
        {
            GameScreen.OnSpawnUnitClick(SpawnID, -1);
            // Spawn LEFT
        }
    }
}

