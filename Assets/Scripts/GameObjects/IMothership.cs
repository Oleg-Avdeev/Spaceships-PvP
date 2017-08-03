using UnityEngine;

public interface IMothershipSpawner {
	ISpawnData[] GetSpawnData();
	bool TrySpawnUnit(int id);
}

public interface IMothership {
	Transform GetTransform();
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
	Texture2D GetIcon();
	string GetTitle();
	int GetEnergyCost();
	float GetHealth();
	float GetDamage();
	float GetAttackSpeed();
	double GetSpawnTime();
}