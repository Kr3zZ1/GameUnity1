using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Item, int> _items = new Dictionary<Item, int>();
    private List<Item> _itemList = new List<Item>();
    private int _currentPotionIndex = 0;
    private int _itemCount = 0;

    public event Action OnInventoryChanged;
    public int ItemCount => _itemCount;
    public Dictionary<Item, int> Items => _items;

    public void AddItem(Item item)
    {
        if (_items.ContainsKey(item))
            _items[item]++;
        else
        {
            _items[item] = 1;
            _itemList.Add(item);
            _itemCount++;
        }    
        Debug.Log($"Added {item.ItemName}. Total: {_items[item]}");

        if (item.Type == ItemType.AutoUse)
        {
            item.ApplyEffect(GameObject.FindGameObjectWithTag("Player"));
            UsePotion(item);
        }

        OnInventoryChanged?.Invoke();
    }

    public int GetItemCount(Item item)
    {
        return _items.ContainsKey(item) ? _items[item] : 0;
    }

    public int GetCurrentIndex()
    {
        return _currentPotionIndex;
    }

    public Item GetPotionAt(int index)
    {
        if (index < 0 || index >= _itemList.Count) return null;
        return _itemList[index];
    }

    public void SelectNextPotion()
    {
        if (_itemList.Count == 0) return;

        _currentPotionIndex = (_currentPotionIndex + 1) % _itemList.Count; // Циклический сдвиг вперёд
        OnInventoryChanged?.Invoke();
    }

    public void SelectPreviousPotion()
    {
        if (_itemList.Count == 0) return;

        _currentPotionIndex = (_currentPotionIndex - 1 + _itemList.Count) % _itemList.Count; // Циклический сдвиг назад
        OnInventoryChanged?.Invoke();
    }

    public void UsePotion(Item item)
    {
        if (!_items.ContainsKey(item)) return;

        _items[item]--;

        if (_items[item] <= 0)
        {
            _items.Remove(item);
            _itemList.Remove(item);
            _itemCount--;

            _currentPotionIndex = Mathf.Clamp(_currentPotionIndex, 0, _itemList.Count - 1);
        }

        OnInventoryChanged?.Invoke();
    }

    public void ClearInventory()
    {
        _items.Clear();
        _itemList.Clear();
        _itemCount = 0;
        _currentPotionIndex = 0;
        OnInventoryChanged?.Invoke();
    }

    public void SetCurrentPotionIndex(int index)
    {
        _currentPotionIndex = Mathf.Clamp(index, 0, _itemList.Count - 1);
        OnInventoryChanged?.Invoke();
    }
}