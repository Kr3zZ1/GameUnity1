using UnityEngine;

public class RulesMenu : MonoBehaviour
{
    public GameObject[] rules; // Массив объектов с правилами
    private int currentRuleIndex = 0;

    void Start()
    {
        UpdateRules();
    }

    public void ShowPreviousRule()
    {
        if (currentRuleIndex > 0)
        {
            currentRuleIndex--;
            UpdateRules();
        }
    }

    public void ShowNextRule()
    {
        if (currentRuleIndex < rules.Length - 1)
        {
            currentRuleIndex++;
            UpdateRules();
        }
    }

    private void UpdateRules()
    {
        for (int i = 0; i < rules.Length; i++)
        {
            rules[i].SetActive(i == currentRuleIndex);
        }
    }
}
