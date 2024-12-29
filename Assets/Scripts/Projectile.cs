using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _liveTime;
    private float _projectileDamage;

    private void Start()
    {
        Destroy(gameObject, _liveTime);
    }

    public void SetProjectileDamage(float damage)
    {
        _projectileDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealthSystem>().TakeDamage(_projectileDamage);
            Destroy(gameObject);
        }
    }
}