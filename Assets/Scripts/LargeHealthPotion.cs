using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LargeHealthPotion", menuName = "Items/Large Health Potion")]
public class LargeHealthPotion : Item
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