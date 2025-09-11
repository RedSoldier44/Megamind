using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SerialCommunicationController))]
public class InterruptorsReader : MonoBehaviour
{
    void Start() => GetComponent<SerialCommunicationController>().OnData.AddListener(OnData);
    void OnDestroy() => GetComponent<SerialCommunicationController>().OnData.RemoveListener(OnData);

    [SerializeField]
    private string key = "INTR_";
    [SerializeField]
    private string dataSeparator = "|";

    [Header("Events")]
    [SerializeField]
    private UnityEvent<int, int> onSingleChanged;
    public UnityEvent<int, int> OnSingleChanged => onSingleChanged;

    [SerializeField]
    private UnityEvent<int[]> onChanged;
    public UnityEvent<int[]> OnChanged => onChanged;

    const int interruptorCount = 5, iCount = interruptorCount - 1;
    readonly Dictionary<int, int> changes = new Dictionary<int, int>();
    readonly int[] interruptors = new int[interruptorCount];
    public int[] Interruptors => interruptors;

    void OnData(string data)
    {
        if (!data.StartsWith(key)) return;
        data = data.Substring(key.Length);

        string[] values = data.Split(dataSeparator);
        if (values.Length != interruptorCount) return;

        changes.Clear();
        for (int i = 0; i < values.Length; ++i) {
            int val = ParseOneHot8To1to6(values[iCount - i]);
            if (val > 0 && val != interruptors[i]) {
                interruptors[i] = val;
                changes.Add(i, val);
            }
        }

        int[] keys = changes.Keys.ToArray();
        if (keys.Length > 0) {
            foreach (int key in keys)
                onSingleChanged.Invoke(key, changes[key]);
            onChanged.Invoke(keys);
            Debug.Log("Interruptors - " + Time.time.ToString() + " : " + string.Join(", ", interruptors));
        }
    }

    /// <summary>
    /// Parse a one-hot 8-bit string (MSB->LSB).
    /// Returns 1..6 for valid one-hot values.
    /// Returns 0 if invalid (multiple bits, malformed, or result would be 7/8).
    /// </summary>
    public static int ParseOneHot8To1to6(string bitString)
    {
        if (string.IsNullOrEmpty(bitString)) return 0;
        if (bitString.Length != 8) return 0;
        int oneIndex = 0, onesCount = 0;

        for (int i = 2; i < 8; ++i) {
            char c = bitString[i];
            if (c != '0' && c != '1') return 0;
            if (c == '1') {
                oneIndex = 8 - i;
                if (++onesCount > 1)
                    return 0;
            }
        }

        if (onesCount != 1) return 0;
        return oneIndex;
    }
}
