using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipController : Ship, IMothership, IMothershipSpawner {

	private ISpawnData[] _units;
	private IEnergyData _energyData;

	private class SpawnData : ISpawnData
	{
		private IUnitInfo _unit;
		private double _lastTimeCreated = 0;

		public SpawnData(IUnitInfo unit)
		{
			_unit = unit;
		}

		public void Spawn()
		{
			_lastTimeCreated = PhotonNetwork.time;
		}

		public double GetTimeToAllowSpawn()
		{
			double spawnTime = _unit.GetSpawnTime();
			double diff = spawnTime - (PhotonNetwork.time - _lastTimeCreated);
			return diff < 0 ? 0 : (diff > spawnTime ? spawnTime : diff);
		}

		public IUnitInfo GetUnitInfo()
		{
			return _unit;
		}
	}

	private class EnergyData : IEnergyData
	{
		private IUnitInfo _unit;
		private double _gameStartTime = 0;
		private int _totalSpent = 0;

		public void Initialize(IUnitInfo unit)
		{
			_gameStartTime = PhotonNetwork.time;
			_unit = unit;
		}

		public void Spend(int amount)
		{
			_totalSpent += amount;
		}

		public int GetCurrentEnergy()
		{
			return (int)((int)((PhotonNetwork.time - _gameStartTime)/_unit.GetSpawnTime())*_unit.GetEnergyCost())
				- _totalSpent;
		}
	}

	void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		_transform = transform;
		if(instantiationData != null)
		{
			if(instantiationData.Length > 0)
			{
				_transform = transform;
				int[] shipsInStack = (int[])instantiationData[1];
				int dreadnoughtID = (int)instantiationData[0];

					if (ownerId == PhotonNetwork.masterClient.ID)
						transform.position = new Vector3(0,-30,0);
					else transform.position = new Vector3(0,30,0);

					if (!PhotonNetwork.isMasterClient)
							HealthText.transform.localRotation = Quaternion.Euler(0,0,-90);
					else 
						HealthText.transform.localRotation = Quaternion.Euler(0,0,90);
				
				
				if (PhotonNetwork.isMasterClient == isMine) HealthText.transform.position = new Vector3(
								-Mathf.Abs(HealthText.transform.position.x),
								HealthText.transform.position.y,
								HealthText.transform.position.z
							);
				else HealthText.transform.position = new Vector3(
								Mathf.Abs(HealthText.transform.position.x),
								HealthText.transform.position.y,
								HealthText.transform.position.z
							);
					
				_units = new SpawnData[shipsInStack.Length];

				for(int i = 0; i < _units.Length; i++)
				{
					_units[i] = new SpawnData(Resolver.Instance.Units.Collection[shipsInStack[i]]);
				}

				_unit = Resolver.Instance.Units.Collection[dreadnoughtID];
				_energyData = new EnergyData();
				
				Resolver.Instance.RoomController.GetMyShips(ownerId).Add(this);
				
				ParseIUnitInfo();
				ColorShip();
			}
		}
	}

	public override void LogicUpdate()
	{
		_targetDirection = Vector2.zero;
	}

	public ISpawnData[] GetSpawnData()
	{
		return _units;
	}

	private Ship _newShip;
	public bool TrySpawnUnit(int id, int shipLane)
	{
		// Debug.Log("_units == null " + _units == null);
		// Debug.Log($"ID {id} _units.Length {_units.Length}");
		// Debug.Log("id < 0 || id >= _units.Length " + (id < 0 || id >= _units.Length));
		
		if(_units == null) return false;
		if(id < 0 || id >= _units.Length) return false;
		// if(_units[id].GetTimeToAllowSpawn() > 0) return false;
		if(_units[id].GetUnitInfo().GetEnergyCost() > _energyData.GetCurrentEnergy()) return false;

		IUnitInfo unit = _units[id].GetUnitInfo();
		_energyData.Spend(unit.GetEnergyCost());
		_units[id].Spawn();

        PhotonNetwork.Instantiate(unit.GetTitle(), _transform.position, Quaternion.identity, 0,
			new object[] { unit.GetIndex(), shipLane }).GetComponent<Ship>();

		return true;
	}

	public void Initialize()
	{
		_energyData.Initialize(_unit);
		MyMothership = this;
	}

	public Transform GetTransform()
	{
		return _transform;
	}

	public List<Ship> GetShips()
	{
		return Resolver.Instance.RoomController.GetMyShips(ownerId);
	}

	public int GetCurrentEnergy()
	{
		return _energyData.GetCurrentEnergy();
	}
}
