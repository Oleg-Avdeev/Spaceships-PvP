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
	[SerializeField] private float FireRate;
	[SerializeField] private float ShipSpeed;
	[SerializeField] private float ProjectileSpeed;
	[SerializeField] private float SpawnTime;

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
		return RemoteSettings.GetInt($"{GetTitle()}-EnergyCost", EnergyCost); // EnergyCost;
	}

	public float GetHealth()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-Health", Health); // Health;
	}

	public float GetDamage()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-Damage", Damage); // Damage;
	}

	public float GetFireRate()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-FireRate", FireRate); // FireRate;
	}

	public float GetShipSpeed()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-ShipSpeed", ShipSpeed)*4; // ShipSpeed;
	}

	public float GetProjectileSpeed()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-ProjectileSpeed", ProjectileSpeed); // ProjectileSpeed;
	}

	public double GetSpawnTime()
	{
		return RemoteSettings.GetFloat($"{GetTitle()}-SpawnTime", SpawnTime); // SpawnTime;
	}

}
