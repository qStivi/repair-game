using UnityEngine.Events;

public class TimerManager
{
    private readonly bool autoReset;
    private readonly double duration;
    private bool isActive;

    public TimerManager(double duration, bool autoReset = false)
    {
        this.duration = duration * 1000;
        this.autoReset = autoReset;
    }

    public float TimeRemaining { get; private set; }


    public event UnityAction TimerElapsed;

    public void StartTimer()
    {
        isActive = true;
        TimeRemaining = (int)duration / 1000;
        EventManager.SyncTickEverySecond += ActionPerSecond;
    }

    private void ActionPerSecond()
    {
        if (!isActive)
            return;

        TimeRemaining--;
        if (TimeRemaining <= 0)
        {
            if (autoReset)
                ResetTimer();
            else
                Stop();
            TimerElapsed.Invoke();
        }
    }

    public void ResetTimer()
    {
        TimeRemaining = (int)duration / 1000;
    }

    public void Pause()
    {
        isActive = false;
    }

    public void Resume()
    {
        isActive = true;
    }

    public void Stop()
    {
        isActive = false;
        EventManager.SyncTickEverySecond -= ActionPerSecond;
    }

    //Safety for GarbageCollection
    private void OnDispose()
    {
        EventManager.SyncTickEverySecond -= ActionPerSecond;
    }
}