using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Platformer.Mechanics
{
    public class TokenScoreController : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public float animationDuration = 0.5f;
        // Scale factor for the "pop" effect when the score updates.
        public float popScale = 1.5f;
        // The displayed score.
        private int currentScore = 0;
        private int targetScore = 0;     // The target score to reach.

        private Coroutine scoreAnimationCoroutine;
        private Vector3 originalScale;         // Store the original scale to prevent cumulative scaling.


        void Start()
        {
            // Store the initial scale of the text to use it as the reset value.
            originalScale = scoreText.transform.localScale;
            // Initialize the score display.
            UpdateScoreDisplay(currentScore);
        }


        /// <summary>
        /// Call this method to update the score and trigger the animation.
        /// </summary>
        /// <param name="newScore">The new score value to display.</param>
        public void IncrementScore()
        {
            targetScore++;

            // If a previous animation is running, stop it.
            if (scoreAnimationCoroutine != null)
            {
                StopCoroutine(scoreAnimationCoroutine);
            }

            // Start the new score animation.
            scoreAnimationCoroutine = StartCoroutine(AnimateScore());
        }

        /// <summary>
        /// Animates the score from the current value to the target value.
        /// </summary>
        private IEnumerator AnimateScore()
        {
            while (currentScore < targetScore)
            {
                // Reset the scale to the original size before starting the animation.
                scoreText.transform.localScale = originalScale;

                // "Pop" the text by increasing its scale briefly for effect.
                scoreText.transform.localScale = originalScale * popScale;

                // Increment the score by 1.
                currentScore += 1;
                UpdateScoreDisplay(currentScore);

                // Smoothly scale back the text to its original size.
                float timeElapsed = 0.0f;
                while (timeElapsed < animationDuration)
                {
                    timeElapsed += Time.deltaTime;
                    scoreText.transform.localScale = Vector3.Lerp(originalScale * popScale, originalScale, timeElapsed / animationDuration);
                    yield return null;
                }

                yield return null;
            }

            // Ensure the text is always reset to the original size after animation.
            scoreText.transform.localScale = originalScale;
        }

        /// <summary>
        /// Updates the score text UI element.
        /// </summary>
        /// <param name="score">The score value to display.</param>
        private void UpdateScoreDisplay(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}
