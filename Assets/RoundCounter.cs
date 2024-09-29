using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    [Header("Dynamic")]
    public int round;
    public Text roundText; // Reference to the UI Text component

    void Start() {
        round = 1; // Start at round 1
        UpdateRoundUI();
    }

    public void UpdateRoundUI() {
        if (roundText != null) {
            roundText.text = (round <= 4) ? "Round " + round : "Game Over";
        } else {
            Debug.LogError("Round Text UI is not assigned!");
        }
    }

    public void IncrementRound() {
        round += 1;
        UpdateRoundUI();
    }
}
