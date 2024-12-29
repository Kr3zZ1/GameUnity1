using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    [Header("Soul Settings")]
    [SerializeField] private int _soulValue;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _activateRadius;

    private Transform _player;
    private bool _isFollowingPlayer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        if (!_isFollowingPlayer && Vector2.Distance(transform.position, _player.position) <= _activateRadius)
            _isFollowingPlayer = true;

        if (_isFollowingPlayer)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * _followSpeed * Time.deltaTime;
            
            RotateTowardsPlayer(direction);
        }  
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle+90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_isFollowingPlayer)
            {
                PlayerSouls playerSouls = collision.GetComponent<PlayerSouls>();
                if (playerSouls != null)
                {
                    playerSouls.AddSoul(_soulValue);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _activateRadius);
    }
}
