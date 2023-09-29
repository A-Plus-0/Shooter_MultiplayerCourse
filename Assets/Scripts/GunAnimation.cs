using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string cnst_Shoot = "Shoot";

    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;
    private void Start()
    {
        _gun.shoot += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(cnst_Shoot);
    }

    private void OnDestroy()
    {
        _gun.shoot -= Shoot;
    }

}
