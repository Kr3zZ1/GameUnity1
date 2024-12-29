using System;
using UnityEngine;

[RequireComponent(typeof(EnemyPatrol))]
[RequireComponent(typeof(EnemyChase))]
public class EnemyController : MonoBehaviour
{
    private enum EnemyState { Idle, Patrolling, Chasing, Returning }
    private EnemyState _currentState = EnemyState.Patrolling;

    private EnemyPatrol _patrol;
    private EnemyChase _chase;
    private Animator _animator;

    private float _idleTimer;
    [SerializeField] private float _idleDuration;

    private void Awake()
    {
        _patrol = GetComponent<EnemyPatrol>();
        _chase = GetComponent<EnemyChase>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                _animator.SetBool("IsMoving", false);
                break;

            case EnemyState.Patrolling:
                _patrol.Patrol();
                if (_patrol.ReachedPatrolBoundary())
                    SwitchToIdle();
                else if (_chase.IsPlayerWithinRadius(_chase.StartChasingRadius))
                    _currentState = EnemyState.Chasing;
                break;

            case EnemyState.Chasing:
                _chase.Chase();

                if (!_chase.IsPlayerWithinRadius(_chase.EndChasingRadius))
                    _currentState = EnemyState.Returning;
                break;

            case EnemyState.Returning:
                if (!_chase.IsPlayerWithinRadius(_chase.EndChasingRadius))
                    _patrol.ReturnToPatrolPoint();
                else
                    _currentState = EnemyState.Chasing;

                if (_patrol.IsAtPatrolPoint())
                    _currentState = EnemyState.Patrolling;
                break;
        }
    }

    private void HandleIdleState()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0f)
        {
            _currentState = EnemyState.Patrolling;
        }
    }

    private void SwitchToIdle()
    {
        _currentState = EnemyState.Idle;
        _idleTimer = _idleDuration;
    }
}