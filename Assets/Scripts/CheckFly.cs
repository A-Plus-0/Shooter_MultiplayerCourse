using UnityEngine;

public class CheckFly : MonoBehaviour
{
    public bool IsFly { get; private set; }
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radius;
    [SerializeField] private float _couoteTime = .15f;
    private float _flyTime = 0;

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            IsFly = false;
            _flyTime = 0;
        }
        else
        {
            _flyTime += Time.deltaTime;
            if (_flyTime > _couoteTime)
            {
                IsFly = true;
            }
        }

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
