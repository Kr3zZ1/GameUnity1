using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingDamageEffect : MonoBehaviour
{
    private Renderer _renderer;
    private float _effectTimer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (_effectTimer > 0)
        {
            _effectTimer -= Time.deltaTime;
            if (_effectTimer < 0)
                _renderer.material.color = Color.white;
        }
    }

    public void ShowDamageEffect(float duration)
    {
        _effectTimer = duration;
        _renderer.material.color = Color.red;
    }
}