using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(TakingDamageEffect))]
public class PlayerHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealthPoints;
    [SerializeField] private float _currentHealthPoints;

    private bool _isRegenerating = false;
    private Coroutine _regenCorutine;
    private bool _isDead = false;

    [Header("UI Settings")]
    [SerializeField] private ProgressBar _healthProgressBar;

    private Animator _animator;
    private PlayerController _playerController;
    private TakingDamageEffect _damageEffect;

    public delegate void PlayerDeathHendler();
    public event PlayerDeathHendler OnPlayerDeath;

    public float MaxHealthPoints => _maxHealthPoints;
    public float CurrentHealthPoints => _currentHealthPoints;

    private void Awake()
    {
        _currentHealthPoints = _maxHealthPoints;
    }

    void Start()
    {
        _healthProgressBar.SetMaxValue(_maxHealthPoints);
        _healthProgressBar.SetCurrentValue(_currentHealthPoints);
        _playerController = GetComponent<PlayerController>();
        _damageEffect = GetComponent<TakingDamageEffect>();

        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (_playerController.IsInvincible) return;

        _currentHealthPoints -= damage;
        _damageEffect.ShowDamageEffect(0.4f);

        _currentHealthPoints = Mathf.Clamp(_currentHealthPoints, 0, _maxHealthPoints);
        _healthProgressBar.SetCurrentValue(_currentHealthPoints);

        if (_currentHealthPoints <= 0) { Die(); }
    }

    public void Heal(float amount)
    {
        _currentHealthPoints += amount;
        _currentHealthPoints = Mathf.Clamp(_currentHealthPoints, 0, _maxHealthPoints);
        _healthProgressBar.SetCurrentValue(_currentHealthPoints);
    }

    private void Die()
    {
        Debug.Log("Player died");
        OnPlayerDeath?.Invoke();

        _animator.SetBool("IsDied", true);
        GetComponent<PlayerController>().enabled = false;
        StartCoroutine(WaithForDeathAnim());

        _isDead = true;
        UIManager.Instance.ShowDeathScreen();
    }

    private IEnumerator WaithForDeathAnim()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        CreateCorpse();
        Destroy(gameObject);
    }

    private void CreateCorpse()
    {
        GameObject corpse = new GameObject("PlayerCorpse");
        corpse.transform.position = transform.position;
        corpse.transform.localScale = transform.localScale;

        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        corpseRenderer.sprite = enemyRenderer.sprite;
        corpseRenderer.sortingOrder = 1;
        corpseRenderer.flipX = enemyRenderer.flipX;

        Destroy(corpse, 5f);
    }

    public void Respawn()
    {
        Vector3 respawnPosition = GameManager.Instance.GetCheckpointPosition();
        transform.position = respawnPosition;

        _maxHealthPoints = GameManager.Instance.GetSavedMaxHealth();
        _healthProgressBar.SetMaxValue(_maxHealthPoints);

        _currentHealthPoints = GameManager.Instance.GetSavedCurrentHealth();
        _healthProgressBar.SetCurrentValue(_currentHealthPoints);

        PlayerSouls playerSouls = GetComponent<PlayerSouls>();
        playerSouls.SetSouls(GameManager.Instance.GetSavedSoulsCount());

        Debug.Log("Player respawned");
        _isDead = false;
        UIManager.Instance.HideDeathScreen();
    }

    public void ExtraHealth(float multiplier)
    {
        float newMaxHealth = _maxHealthPoints * multiplier;
        float healthIncrease = newMaxHealth - _maxHealthPoints;
        _maxHealthPoints = newMaxHealth;
        _currentHealthPoints = Mathf.Min(_currentHealthPoints + healthIncrease, _maxHealthPoints);
    }

    public void HealthRegeneration(float amountPerSecond, float effectDuration)
    {
        if (_isRegenerating)
            StopCoroutine(_regenCorutine);
        _regenCorutine = StartCoroutine(RegenerateHealth(amountPerSecond, effectDuration));
    }

    private IEnumerator RegenerateHealth(float amountPerSecond, float duration)
    {
        _isRegenerating = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Heal(amountPerSecond * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isRegenerating = false;
    }
}