using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Usable,
    TemporaryEffect,
    AutoUse
}

public abstract class Item : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [SerializeField] private string _shortDescription;
    [SerializeField] private int _itemPrice;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private ItemType _itemType;

    public string ItemName => _itemName;
    public ItemType Type => _itemType;
    public Sprite ItemSprite => _itemSprite;
    public string ItemDescription => _itemDescription;
    public string ShortDescription => _shortDescription;
    public int ItemPrice => _itemPrice;
    public GameObject ItemPrefab => _itemPrefab;


    public abstract void ApplyEffect(GameObject target);
}