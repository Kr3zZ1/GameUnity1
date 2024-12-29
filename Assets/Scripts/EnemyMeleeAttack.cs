using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackRadius;

    private float _lastAttackTime;
    private Transform _player;
    private Animator _animator;
    private bool _isAttacking;
    private EnemyChase _enemyChase;

    public float AttackRadius => _attackRadius;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _enemyChase = GetComponent<EnemyChase>();
    }

    private void Update()
    {
        if (!_isAttacking && CanAttack(_player))
            StartCoroutine(PerformAttack());
    }

    private bool CanAttack(Transform target)
    {
        return target != null
               && Vector3.Distance(transform.position, target.position) <= _enemyChase.AttackDistance
               && Time.time >= _lastAttackTime;
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true;
        _animator.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);

        _animator.SetBool("IsAttacking", false);
        _isAttacking = false;
    }

    private void MeleeAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _enemyChase.AttackDistance, LayerMask.GetMask("Player"));
        if (player != null)
        {
            var playerHealth = player.GetComponent<PlayerHealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_attackDamage);
                _lastAttackTime = Time.time + _attackCooldown;
            }
        }
    }
}