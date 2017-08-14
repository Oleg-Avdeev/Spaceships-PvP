using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Wazzapps.UI
{
    public class SpawnCard : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private Image _selfImage;
        private Vector2 _point0;
        private bool _dragging;

        private RectTransform _rTransform;
        private Transform _transform;
        private Vector2 _defaultPosition;

        public Wazzapps.UI.CardsGameScreen CardsGameScreen;
        public Image ShipImage;
        public Text ShipEnergyCost;
        public Text ShipName;
        private int _spawnID;
        private Color _srcColor;

        public bool IsEmpty { get; private set; }

        private void Initialize()
        {
            _selfImage = GetComponent<Image>();
            _rTransform = GetComponent<RectTransform>();
            _transform = transform;
            _defaultPosition = _rTransform.anchoredPosition;
            _srcColor = _selfImage.color;
        }

        public void SetValues(IUnitInfo info)
        {
            if (_transform == null) Initialize();

            _spawnID = info.GetIndex();
            ShipImage.sprite = info.GetBlueIcon();
            ShipEnergyCost.text = info.GetEnergyCost().ToString();

            ShipName.text = $"{info.GetTitle()[0]}<size=7>{info.GetTitle().Remove(0,1)}</size>";

            _rTransform.anchoredPosition = _defaultPosition;
            
            _selfImage.color = _srcColor;
            ShipImage.gameObject.SetActive(true);
            ShipEnergyCost.gameObject.SetActive(true);
            ShipName.gameObject.SetActive(true);

            IsEmpty = false;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (IsEmpty) return;
            _selfImage.color = new Color(_srcColor.r,_srcColor.g,_srcColor.b, _srcColor.a/2);
            _point0 = eventData.position;
            _dragging = true;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            // _point0 = _point0 - eventData.position;
            _point0 = eventData.position;

            float _thr1 =  Screen.width/2;
            // float _thr2 =  2 * _thr1;.
            bool result = false;

            if (_point0.x > _thr1) 
            {
                result = CardsGameScreen.OnSpawnUnitClick(_spawnID, 1);
                // Spawn RIGHT
            }

            // else if (_point0.x < _thr1) 
            // {
                // result = CardsGameScreen.OnSpawnUnitClick(_spawnID, -1);
            //     // Spawn LEFT
            // }

            else 
            {
                result = CardsGameScreen.OnSpawnUnitClick(_spawnID, -1);
                // result = CardsGameScreen.OnSpawnUnitClick(_spawnID, 0);
                // Spawn UP
            }

            _dragging = false;
            _rTransform.anchoredPosition = _defaultPosition;

            if (!result) return;
            IsEmpty = true;
            _selfImage.color = new Color(0,0,0,0);
            ShipImage.gameObject.SetActive(false);
            ShipEnergyCost.gameObject.SetActive(false);
            ShipName.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (_dragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(0);
                transform.position = new Vector3(rayPoint.x, rayPoint.y, 0);
                _rTransform.anchoredPosition3D += new Vector3(0, 0, -_rTransform.anchoredPosition3D.z);
            }
        }
    }
}