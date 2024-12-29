using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private Transform _followingTarget;
    [SerializeField, Range(0f, 1f)] private float _parallaxStrength;
    [SerializeField] private bool _disableVerticalParallax;
    private Vector3 _targetPosition;

    void Start()
    {
        if (!_followingTarget)
            _followingTarget = Camera.main.transform;

        _targetPosition = _followingTarget.position;
    }

    void Update()
    {
        var delta = _targetPosition = _followingTarget.position - _targetPosition;

        if (_disableVerticalParallax)
            delta.y = 0;

        _targetPosition = _followingTarget.position;
        transform.position += delta * _parallaxStrength;
    }
}
