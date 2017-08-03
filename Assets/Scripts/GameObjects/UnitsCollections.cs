using UnityEngine;

[CreateAssetMenu(fileName = "UnitsCollections")]
public class UnitsCollections : ScriptableObject {
	[SerializeField]
	public UnitInfo[] Collection;
}
