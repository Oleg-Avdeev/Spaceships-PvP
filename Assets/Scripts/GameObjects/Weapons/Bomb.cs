using UnityEngine;

public class Bomb : Projectile
{
    [PunRPC]
    protected override void SyncExplode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Vector2 direction = Vector2.up;
        for (int i = 0; i < 10; i++)
        {
            PhotonNetwork.Instantiate("Shrapnel", Vector3.zero, Quaternion.identity, 0, 
                new object[] {
                    viewID, _shipIndex, direction, Owner
                }
            );
            direction = Quaternion.Euler(0,0,36) * direction;
        }
    }
}