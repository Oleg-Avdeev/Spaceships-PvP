using UnityEngine;
// using System.Collections.Generic;

// public enum Fraction
// {
//     Blue,
//     Red,
//     Green,
//     Yellow
// }

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public float MaxVelocity = 1;
    public float MaxTorque = 4;

    public void Awake()
    {
        Instance = GetComponent<GameController>();
    }
}
//     public static int NumberOfFractions = 4;

//     public List<Ship>[] FractionShipsList;

//     public static GameController Instance
//     { 
//         get { return _instance;}
//     }
//     private static GameController _instance;

//     private Vector3 _deltaTouch;
//     private Vector3 _mouseInitial;

//     public void Awake()
//     {
//         Debug.Log("Init");
//         GameController._instance = this;
//         FractionShipsList = new List<Ship>[NumberOfFractions];
//         for (int i = 0; i < NumberOfFractions; i++)
//             FractionShipsList[i] = new List<Ship>();
//     }


//     public void Update()
//     {
//         if (Input.touchCount == 1)
//             _deltaTouch = Input.GetTouch(0).deltaPosition;
        
//         if (Input.GetMouseButton(0))
//         {
//             if (_mouseInitial == Vector3.zero)
//                 _mouseInitial = Input.mousePosition; 
//             _deltaTouch = -(Input.mousePosition - _mouseInitial)/500;
//         } else  _mouseInitial = Vector3.zero; 

//         _deltaTouch = _deltaTouch*0.95f;
//         Camera.main.transform.position += _deltaTouch;
//     }
// }
