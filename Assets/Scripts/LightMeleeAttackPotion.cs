using UnityEngine;

[CreateAssetMenu(fileName = "LightMeleeAttackPotion", menuName = "Items/Light Melee Attack Potion")]
public class LightMeleeAttackPotion : Item
{
    [Header("Light Melee Attack Potion Properties")]
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _duration;

    public override void ApplyEffect(GameObject target)
    {
        var playerMelee = target.GetComponent<PlayerMeleeAttack>();
        if (playerMelee != null)
        {
            playerMelee.IncreaseLightMeleeAttack(_damageMultiplier, _duration);
            Debug.Log($"Light melee attack damage increased by {_damageMultiplier} for {_duration} seconds.");
        }
    }
}