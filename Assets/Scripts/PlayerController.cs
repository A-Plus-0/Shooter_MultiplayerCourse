using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;

    [SerializeField] private List<PlayerGun> _guns;
    [SerializeField] private int _currentGun = 0;
    [SerializeField] private float _changeGunDelay = .1f;
    private float _changeGunDelayTemp = 0;

    [SerializeField] private float _mouseSensetivite = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _hold = false;
    // [SerializeField] ControlsSetting _cs;
    [SerializeField] KeyCode _jumpKey;
    [SerializeField] KeyCode _CrouchKey;
    [SerializeField] KeyCode _speedUpKey;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }
    private void Update()
    {
        if (_hold) { return; }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);

        bool space = Input.GetKeyDown(_jumpKey);

        bool isCrouch = Input.GetKey(_CrouchKey);

        bool isSpeedUp = Input.GetKey(_speedUpKey);

        //_currentGun += Input.

        _changeGunDelayTemp -= Time.deltaTime;
        ChangeGun(Math.Sign(Input.mouseScrollDelta.y));

        _player.SetInput(h, v, mouseX * _mouseSensetivite, isCrouch, isSpeedUp);

        _player.RotateX(-mouseY * _mouseSensetivite);

        if (space) _player.Jump();

        if (isShoot && _guns[_currentGun].TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendMove();
    }

    private void ChangeGun(int switchIndex)
    {
        if (switchIndex != 0 && _changeGunDelayTemp < 0)
        {
            int GC = _guns.Count;
            _currentGun += switchIndex;
            _currentGun = _currentGun - ((int)MathF.Floor(_currentGun / (float)GC) * GC);
            _changeGunDelayTemp = _changeGunDelay;
            for (int i = 0; i < GC; i++)
            {
                    _guns[i].gameObject.SetActive(i == _currentGun);               
            }
        }

    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out Vector2 rotate, out bool isCrouch);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"tGun", _currentGun},

            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},

            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},

            {"rX", rotate.x},
            {"rY", rotate.y},

            {"iC", isCrouch}
        };
        _multiplayerManager.SendMessage("move", data);
    }

    public void Restart(string jsonRestartInfo)
    {
        RestartInfo RI = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);
        StartCoroutine(Hold());

        _player.transform.position = new Vector3(RI.x, 0, RI.z);
        _player.SetInput(0, 0, 0, false, false);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", RI.x},
            {"pY", 0},
            {"pZ", RI.z},

            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},

            {"rX", 0},
            {"rY", 0},

            {"iC", false}
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;

    public float pX;
    public float pY;
    public float pZ;

    public float dX;
    public float dY;
    public float dZ;
}

[Serializable]
public struct RestartInfo
{
    public float x;
    public float z;
}
