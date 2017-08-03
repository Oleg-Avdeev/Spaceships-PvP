using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipController : PhotonView, IMothership, IMothershipSpawner {
	private Transform _transform;
	private List<GameObject> _ships = new List<GameObject>();
	private ISpawnData[] _units;

    Vector3 _serverPosition = Vector3.zero;
    Quaternion _serverRotation = Quaternion.identity;

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
				int[] shipsInStack = (int[])instantiationData[0];
				_units = new SpawnData[shipsInStack.Length];
				for(int i = 0; i < _units.Length; i++)
				{
					_units[i] = new SpawnData(Resolver.Instance.Units.Collection[shipsInStack[i]]);
				}
			}
		}
	}

	public ISpawnData[] GetSpawnData()
	{
		return _units;
	}

	public bool TrySpawnUnit(int id)
	{
		if(_units == null) return false;
		if(id < 0 || id >= _units.Length) return false;
		if(_units[id].GetTimeToAllowSpawn() > 0) return false;

		IUnitInfo unit = _units[id].GetUnitInfo();
		_units[id].Spawn();

        PhotonNetwork.Instantiate(unit.GetTitle(), _transform.position, Quaternion.identity, 0,
			new object[] { unit.GetIndex() });

		return true;
	}

	public Transform GetTransform()
	{
		return _transform;
	}
}
