using UnityEngine;

[System.Serializable]
public class UnitInfo : IUnitInfo
{
	[SerializeField] private int Index;
	[SerializeField] private string Title;
	[SerializeField] private Sprite Icon;
	[SerializeField] private Sprite RedIcon;
	[SerializeField] private int EnergyCost;
	[SerializeField] private float Health;
	[SerializeField] private float Damage;
	[SerializeField] private float AttackSpeed;
	[SerializeField] private double SpawnTime;

	public int GetIndex()
	{
		return Index;
	}

	public Sprite GetBlueIcon()
	{
		return Icon;
	}

	public Sprite GetRedIcon()
	{
		return RedIcon;
	}

	public string GetTitle()
	{
		return Title;
	}

	public int GetEnergyCost()
	{
		return EnergyCost;
	}

	public float GetHealth()
	{
		return Health;
	}

	public float GetDamage()
	{
		return Damage;
	}

	public float GetAttackSpeed()
	{
		return AttackSpeed;
	}

	public double GetSpawnTime()
	{
		return SpawnTime;
	}

}
