using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private bool isTimerRunning = false;
    private float elapsedTime = 0; // Tracks elapsed time in seconds

    /// <summary>
    /// Starts the timer.
    /// </summary>
    private void Start()
    {
        StartTimer();
    }
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            elapsedTime = 0; // Reset the timer when starting
            StartCoroutine(TimerCoroutine());
        }
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// Gets the recorded time as a formatted string (M:SS).
    /// </summary>
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return $"{minutes}:{seconds:D2}"; // Format as "M:SS"
    }

    /// <summary>
    /// Gets the raw elapsed time in seconds.
    /// </summary>
    public float GetRawElapsedTime()
    {
        return elapsedTime;
    }

    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }
}
