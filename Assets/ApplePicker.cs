using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplePicker : MonoBehaviour{
    [Header("Inscribed")]
    public GameObject basketPrefab;
    public int numBasket = 4;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
    
    // Reference to the RoundCounter component
    public RoundCounter roundCounter;
    public Button restartButton; // Reference to the restart button

    private bool gameActive = true; // Track if the game is active

    void Start(){
        // Find the RoundCounter component in the scene
        roundCounter = FindObjectOfType<RoundCounter>();
        if (roundCounter == null) {
            Debug.LogError("RoundCounter not found! Make sure there's a GameObject with the RoundCounter script in the scene.");
            return; // Exit if RoundCounter is not found
        }

        // Initialize the basket list and create baskets
        basketList = new List<GameObject>();
        for (int i = 0; i < numBasket; i++) {
            GameObject tBasketGO = Instantiate(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }

        // Hide the restart button initially
        if (restartButton != null) {
            restartButton.gameObject.SetActive(false);
        } else {
            Debug.LogError("Restart button is not assigned!");
        }
    }

    public void AppleMissed(){
        // Only proceed if the game is active
        if (!gameActive) return;

        // Destroy all apples tagged "Apple"
        GameObject[] appleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tempGO in appleArray) {
            Destroy(tempGO);
        }

        // Remove the last basket from the list if there are any left
        if (basketList.Count > 0) {
            int basketIndex = basketList.Count - 1;
            GameObject basketGo = basketList[basketIndex];
            basketList.RemoveAt(basketIndex);
            Destroy(basketGo);

            // Increase the round when a basket is destroyed
            if (roundCounter != null) {
                roundCounter.IncrementRound();

                // Check if the round has reached 5 (Game Over)
                if (roundCounter.round > 4) {
                    gameActive = false; // Stop the game
                    restartButton.gameObject.SetActive(true); // Show the restart button
                }
            } else {
                Debug.LogError("RoundCounter is not assigned!");
            }
        }

        // Reload the scene if all baskets are gone and the game is still active
        if (basketList.Count == 0 && gameActive) {
            SceneManager.LoadScene("_Scene_0");
        }
    }

    public void RestartGame(){
        Debug.Log("RestartGame method called");
        roundCounter.round = 1; // Reset to round 1
        roundCounter.UpdateRoundUI(); // Update the UI
        gameActive = true; // Set game as active

        // Clear baskets and restart the game
        foreach (GameObject basket in basketList) {
            Destroy(basket);
        }
        basketList.Clear();
        Start(); // Reinitialize baskets

        restartButton.gameObject.SetActive(false); // Hide the restart button again
    }
}
