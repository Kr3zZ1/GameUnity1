using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmallHealthPotion", menuName = "Items/Small Health Potion")]
public class SmallHealthPotion : Item
{
    [Header("Health Potion Properties")]
    [SerializeField] private float _healAmount;

    public override void ApplyEffect(GameObject target)
    {
        var playerHealth = target.GetComponent<PlayerHealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.Heal(_healAmount);
            Debug.Log($"Healed {_healAmount} HP.");
        }
    }
}