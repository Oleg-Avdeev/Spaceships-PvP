using UnityEngine;

[System.Serializable]
public class UnitInfo : IUnitInfo
{
	[SerializeField] private int Index;
	[SerializeField] private string Title;
	[SerializeField] private Texture2D Icon;
	[SerializeField] private int EnergyCost;
	[SerializeField] private float Health;
	[SerializeField] private float Damage;
	[SerializeField] private float AttackSpeed;
	[SerializeField] private double SpawnTime;

	public int GetIndex()
	{
		return Index;
	}

	public Texture2D GetIcon()
	{
		return Icon;
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
