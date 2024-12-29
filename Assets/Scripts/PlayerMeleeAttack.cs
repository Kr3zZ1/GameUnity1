using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float _lightAttackCooldown;
    [SerializeField] private float _heavyAttackCooldown;
    [SerializeField] private float _comboHoldTime;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyLayers;
    private float _lastLightAttackTime;
    private float _lastHeavyAttackTime;
    private float _lightAttackHoldTime;
    private bool _isHoldingLightAttack;
    private bool _isAttacking = false;

    [Header("Damage Settings")]
    [SerializeField] private float _lightAttackDamage;
    [SerializeField] private float _heavyAttackDamage;
    [SerializeField] private float _comboAttackDamage;

    [Header("Other Settings")]
    [SerializeField] private Rigidbody2D _player;

    private Animator _animator;
    private bool _isLightAttackDamageIncreased = false;
    private Coroutine _meleeAttackCorutine;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isHoldingLightAttack = true;
            _lightAttackHoldTime = 0f;
        }

        if (Input.GetMouseButton(0) && _isHoldingLightAttack)
            _lightAttackHoldTime += Time.deltaTime;

        if (Input.GetMouseButtonUp(0) && _isHoldingLightAttack)
        {
            _isHoldingLightAttack = false;

            if (_lightAttackHoldTime >= _comboHoldTime && Time.time >= _lastLightAttackTime + _lightAttackCooldown)
                PerformComboAttack();
            else if (Time.time >= _lastLightAttackTime + _lightAttackCooldown)
                PerformLightAttack();
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= _lastHeavyAttackTime + _heavyAttackCooldown)
            PerformHeavyAttack();
    }

    private void PerformLightAttack()
    {
        _player.velocity = new Vector2(0, _player.velocity.y);
        _lastLightAttackTime = Time.time;
        _animator.SetBool("LightAttack", true);
        GetComponent<PlayerController>().enabled = false;
        DealDamage(_lightAttackDamage);
        Invoke(nameof(ResetAttackAnimations), GetAnimationClipLenght("LightAttackKnightAnim"));
    }

    private void PerformHeavyAttack()
    {
        _player.velocity = new Vector2(0, _player.velocity.y);
        _lastHeavyAttackTime = Time.time;
        _animator.SetBool("HeavyAttack", true);
        GetComponent<PlayerController>().enabled = false;
        DealDamage(_heavyAttackDamage);
        Invoke(nameof(ResetAttackAnimations), GetAnimationClipLenght("HeavyAttackKnightAnim"));
    }

    private void PerformComboAttack()
    {
        _player.velocity = new Vector2(0, _player.velocity.y);
        _lastLightAttackTime = Time.time;
        _animator.SetBool("ComboAttack", true);
        GetComponent<PlayerController>().enabled = false;
        DealDamage(_comboAttackDamage);
        Invoke(nameof(ResetAttackAnimations), GetAnimationClipLenght("ComboAttackKnightAnim"));
    }

    private void DealDamage(float damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealthSystem enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
            if (enemyHealthSystem != null)
            {
                enemyHealthSystem.TakeDamage(damage);
            }
        }
    }

    private void ResetAttackAnimations()
    {
        _animator.SetBool("LightAttack", false);
        _animator.SetBool("HeavyAttack", false);
        _animator.SetBool("ComboAttack", false);
        GetComponent<PlayerController>().enabled = true;
    }

    private float GetAnimationClipLenght(string animatiomName)
    {
        foreach (var clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animatiomName)
                return clip.length;   
        }
        return 0f;
    }

    private void Friction(bool argument)
    {
        _player.sharedMaterial.friction = argument ? 1.0f : 0.0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    public void IncreaseLightMeleeAttack(float damageMultiplier, float effectDuration)
    {
        if (_meleeAttackCorutine != null)
            StopCoroutine(_meleeAttackCorutine);

        _meleeAttackCorutine = StartCoroutine(IncreaseLightMelee(damageMultiplier, effectDuration));
    }

    private IEnumerator IncreaseLightMelee(float multiplier, float duration)
    {
        _isLightAttackDamageIncreased = true;
        float originalLightAttackDamage = _lightAttackDamage; //Сохраняем оригинальное значение
        _lightAttackDamage *= multiplier;

        yield return new WaitForSeconds(duration);

        _lightAttackDamage = originalLightAttackDamage; //Восстанавливаем урон после окончания действия эффекта
        _isLightAttackDamageIncreased = false;
    }
}