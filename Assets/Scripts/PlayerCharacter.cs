using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 40;
    [SerializeField] private float _minHeadAngle = -80;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDeley = 0.2f;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private float _inputH;
    private float _inputV;

    private float _rotateY;
    private float _currentRotateX;

    private float _jumpTime;

    private bool _isCrouch = false;
    private float _crouchFase = 0;

    private bool _isSpeedUp = false;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localRotation = Quaternion.identity;
        camera.localPosition = Vector3.zero;
    }
    public void SetInput(float h, float v, float rotateY, bool isCrouch, bool isSpeedUp)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
        _isCrouch = isCrouch;
        _isSpeedUp = isSpeedUp;
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
        Crouch();
    }
    private void Move()
    {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * speed * (_isSpeedUp ? speedUP : 1);
        velocity.y = _rigidbody.velocity.y;
        base.velocity = velocity;
        _rigidbody.velocity = base.velocity;
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    public void Crouch()
    {
        base.isCrouch = _isCrouch;
        if (_isCrouch)
        {
            _crouchFase += Time.deltaTime * 3;
        }
        else
        {
            _crouchFase -= Time.deltaTime * 3;
        }
        _crouchFase = Mathf.Clamp01(_crouchFase);

        _capsuleCollider.center = new Vector3(0, Mathf.Lerp(1f, .875f, Mathf.Clamp01(_crouchFase)), 0);
        _capsuleCollider.height = Mathf.Lerp(2f, 1.75f, Mathf.Clamp01(_crouchFase));
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out Vector2 rotate, out bool isCrouch)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotate.x = _head.localEulerAngles.x;
        rotate.y = transform.eulerAngles.y;
        isCrouch = this.isCrouch;
    }

    public void Jump()
    {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDeley) return;

        _jumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }
}
