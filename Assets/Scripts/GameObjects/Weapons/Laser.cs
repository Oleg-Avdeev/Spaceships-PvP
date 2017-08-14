using UnityEngine;

public class Laser : Projectile {

	private int _hitsNumber = 2;

	public override void Delete()
	{
        _hitsNumber--;
		if (_hitsNumber > 0) return;
		base.Delete();
	}

}
