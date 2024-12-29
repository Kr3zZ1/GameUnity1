using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraHealthPotion", menuName = "Items/Extra Health Potion")]
public class ExtraHealthPotion : Item
{
    [Header("Extra Health Potion Properties")]
    [SerializeField] private float _healthMiltiplier;

    public override void ApplyEffect(GameObject target)
    {
        var playerHealth = target.GetComponent<PlayerHealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.ExtraHealth(_healthMiltiplier);
            Debug.Log($" HP increased by {_healthMiltiplier}");
        }
    }
}