using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private Transform _patrolPoint;

    private bool _movingRight = true;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        if (_patrolPoint == null)
        {
            _patrolPoint = new GameObject("PatrolCenter").transform;
            _patrolPoint.position = transform.position;
        }
    }

    public void Patrol()
    {
        if (transform.position.x > _patrolPoint.position.x + _patrolRadius)
            _movingRight = false;
        else if (transform.position.x < _patrolPoint.position.x - _patrolRadius)
            _movingRight = true;

        float moveDirection = _movingRight ? 1f : -1f;
        _rigidbody2D.velocity = new Vector2(moveDirection * _moveSpeed, _rigidbody2D.velocity.y);

        if (Mathf.Abs(moveDirection) > 0.1f)
            transform.localScale = new Vector3(Mathf.Sign(moveDirection), 1, 1);

        UpdateAnimator();
    }

    public void ReturnToPatrolPoint()
    {
        Vector2 direction = (_patrolPoint.position - transform.position).normalized;
        _rigidbody2D.velocity = direction * _moveSpeed;

        if (Mathf.Abs(direction.x) > 0.1f)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

        UpdateAnimator();
    }

    public bool IsAtPatrolPoint()
    {
        return Vector2.Distance(transform.position, _patrolPoint.position) < 0.1f;
    }

    public bool ReachedPatrolBoundary()
    {
        return transform.position.x > _patrolPoint.position.x + _patrolRadius ||
               transform.position.x < _patrolPoint.position.x - _patrolRadius;
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("IsMoving", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_patrolPoint != null ? _patrolPoint.position : transform.position, _patrolRadius);
    }
}