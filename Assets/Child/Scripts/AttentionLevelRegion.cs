using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttentionRegion", menuName = "RegulatorsRegion/AttentionRegion")]
public class AttentionLevelRegion : ScriptableObject
{
    public float activatePercentageLoBound = 0.0f;
    public float activatePercentageHiBound = 1.0f;
    public float tantrumLevelIncreasePerSecond = 0.0f;
    public float tantrumLevelDecreasePerSecond = 0.0f;
    public float attentionLevelAddedPerSecond = 0.0f;
    public float attentionLevelLostPerSecond = 1.0f;
    public Color barColor = Color.white;
}
