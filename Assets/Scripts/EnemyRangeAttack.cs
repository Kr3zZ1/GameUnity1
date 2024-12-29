using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{
    [Header("Range Attack Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _attackCooldown;

    private Transform _player;
    private Animator _animator;
    private EnemyChase _enemyChase;
    private bool _isAttacking;
    private float _attackCooldownTimer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _enemyChase = GetComponent<EnemyChase>();
        _attackCooldownTimer = _attackCooldown;
    }

    private void Update()
    {
        if (!_isAttacking && CanShoot(_player) && _attackCooldownTimer <= 0)
            StartCoroutine(PerformTangeAttack());
        else
            _attackCooldownTimer -= Time.deltaTime;
    }

    private bool CanShoot(Transform target)
    {
        return target != null
            && Vector2.Distance(transform.position, _player.position) <= _enemyChase.AttackDistance;
    }

    private IEnumerator PerformTangeAttack()
    {
        _isAttacking = true;
        _animator.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);

        _animator.SetBool("IsAttacking", false);
        _isAttacking = false;
        _attackCooldownTimer = _attackCooldown;
    }

    public void SpawnProjectile()
    {
        Vector2 direction = (_player.position.x - transform.position.x) > 0 ? Vector2.right : Vector2.left;

        transform.localScale = new Vector3(Mathf.Sign(direction.x), transform.localScale.y, transform.localScale.z);

        GameObject bullet = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(direction * _projectileSpeed, ForceMode2D.Impulse);

        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
            projectile.SetProjectileDamage(_damage);

        bullet.transform.localScale = new Vector3(Mathf.Sign(direction.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
    }
}