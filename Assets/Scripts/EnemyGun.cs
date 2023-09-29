using UnityEngine;

public class EnemyGun : Gun
{
    [SerializeField] private Bullet _bulletPrefab;
    public void Shoot(Vector3 position, Vector3 velocity)
    {
        Instantiate(_bulletPrefab, position, Quaternion.identity).Init(velocity);
        shoot?.Invoke();
    }
}
