using UnityEngine;
using System.Collections.Generic;

public interface IMothershipSpawner {
	ISpawnData[] GetSpawnData();
	bool TrySpawnUnit(int id, int shipLane);
}

public interface IMothership {
	Transform GetTransform();
	int GetCurrentEnergy();
	List<Ship> GetShips();
}

public interface ISpawnData
{
	double GetTimeToAllowSpawn();
	void Spawn();
	IUnitInfo GetUnitInfo();
}

public interface IEnergyData
{
	void Initialize(IUnitInfo unit);
	void Spend(int amount);
	int GetCurrentEnergy();
}

public interface IUnitInfo
{
	int GetIndex();
	Sprite GetBlueIcon();
	Sprite GetRedIcon();
	string GetTitle();
	int GetEnergyCost();
	float GetHealth();
	float GetDamage();
	float GetFireRate();
	float GetShipSpeed();
	float GetProjectileSpeed();
	double GetSpawnTime();
}