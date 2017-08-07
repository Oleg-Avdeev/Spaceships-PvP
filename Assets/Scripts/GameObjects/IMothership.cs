using UnityEngine;
using System.Collections.Generic;

public interface IMothershipSpawner {
	ISpawnData[] GetSpawnData();
	bool TrySpawnUnit(int id);
}

public interface IMothership {
	Transform GetTransform();
	List<Ship> GetShips();
}

public interface ISpawnData
{
	double GetTimeToAllowSpawn();
	void Spawn();
	IUnitInfo GetUnitInfo();
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