using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTantrumRegion", menuName = "RegulatorsRegion/TantrumRegion")]
public class TantrumRegion : ScriptableObject
{
    public float activatePercentageLoBound = 0.0f;
    public float activatePercentageHiBound = 1.0f;
    public Color barColor = Color.white;
    public List<ChildBehaviorState> validBehaviors;
    public List<AudioClip> utterances;
}

[System.Serializable]
public struct ChildBehaviorState
{
    public ChildBehaviorEnum state;
    [Range(0f, 100f)]
    public int probability;
}

public enum ChildBehaviorEnum
{
    Null,
    Sitting,
    Standing,
    Fiddling,
    Sobbing,
    LayingDown,
    Walking,
    Crying,
    Kicking,
    Punching
}
