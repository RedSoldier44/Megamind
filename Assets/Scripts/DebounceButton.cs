using UnityEngine;
using UnityEngine.Events;

public class DebounceButton : MonoBehaviour
{
    [SerializeField]
    private float pressDelay = 0.15f;
    [SerializeField]
    private float releaseDelay = 0.15f;
    [SerializeField]
    private UnityEvent onPressed;
    [SerializeField]
    private UnityEvent onReleased;

    private float pressTiming = -1f;
    private float releaseTiming = -1f;

    public void InvokePressed()
    {
        float time = Time.time;
        if (pressTiming < 0 || (time - pressTiming) >= pressDelay) {
            pressTiming = time;
            onPressed.Invoke();
        }
    }

    public void InvokeReleased()
    {
        float time = Time.time;
        if (releaseTiming < 0 || (time - releaseTiming) >= releaseDelay) {
            releaseTiming = time;
            onReleased.Invoke();
        }
    }
}
