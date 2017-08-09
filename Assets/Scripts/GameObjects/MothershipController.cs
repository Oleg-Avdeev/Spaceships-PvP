using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipController : Ship, IMothership, IMothershipSpawner {

	private ISpawnData[] _units;

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
					
				_units = new SpawnData[shipsInStack.Length];

				for(int i = 0; i < _units.Length; i++)
				{
					_units[i] = new SpawnData(Resolver.Instance.Units.Collection[shipsInStack[i]]);
				}

				_unit = Resolver.Instance.Units.Collection[dreadnoughtID];
				
				if (isMine)
					Resolver.Instance.RoomController.GetMyShips().Add(this);
				else 
					Resolver.Instance.RoomController.GetEnemyShips().Add(this);
				
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
	public bool TrySpawnUnit(int id)
	{
		if(_units == null) return false;
		if(id < 0 || id >= _units.Length) return false;
		if(_units[id].GetTimeToAllowSpawn() > 0) return false;

		IUnitInfo unit = _units[id].GetUnitInfo();
		_units[id].Spawn();

        PhotonNetwork.Instantiate(unit.GetTitle(), _transform.position, Quaternion.identity, 0,
			new object[] { unit.GetIndex() }).GetComponent<Ship>();

		return true;
	}

	public Transform GetTransform()
	{
		return _transform;
	}

	public List<Ship> GetShips()
	{
		if (isMine) return Resolver.Instance.RoomController.GetMyShips();
		else return Resolver.Instance.RoomController.GetEnemyShips();
	}
}
