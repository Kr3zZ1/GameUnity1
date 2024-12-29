using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Item Settings")]
    public Item item;

    [Header("Follow Settings")]
    [SerializeField] private float _activateRadius;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _collectDistance;

    private Transform _player;
    private bool _isFollowingPlayer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        if (!_isFollowingPlayer && Vector2.Distance(transform.position, _player.position) <= _activateRadius)
            _isFollowingPlayer = true;

        if (_isFollowingPlayer)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * _followSpeed * Time.deltaTime;

            RotateTowardsPlayer(direction);

            if (Vector2.Distance(transform.position, _player.position) <= _collectDistance)
                CollectItem();
        }
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void CollectItem()
    {
        var inventory = _player.GetComponent<Inventory>();
        if (inventory != null)
            inventory.AddItem(item);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _activateRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _collectDistance);
    }
}