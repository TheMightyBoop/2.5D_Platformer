using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour {
    
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    public Transform _rotatableTransform;
    [SerializeField]
    public Transform _scalableTransform;
    
    public float StretchMultiplier = 0.005f;
    public float SquashMultiplier = 0.06f;
    public float DelayMultiplier = 0.2f;
    public float ScaleChangeRate = 20f;
    
    private Quaternion _targetRotation;
    private Quaternion _currentRotation;

    private float _currentScale = 1f;
    private float _targetScale = 1f;
    private Vector3 _savedVelocity;
    private Vector3 _savedContactNormal;
    private bool _ground = false;
    private bool _inverted;

    void LateUpdate() {

        if (!_ground) {
            // поворачиваем Rotatable вслед за скоростью
            _targetRotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.forward);
            // Зависимость масштаба от скорости движения
            float velocity = _rigidbody.velocity.magnitude;
            _targetScale = 1f + velocity * velocity * StretchMultiplier;
            _targetScale = Mathf.Clamp(_targetScale, 1f, 2f);
        }

        _currentScale = Mathf.Lerp(_currentScale, _targetScale, Time.deltaTime * ScaleChangeRate);
        
        SquashScale(_currentScale);

        if (!_inverted && _currentScale >= 1f) {
            _inverted = true;
            _rotatableTransform.rotation = _targetRotation = _currentRotation = Quaternion.LookRotation(_savedContactNormal, Vector3.forward);
        }
        
        _currentRotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * 10f);
        _rotatableTransform.rotation = _currentRotation;
        
    }

    // Масштабирование с искожением
    void SquashScale(float value) {
        if (value == 0f) return;
        _scalableTransform.localScale = new Vector3(1/value, value, 1/value);
    }
    
    public void OnCollisionEnter(Collision collision) {

        if (_ground) return;
        _ground = true;

        _savedVelocity = _rigidbody.velocity;
        _savedContactNormal = collision.contacts[0].normal;
        _rigidbody.isKinematic = true;

        _targetRotation = Quaternion.LookRotation(-collision.contacts[0].normal, Vector3.forward);

        _targetScale = Mathf.Lerp(1f, 0.3f, _savedVelocity.magnitude * SquashMultiplier);

        float velocityProjectionMagnitude = Vector3.Project(_savedVelocity, -_savedContactNormal).magnitude;
        float groundedTime = velocityProjectionMagnitude * DelayMultiplier;
        groundedTime = Mathf.Clamp(groundedTime, 0f, 0.15f);

        // перемещение объекта в точку контакта
        transform.position = collision.contacts[0].point + collision.contacts[0].normal * 0.5f;

        Invoke("StartToStretch", groundedTime);
        Invoke("DisableIsKinematic", groundedTime * 1.5f);
        
    }

    void StartToStretch() {
        _targetScale = Mathf.Lerp(0.5f, 1f, 1f + _savedVelocity.magnitude * StretchMultiplier);
        _inverted = false;
    }

    void DisableIsKinematic() {
        _rigidbody.isKinematic = false;
        Invoke("ExitSaveMode", 0.02f);
    }

    void ExitSaveMode() {
        _ground = false;
        _rigidbody.velocity = Vector3.Reflect(_savedVelocity, _savedContactNormal);
    }
    
}
