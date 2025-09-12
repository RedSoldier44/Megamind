using UnityEngine;
using UnityEngine.Events;

// DATA_BTN_BIG_ 0-1
// DATA_BTN_MED_ 0-1

[RequireComponent(typeof(SerialCommunicationController))]
public class ButtonReader : MonoBehaviour
{
    void Start() => GetComponent<SerialCommunicationController>().OnData.AddListener(OnData);
    void OnDestroy() => GetComponent<SerialCommunicationController>().OnData.RemoveListener(OnData);

    [SerializeField, Tooltip("Button Data Subkey (after global key)")]
    private string key = "BTN_";
    [SerializeField, Tooltip("Button Identifier (after button key)")]
    private string identifier = "BIG_-MED_";

    [Header("Events")]
    [SerializeField, Tooltip("Callback invoked when button state change)")]
    private UnityEvent<bool> onChanged;
    public UnityEvent<bool> OnChanged => onChanged;

    [SerializeField, Tooltip("Callback invoked when button is pressed")]
    private UnityEvent onPressed;
    public UnityEvent OnPressed => onPressed;

    [SerializeField, Tooltip("Callback invoked when button is released")]
    private UnityEvent onReleased;
    public UnityEvent OnReleased => onReleased;

    private bool state = false;
    public bool State => state;

    void OnData(string data)
    {
        // Check if sub-key match with the button one
        if (!data.StartsWith(key)) return;
        data = data.Substring(key.Length);

        // Check if identifier match with this one
        if (!data.StartsWith(identifier)) return;
        data = data.Substring(identifier.Length);

        // Try parse new button state
        if (int.TryParse(data, out int value)) {
            if (value != 0 && value != 1) return;

            // Releasing button
            if (value == 0 && state) {
                state = false;
                onReleased.Invoke();
                onChanged.Invoke(state);

            // Pressing button
            } else if (value == 1 && !state) {
                state = true;
                onPressed.Invoke();
                onChanged.Invoke(state);
            }
        }
    }
}
