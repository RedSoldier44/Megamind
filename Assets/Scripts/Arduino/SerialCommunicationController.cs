using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SerialCommunicationController : MonoBehaviour
{
    private SerialPort serialPort;
    private Thread serialThread;
    private bool isRunning = false;
    
    readonly object dataLock = new object();
    private string data;

    // à changer celon vos paramettres
    //readonly string portName = "COM8"; // Remplacez par votre port série
    readonly int baudRate = 9600; // Remplacez par votre port baudRate
    readonly int readTimeout = 500; // Timeout de lecture en millisecondes

    readonly float frameSkip = 0.5f;
    float frameSkipTime = 0.0f;
    bool tryConnect = false;

    [SerializeField]
    private string ping = "ping";
    [SerializeField]
    private string pong = "pong";
    [SerializeField]
    private UnityEvent<string> onData;
    public UnityEvent<string> OnData => onData;

    void Start() => TryConnect();
    void Update()
    {
        // Force check serial port opening
        if (serialPort == null && !isRunning && !tryConnect) {
            frameSkipTime += Time.deltaTime;
            if (frameSkipTime >= frameSkip) {
                frameSkipTime = 0.0f;
                TryConnect();
            }
        }

        lock (dataLock) {
            if (!string.IsNullOrEmpty(data)) {
                onData.Invoke(data);
                data = string.Empty; // Réinitialiser la donnée après traitement
            }
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
            serialThread.Join(); // Attendre que le thread se termine
        CloseSerialPort();
    }

    async void TryConnect()
    {
        tryConnect = true;
        await OpenSerialPort();
        if (destroyCancellationToken.IsCancellationRequested) return;
        if (serialPort != null) {
            isRunning = true;
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
        } tryConnect = false;
    }

    private async Task OpenSerialPort()
    {
        string portName = await SerialHelper.DetectArduinoPort(destroyCancellationToken, baudRate, ping, pong);
        if (string.IsNullOrEmpty(portName) || destroyCancellationToken.IsCancellationRequested) return;

        serialPort = new SerialPort(portName, baudRate) { ReadTimeout = readTimeout };
        serialPort.RtsEnable = true;
        serialPort.DtrEnable = true;

        try {
            serialPort.Open();
            Debug.Log("Serial port opened: " + portName);
        } catch (System.Exception ex) {
            serialPort = null;
            Debug.LogError("Error opening serial port: " + ex.Message);
        }
    }

    private void CloseSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen) {
            string portName = serialPort.PortName;
            serialPort.Close();
            Debug.Log("Serial port closed: " + portName);
        }
    }

    private void ReadSerialData()
    {
        while (isRunning && serialPort.IsOpen) {
            try {

                // Utiliser ReadLine pour lire une ligne complète
                string incomingData = serialPort.ReadLine();
                if (!string.IsNullOrEmpty(incomingData)) {
                    lock (dataLock) {
                        data = incomingData;
                    }
                }

            } catch (System.TimeoutException) {
            } catch (System.Exception ex) {
                Debug.LogError("Error reading from serial port: " + ex.Message +
                    "\nClosing serial port and try re-open it");

                // Nettoyer et forcer reconnexion
                try { serialPort.Close(); } catch { }
                serialPort = null;
                isRunning = false;
                frameSkipTime = 0.0f;
                return;
            }

            // Attendre un court moment avant de lire à nouveau pour éviter une surcharge du CPU
            Thread.Sleep(50); // 100
        }
    }

    public void SendData(string message)
    {
        if (serialPort != null && serialPort.IsOpen) {
            try {
                serialPort.WriteLine(message);
            } catch (System.Exception ex) {
                Debug.LogError("Error writing to serial port: " + ex.Message);
            }
        }
    }
}