using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PotionUI : MonoBehaviour
{
    [Header("UI Main References")]
    [SerializeField] private Image _centerPotionImage;
    [SerializeField] private Image _leftPotionImage;
    [SerializeField] private Image _rightPotionImage;
    [SerializeField] private Inventory _inventory;
    
    [Header("Text UI References")]
    [SerializeField] private TextMeshProUGUI _shortDescriprion;
    [SerializeField] private TextMeshProUGUI _currentPotionCount;
    [SerializeField] private TextMeshProUGUI _nextPotionCount;
    [SerializeField] private TextMeshProUGUI _previousPotionCount;

    private void Start()
    {
        _centerPotionImage.gameObject.SetActive(false);
        _rightPotionImage.gameObject.SetActive(false);
        _leftPotionImage.gameObject.SetActive(false);

        _inventory.OnInventoryChanged += UpdatePotionUI;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            _inventory.SelectPreviousPotion();

        if (Input.GetKeyDown(KeyCode.E))
            _inventory.SelectNextPotion();

        if (Input.GetKeyDown(KeyCode.H))
        {
            var potion = _inventory.GetPotionAt(_inventory.GetCurrentIndex()); // Получаем текущее зелье и его эффект
            if (potion != null)
            {
                potion.ApplyEffect(GameObject.FindGameObjectWithTag("Player"));
                _inventory.UsePotion(potion);
            }
        }
    }

    void UpdatePotionUI()
    {
        int itemCount = _inventory.ItemCount;

        if (itemCount == 0)
        {
            _centerPotionImage.gameObject.SetActive(false);
            _leftPotionImage.gameObject.SetActive(false);
            _rightPotionImage.gameObject.SetActive(false);
            _shortDescriprion.text = string.Empty;
            _currentPotionCount.text = string.Empty;
            _previousPotionCount.text = string.Empty;
            _nextPotionCount.text = string.Empty;
            return;
        }

        int currentIndex = _inventory.GetCurrentIndex();
        int previousIndex = (currentIndex - 1 + itemCount) % itemCount;
        int nextIndex = (currentIndex + 1) % itemCount;

        var currentPotion = _inventory.GetPotionAt(currentIndex);
        if (currentPotion != null)
        {
            _centerPotionImage.sprite = currentPotion.ItemSprite;
            _centerPotionImage.gameObject.SetActive(true);
            _shortDescriprion.text = currentPotion.ShortDescription;
            _currentPotionCount.text = _inventory.GetItemCount(currentPotion).ToString();
        }
        else
        {
            _centerPotionImage.gameObject.SetActive(false);
            _shortDescriprion.text = string.Empty;
            _currentPotionCount.text = string.Empty;
        }

        if (itemCount > 1)
        {
            var previousPotion = _inventory.GetPotionAt(previousIndex);
            if (previousPotion != null)
            {
                _leftPotionImage.sprite = previousPotion.ItemSprite;
                _leftPotionImage.gameObject.SetActive(true);
                _nextPotionCount.text = _inventory.GetItemCount(previousPotion).ToString();
            }
            else
            {
                _leftPotionImage.gameObject.SetActive(false);
                _nextPotionCount.text = string.Empty;
            }
                
        }
        else
        {
            _leftPotionImage.gameObject.SetActive(false);
            _nextPotionCount.text = string.Empty;
        }


        if (itemCount > 2)
        {
            var nextPotion = _inventory.GetPotionAt(nextIndex);
            if (nextPotion != null)
            {
                _rightPotionImage.sprite = nextPotion.ItemSprite;
                _rightPotionImage.gameObject.SetActive(true);
                _previousPotionCount.text = _inventory.GetItemCount(nextPotion).ToString();
            }
            else
            {
                _rightPotionImage.gameObject.SetActive(false);
                _previousPotionCount.text = string.Empty;
            }

        }
        else
        {
            _rightPotionImage.gameObject.SetActive(false);
            _previousPotionCount.text = string.Empty;
        }

    }
}