using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

    public Player player;

    protected Vector2 _initialPosition;
    protected Vector3 _initialCanvasPosition;
    protected RectTransform _transform;
    protected RectTransform _parentTransform;
    protected Ray _ray;
    protected Vector3 _rayPoint;
    
    public float MaxDistance;

	void Start () {
        _transform = GetComponent<RectTransform>();
        _initialPosition = transform.position;
        _initialCanvasPosition = _transform.anchoredPosition3D;
        _parentTransform = transform.parent.GetComponent<RectTransform>();
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 v = eventData.position - _initialPosition; 
        player.Direction =  3*(v).normalized * Mathf.Log10(1 + v.magnitude);

        transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        
        _transform.anchoredPosition3D += new Vector3(0, 0, -_transform.anchoredPosition3D.z);
	}

    public void OnEndDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition = _initialCanvasPosition;
	}
}
