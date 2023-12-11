using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildState : MonoBehaviour
{
    //Child state variables
    public float tantrumLevel = 0, attentionLevel = 0;
    public bool receivingAttention = false;
    public float tl;

    public void ChangeTantrumLevel(float amount)
    {
        tl = Mathf.Clamp(tantrumLevel + amount, 0, 100);
        //Debug.Log("tantrumLevel + amount is "+ tantrumLevel + amount + " tl is "+ tl + " tantrum level is "+ tantrumLevel + " amount passed is "+ amount);
        if (tl > 99.0f) { tantrumLevel = 100; }
        else if (tl < 1.0f) { tantrumLevel = 0; }
        else { tantrumLevel = tl; }
    }

    public void setStartTantrumLevel(int startTL) {
        tantrumLevel = startTL;
    }

    public void setStartAttentionLevel(int startAT)
    {
        attentionLevel = startAT;
    }
}
