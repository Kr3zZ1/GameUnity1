using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyChase : MonoBehaviour
{
    [Header("Chasing Settings")]
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _startChasingRadius;
    [SerializeField] private float _endChasingRadius;
    [SerializeField] private float _attackDistance; //Минимальное расстояние до игрока
    [SerializeField] private float _nextWaypointDistance = 1f;

    private Transform _player;
    private Rigidbody2D _rigidbody2D;
    private Seeker _seeker;
    private Animator _animator;
    private EnemyMeleeAttack _enemyMeleeAttack;

    private Path _path;
    private int _currentWaypoint = 0;

    public float StartChasingRadius => _startChasingRadius;
    public float EndChasingRadius => _endChasingRadius;
    public float AttackDistance => _attackDistance;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();
        _animator = GetComponent<Animator>();
        _enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (_player == null || !IsPlayerWithinRadius(_startChasingRadius) || !_seeker.IsDone())
            return;

        _seeker.StartPath(_rigidbody2D.position, _player.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    public void Chase()
    {
        if (_player == null || _path == null)
            return;

        //Если враг близко к игроку, остановиться
        float playerDistance = Vector2.Distance(transform.position, _player.position);
        if (playerDistance <= _attackDistance)
        {
            StopChasing();
            return;
        }

        // Следование по пути
        if (_currentWaypoint >= _path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
        Vector2 force = direction * _chaseSpeed;

        _rigidbody2D.velocity = new Vector2(force.x, _rigidbody2D.velocity.y);

        float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }

        //Обновляем направление спрайта
        if (direction.x > 0.05f)
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        else if (direction.x < -0.05f)
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        _animator.SetBool("IsMoving", true);
    }

    public bool IsPlayerWithinRadius(float radius)
    {
        if (_player == null) return false;
        return Vector2.Distance(transform.position, _player.position) <= radius;
    }

    public void StopChasing()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetBool("IsMoving", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _startChasingRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _endChasingRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}