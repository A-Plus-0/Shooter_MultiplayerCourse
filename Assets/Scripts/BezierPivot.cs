using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPivot : MonoBehaviour
{
    [SerializeField] private Vector2 _rotation;

    private Transform _pivotTransfrom;
    private Transform _pivotTransfromTEMP;
    [SerializeField] private float _scaleP1;
    [SerializeField] private Transform _transformP1;
    [SerializeField] private Vector3 _transformP1TEMP;
    [SerializeField] private float _scaleP2;
    [SerializeField] private Transform _transformP2;
    [SerializeField] private Vector3 _transformP2TEMP;
    [SerializeField] private bool Calc;

    void OnValidate()
    {
        if (transform.hasChanged)
        {
            _scaleP1 = _transformP1.localPosition.z;
            _scaleP2 = _transformP2.localPosition.z;
            transform.hasChanged = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        }

        
        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
      


        if (_transformP1TEMP == null) _transformP1TEMP = _transformP1.position;
        if (_transformP2TEMP == null) _transformP2TEMP = _transformP2.position;


        if (_transformP1.position.z != _transformP1TEMP.z) { _scaleP1 = _transformP1.localPosition.z; _transformP1TEMP.z = _transformP1.position.z; };
        if (_scaleP1 != _transformP1.localPosition.z)
        {
            _transformP1.localPosition = new Vector3(_transformP1.localPosition.x, _transformP1.localPosition.y, _scaleP1);
        }
    }

}
