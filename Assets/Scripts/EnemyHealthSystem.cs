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
        //Ожидаем, пока текущая анимация перейдёт в состояние с тегом "Death"
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null; //Ждём следующий кадр
        }
        while (!stateInfo.IsTag("Death"));

        //Получаем текущую информацию о клипе
        var clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float clipLength = clipInfo[0].clip.length / _animator.speed; //Учитываем скорость анимации
            Debug.Log($"Waiting for clip: {clipInfo[0].clip.name}, Length: {clipLength}");
            yield return new WaitForSeconds(clipLength-0.2f); //Ждём завершения анимации
        }
        else
        {
            Debug.LogWarning("No clip info available!");
        }

        GetComponent<SoulDrop>()?.DropSouls();

        // Создаём тело врага
        CreateCorpse();

        Destroy(gameObject);
    }

    private void CreateCorpse()
    {
        //Создаем новый объект тела
        GameObject corpse = new GameObject("EnemyCorpse");
        corpse.transform.position = transform.position;
        corpse.transform.localScale = transform.localScale;

        //Добавляем спрайт тела
        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        corpseRenderer.sprite = enemyRenderer.sprite; //Последний кадр анимации
        corpseRenderer.sortingOrder = 1;
        corpseRenderer.flipX = enemyRenderer.flipX;

        //Добавляем таймер уничтожения тела через несколько секунд
        Destroy(corpse, 5f); //Тело исчезнет через 5 секунд
    }
}