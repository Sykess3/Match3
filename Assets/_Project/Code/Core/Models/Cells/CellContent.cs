﻿using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure;
using UnityEngine;

public class CellContent : IModel
{
    private bool _isFalling;
    private readonly ICellContentConfig _config;
    private Vector2 _position;

    public virtual IEnumerable<ContentType> MatchableContent => _config.MatchableContent;

    public virtual bool Switchable => _config.Switchable;

    public virtual ContentType Type => _config.ContentType;

    public event Action StartedMovement;

    public event Action PositionChanged;

    public event Action Matched;

    public bool IsFalling
    {
        get => _isFalling;
        set
        {
            _isFalling = value;
            if (_isFalling) 
                StartedMovement?.Invoke();

        }
    }

    public Vector2 Position
    {
        get => _position;
        set => ChangePosition(value);
    }

    public CellContent(ICellContentConfig config)
    {
        _config = config;
    }

    private void ChangePosition(Vector2 value)
    {
        _position = value;
        PositionChanged?.Invoke();
    }

    public void Match() => Matched?.Invoke();
}

public enum ContentType
{
    Empty,
    Red,
    Blue,
    Orange,
    Purple,
    Green,
    Yellow
}