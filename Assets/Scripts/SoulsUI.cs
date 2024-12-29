using UnityEngine;
using TMPro;

public class SoulsUI : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _soulsText;
    private int _currentSouls = 0;

    public void UpdateSouls(int newAmount)
    {
        _currentSouls = newAmount;
        _soulsText.text = _currentSouls.ToString();
    }
}