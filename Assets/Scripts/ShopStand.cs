using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopStand : MonoBehaviour
{
    [Header("Stand Properties")]
    [SerializeField] private Transform _displayPoint;
    [SerializeField] private SpriteRenderer _itemSpriteRenderer;

    [Header("UI Elements")]
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;
    [SerializeField] private GameObject _interactButton;

    private Item _assignedItem;
    private GameObject _assignedPrefab;
    private bool _isPlayerInRange = false;
    private GameObject _player;

    public Transform DisplayPoint => _displayPoint;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        HideItemInfo();
        _interactButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            ShowItemInfo();
            _interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            HideItemInfo();
            _interactButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PurchaseItem(_player);
        }
    }

    public void AssignItem(Item item, GameObject prefab)
    {
        _assignedItem = item;
        _assignedPrefab = prefab;
        _itemSpriteRenderer.sprite = item.ItemSprite;

        if (_assignedItem != null)
        {
            UpdateItemInfo(_assignedItem.ItemName, _assignedItem.ItemDescription, _assignedItem.ItemPrice);
        }
    }

    public void PurchaseItem(GameObject player)
    {
        if (_assignedItem == null) return;

        PlayerSouls playerSouls = player.GetComponent<PlayerSouls>();
        if (playerSouls != null && playerSouls.GetSouls() >= _assignedItem.ItemPrice)
        {
            playerSouls.SpendSouls(_assignedItem.ItemPrice);
            StartCoroutine(ThrowItem());
        }
        else
        {
            Debug.Log("Not enough souls!");
        }
    }

    private IEnumerator ThrowItem()
    {
        yield return new WaitForSeconds(0.2f);

        if (_assignedPrefab != null)
        {
            GameObject droppedItem = Instantiate(_assignedPrefab, _displayPoint.position, Quaternion.identity);

            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            var collectible = droppedItem.GetComponent<CollectibleItem>();

            if (collectible != null)
                collectible.enabled = false;

            if (rb != null)
            {
                Vector2 randomDirection = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(2.5f, 3.5f)).normalized;
                rb.AddForce(randomDirection * Random.Range(2f, 4f), ForceMode2D.Impulse);
            }

            _assignedItem = null;
            _assignedPrefab = null;
            _itemSpriteRenderer.sprite = null;
            _itemInfoPanel = null;
            _itemNameText.text = string.Empty;
            _itemDescriptionText.text = string.Empty;
            _itemPriceText.text = string.Empty;

            yield return new WaitForSeconds(0.5f);

            if (collectible != null)
                collectible.enabled = true;

            HideItemInfo();
        }
    }

    private void UpdateItemInfo(string name, string description, int price)
    {
        if (_itemInfoPanel != null)
        {
            _itemNameText.text = name;
            _itemDescriptionText.text = description;
            _itemPriceText.text = $"<color=red>{price}</color>";
        }
    }

    private void ShowItemInfo()
    {
        if (_itemInfoPanel != null)
        {
            _itemInfoPanel.SetActive(true);
        }
    }

    private void HideItemInfo()
    {
        if (_itemInfoPanel != null)
        {
            _itemInfoPanel.SetActive(false);
        }
    }
}