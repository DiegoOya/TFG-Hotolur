using UnityEngine;

public class TimeManager : MonoBehaviour {

    #region Singleton

    public static TimeManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public float maxTime = 60f;
    public float maxTimeInCP = 10f;

    private float time;

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

    public void OutCheckPoint()
    {
        time = maxTime;
    }

    public void InCheckPoint()
    {
        time = maxTimeInCP;
    }

    public void AddTime(float extraTime)
    {
        // Add the time timeExtra seconds
        // And if it's higher than maxTime then set it to maxTime
        time += extraTime;
        time = Mathf.Min(time, maxTime);
    }

    public void TimeEnd()
    {
        // Implement all the handicaps thought for this event
    }

}
