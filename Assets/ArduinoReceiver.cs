using UnityEngine;
using System.IO.Ports;

public class ArduinoReader : MonoBehaviour
{
    SerialPort arduino = new SerialPort("COM3", 9600);
    int value;

    void Start()
    {
        arduino.Open();
    }

    void Update()
    {
        if (arduino.IsOpen && arduino.BytesToRead > 0)
        {
            try
            {
                string data = arduino.ReadLine();
                value = int.Parse(data); // convertit en int
                Debug.Log($"Valeur Arduino : {value}");
            }
            catch { }
        }
    }
}
