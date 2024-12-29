using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    [SerializeField] private float _openRadius;
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private int _minItems;
    [SerializeField] private int _maxItems;
    [SerializeField] private GameObject[] _lootPool;

    [Header("UI Settings")]
    [SerializeField] private GameObject _interactButton;

    private Animator _animator;
    private bool _isOpened = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _interactButton.SetActive(false);
    }

    private void Update()
    {
        if (_isOpened) return;

        if (Input.GetKeyDown(KeyCode.F) && Vector2.Distance(transform.position, PlayerPosition()) <= _openRadius)
            OpenChest();
    }

    private void OpenChest()
    {
        _isOpened = true;
        _animator.SetBool("IsOpened", true);
        StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenWoodChestAnim") ||
               _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        _animator.SetBool("IsFinishToOpen", true);
        StartCoroutine(DropLoot());
    }


    private IEnumerator DropLoot()
    {
        int itemCount = Random.Range(_minItems, _maxItems + 1);

        for (int i = 0; i < itemCount; i++)
        {
            var loot = Instantiate(_lootPool[Random.Range(0, _lootPool.Length)], _dropPoint.position, Quaternion.identity);
            Rigidbody2D rb = loot.GetComponent<Rigidbody2D>();
            var collectible = loot.GetComponent<CollectibleItem>();

            if (collectible != null)
                collectible.enabled = false;

            if (rb != null)
            {
                Vector2 randomDirection = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(2.5f, 3.5f)).normalized;
                rb.AddForce(randomDirection * Random.Range(2f, 4f), ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

        foreach (var loot in GameObject.FindGameObjectsWithTag("Collectible Item"))
        {
            var collectible = loot.GetComponent<CollectibleItem>();
            if (collectible != null) 
                collectible.enabled = true;
        }
    }

    private Vector3 PlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            _interactButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _interactButton.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _openRadius);
    }
}