using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string cnst_jumpName = "Grounded";
    private const string cnst_speed = "Speed";
    private const string cnst_isCrouch = "isCrouch";



    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Animator _animatorFoot;
    [SerializeField] private Animator _animatorHead;
    [SerializeField] private Animator _animatorBody;

    // [SerializeField] private Rigidbody _rb;
    // [SerializeField] private float _maxSpeed;
    [SerializeField] private Character _character;

    private void Update()
    {
        Vector3 localVelocity = _character.transform.InverseTransformVector(_character.velocity);
        float speed = localVelocity.magnitude / _character.speed;
        float sign = Mathf.Sign(localVelocity.z);

        _animatorFoot.SetFloat(cnst_speed, speed * sign);
        _animatorFoot.SetBool(cnst_jumpName, _checkFly.IsFly == false);
        _animatorHead.SetBool(cnst_isCrouch, _character.isCrouch);
        _animatorBody.SetBool(cnst_isCrouch, _character.isCrouch);

    }
}
