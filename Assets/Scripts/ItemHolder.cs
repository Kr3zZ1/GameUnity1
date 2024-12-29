using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Item _itemData;

    public Item ItemData => _itemData;
}