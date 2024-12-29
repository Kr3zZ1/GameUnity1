using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Teleport _linkedTeleport;
    [SerializeField] private bool _enableTeleport = true;
    private bool _playerInRange = false;

    [SerializeField] private GameObject _interactButton;

    private void Start()
    {
        _interactButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = true;
            _interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = false;
            _interactButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (_playerInRange && _enableTeleport && Input.GetKeyDown(KeyCode.F) && _linkedTeleport != null)
        {
            PerformTeleporting();
        }
    }

    private void PerformTeleporting()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = _linkedTeleport.transform.position;

            _linkedTeleport.DisableTeleportTemporarily();
        }
    }

    public void DisableTeleportTemporarily()
    {
        _enableTeleport = false;
        Invoke(nameof(EnableTeleport), 0.1f);
    }

    private void EnableTeleport()
    {
        _enableTeleport = true;
    }
}