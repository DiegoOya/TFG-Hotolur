using UnityEngine;

/// <summary>
/// Script to manage the economy of the time
/// </summary>
public class TimeManager : MonoBehaviour {

    #region Singleton

    public static TimeManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    // Variables to control maximum time between each check point and inside the check point
    public float maxTime = 60f;
    public float maxTimeInCP = 10f;

    private float time;

    // Initialize time
    private void Start()
    {
        time = maxTime;
    }

    private void Update()
    {
        // Decrease the time until it reaches zero
        time -= Time.deltaTime;

        // When it reaches zero then apply all the handicaps
        if(time < 0f)
        {
            // Implement all the handicaps thought for this event
        }
    }

    // Called when the player goes out a check point
    public void OutCheckPoint()
    {
        time = maxTime;
    }

    // Called when the player enters a check point
    public void InCheckPoint()
    {
        time = maxTimeInCP;
    }

    // Called to add time to the time counter
    public void AddTime(float extraTime)
    {
        // Add the time timeExtra seconds
        // And if it's higher than maxTime then set it to maxTime
        time += extraTime;
        time = Mathf.Min(time, maxTime);
    }

    // Called when the time counter reaches 0
    public void TimeEnd()
    {
        // *************Implement all the handicaps thought for this event
    }

}
