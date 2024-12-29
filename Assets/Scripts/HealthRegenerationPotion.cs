using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenerationPotion", menuName = "Items/Health Regeneration Potion")]
public class HealthRegenerationPotion : Item
{
    [Header("Health Regeneration Potion Properties")]
    [SerializeField] private float _healthPerSecond;
    [SerializeField] private float _duration;

    public override void ApplyEffect(GameObject target)
    {
        var playerHealth = target.GetComponent<PlayerHealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.HealthRegeneration(_healthPerSecond, _duration);
            Debug.Log($"Started health regeneration: {_healthPerSecond} HP per second for {_duration} seconds.");
        }
    }
}