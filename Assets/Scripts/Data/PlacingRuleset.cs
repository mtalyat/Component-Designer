using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class PlacingRuleset
{
    [SaveThis]
    [EditableProperty]
    public Vector2 offsetDimensions { get; set; } = new Vector2(2, 1);
    [SaveThis]
    [EditableProperty]
    public Vector2 defaultOffset { get; set; } = new Vector2(0, 0);
    [SaveThis]
    [EditableProperty]
    public Vector2 offsetScale { get; set; } = new Vector2(1.0f, 1.0f);
    [SaveThis]
    [EditableProperty]
    public Vector2 cornerMidpoint { get; set; } = new Vector2(0.0f, 0.0f);
    [SaveThis]
    [EditableProperty]
    public Vector2 gridPlacingDimensions { get; set; } = new Vector2(1, 1);
    [SaveThis]
    [EditableProperty]
    public bool allowWorldRotation { get; set; } = true;
    [SaveThis]
    [EditableProperty]
    public bool allowFineRotation { get; set; } = true;
    [SaveThis]
    [EditableProperty]
    public bool canBeFlipped { get; set; } = false;
    [SaveThis]
    [EditableProperty]
    public float flippingPointHeight { get; set; } = -0.25f;
    [SaveThis]
    [EditableProperty]
    public bool gridPositionsAreRelative { get; set; } = false;
    [SaveThis]
    [EditableProperty]
    public bool enableEdgeExtensions { get; set; } = false;
    [SaveThis]
    [EditableProperty]
    public List<Vector2> primaryGridPositions { get; set; } = new List<Vector2>(new Vector2[] { new Vector2(0.5f, 0.5f) });
    [SaveThis]
    [EditableProperty]
    public List<Vector2> secondaryGridPositions { get; set; } = new List<Vector2>();
    [SaveThis]
    [EditableProperty]
    public List<Vector2> primaryEdgePositions { get; set; } = new List<Vector2>();
    [SaveThis]
    [EditableProperty]
    public List<Vector2> secondaryEdgePositions { get; set; } = new List<Vector2>();

    public PlacingRuleset()
    {

    }
}