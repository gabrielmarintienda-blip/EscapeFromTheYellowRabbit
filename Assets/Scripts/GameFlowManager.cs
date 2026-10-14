using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("6 AM screen")]
    [SerializeField] private CanvasGroup sixAmImage;        // Fullscreen image with a CanvasGroup
    [SerializeField] private float fadeInDuration = 2f;     // Seconds to fade in
    [SerializeField] private float holdTime = 1f;           // Seconds the image stays fully visible
    [SerializeField] private GameObject nightCompleteMenu;  // Menu: next level / main menu

    [Header("Death screen")]
    [SerializeField] private GameObject deathMenu;          // Menu: retry / level select

    [Header("Jumpscare")]
    [SerializeField] private Image jumpscareImage;          // Fullscreen image, sprite set at runtime
    [SerializeField] private AudioSource audioSource;       // Plays the jumpscare sound
    [SerializeField] private float jumpscareDuration = 1.5f;
    [SerializeField] private float shakeAmount = 15f;       // Shake intensity in pixels

    [Header("Scenes")]
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string mainMenuScene = "MainMenu";

    [Header("Audio")]
    [SerializeField] private AudioSource[] soundsToStop; // Music and any ambient sounds to cut on death
    [SerializeField] private AudioClip deathSound;       // Played when the player dies

    private bool gameEnded = false; // Prevents triggering two endings at once

    // ---------- 6 AM ----------
    public void ShowNightComplete()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f; // Freeze the game
        StartCoroutine(NightCompleteRoutine());
    }

    private IEnumerator NightCompleteRoutine()
    {
        sixAmImage.gameObject.SetActive(true);
        sixAmImage.alpha = 0f;

        // Fade in using unscaled time (works while timeScale is 0)
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.unscaledDeltaTime;
            sixAmImage.alpha = Mathf.Clamp01(t / fadeInDuration);
            yield return null;
        }
        sixAmImage.alpha = 1f;

        yield return new WaitForSecondsRealtime(holdTime);

        sixAmImage.gameObject.SetActive(false);
        nightCompleteMenu.SetActive(true);
    }

    // ---------- Normal death ----------
    public void ShowDeathScreen()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;
        StopMusic();
        deathMenu.SetActive(true);
    }

    // ---------- Jumpscare ----------
    public void PlayJumpscare(Sprite sprite, AudioClip sound, bool shake)
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;
        StopMusic(); // The music stops as soon as the scare starts
        StartCoroutine(JumpscareRoutine(sprite, sound, shake));
    }

    private IEnumerator JumpscareRoutine(Sprite sprite, AudioClip sound, bool shake)
    {
        jumpscareImage.sprite = sprite;
        jumpscareImage.gameObject.SetActive(true);
        if (sound != null) audioSource.PlayOneShot(sound);

        RectTransform rect = jumpscareImage.rectTransform;
        Vector2 originalPos = rect.anchoredPosition;

        float t = 0f;
        while (t < jumpscareDuration)
        {
            t += Time.unscaledDeltaTime;
            if (shake)
            {
                // Random offset every frame to shake the image
                rect.anchoredPosition = originalPos + Random.insideUnitCircle * shakeAmount;
            }
            yield return null;
        }

            rect.anchoredPosition = originalPos;
            jumpscareImage.gameObject.SetActive(false);
            PlayDeathSound(); // Death sound after the scare (remove this line if you don't want it)
            deathMenu.SetActive(true);
    }

    // ---------- Button methods (hook these up in the Inspector) ----------
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectScene);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(mainMenuScene); // No more levels
    }

 private void StopMusic()
{
    Debug.Log("StopMusic called, sources in list: " + soundsToStop.Length);
    foreach (AudioSource source in soundsToStop)
    {
        if (source != null) source.Stop();
    }
}

    private void PlayDeathSound()
    {
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
    }

    /// <summary>
    /// Stops the music and plays the death sound right at the moment of a normal crash.
    /// </summary>
    public void PlayDeathAudio()
    {
        StopMusic();
        PlayDeathSound();
    }
}