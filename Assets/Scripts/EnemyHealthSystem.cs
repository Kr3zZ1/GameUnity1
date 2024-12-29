using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(TakingDamageEffect))]
public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    private TakingDamageEffect _damageEffect;
    private Animator _animator;
    public event Action OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
        _damageEffect = GetComponent<TakingDamageEffect>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _damageEffect.ShowDamageEffect(0.4f);
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }

    private void Die()
    {
        GetComponent<EnemyPatrol>().enabled = false;
        GetComponent<EnemyChase>().enabled = false;

        OnDeath?.Invoke();
        _animator.SetBool("IsDied", true);

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        //�������, ���� ������� �������� ������� � ��������� � ����� "Death"
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null; //��� ��������� ����
        }
        while (!stateInfo.IsTag("Death"));

        //�������� ������� ���������� � �����
        var clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float clipLength = clipInfo[0].clip.length / _animator.speed; //��������� �������� ��������
            Debug.Log($"Waiting for clip: {clipInfo[0].clip.name}, Length: {clipLength}");
            yield return new WaitForSeconds(clipLength-0.2f); //��� ���������� ��������
        }
        else
        {
            Debug.LogWarning("No clip info available!");
        }

        GetComponent<SoulDrop>()?.DropSouls();

        // ������ ���� �����
        CreateCorpse();

        Destroy(gameObject);
    }

    private void CreateCorpse()
    {
        //������� ����� ������ ����
        GameObject corpse = new GameObject("EnemyCorpse");
        corpse.transform.position = transform.position;
        corpse.transform.localScale = transform.localScale;

        //��������� ������ ����
        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        corpseRenderer.sprite = enemyRenderer.sprite; //��������� ���� ��������
        corpseRenderer.sortingOrder = 1;
        corpseRenderer.flipX = enemyRenderer.flipX;

        //��������� ������ ����������� ���� ����� ��������� ������
        Destroy(corpse, 5f); //���� �������� ����� 5 ������
    }
}