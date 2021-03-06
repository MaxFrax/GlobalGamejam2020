﻿using System;
using UnityEditor.U2D;
using UnityEngine;
using Random = UnityEngine.Random;

public class BugMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField, Range(0f, 3f), Tooltip("How big is the step each frame?")]
    private float speed = .1f;

    [SerializeField, Range(0f, 5f), Tooltip("How long does it take to change rotation?")]
    private float straightLine = 3f;

    private float _lastRotationUpdate;
    private float rotation;
    private Camera _camera;
    private bool _init;

    private void Reset()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _camera = Camera.main;
        _lastRotationUpdate = Random.Range(0f, straightLine);
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - _lastRotationUpdate > straightLine || !_init)
        {
            rotation = Random.Range(0f, 360f);
            _rigidbody2D.SetRotation(rotation);
            _lastRotationUpdate = Time.timeSinceLevelLoad;
            _init = true;
        }

        Vector2 newPosition = _rigidbody2D.position + (Vector2) transform.up * speed;
        var screenPosition = _camera.WorldToViewportPoint(newPosition);
        screenPosition.x = screenPosition.x > 0.925f ? screenPosition.x - 0.925f : screenPosition.x;
        screenPosition.y = screenPosition.y > 1 ? screenPosition.y - 1 : screenPosition.y;
        screenPosition.x = screenPosition.x < 0 ? screenPosition.x + 0.925f : screenPosition.x;
        screenPosition.y = screenPosition.y < 0 ? screenPosition.y + 1 : screenPosition.y;
        newPosition = _camera.ViewportToWorldPoint(screenPosition);
        _rigidbody2D.MovePosition(newPosition);
    }
}