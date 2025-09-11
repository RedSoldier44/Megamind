using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class SerialHelper
{
    public static async Task<string> DetectArduinoPort(CancellationToken token, int baudRate = 9600, string ping = "ping", string pong = "pong")
    {
        if (string.IsNullOrEmpty(ping) || string.IsNullOrEmpty(pong)) {
            Debug.LogError("Ping/Pong keys are invalid !");
            return null;
        }

        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports) {
            try {
                using SerialPort testPort = new SerialPort(port, baudRate);
                testPort.ReadTimeout = 500;
                testPort.WriteTimeout = 500;
                
                testPort.Open();
                await Task.Delay(2000);
                if (token.IsCancellationRequested) return null;
                testPort.DiscardInBuffer();

                // On "ping" l'Arduino
                testPort.WriteLine(ping);
                string response = await Task.Run(() => testPort.ReadLine());
                if (token.IsCancellationRequested) return null;

                if (response.Contains(pong)) {
                    //Debug.Log("Arduino détecté sur " + port);
                    return port;
                }
            } catch { /* Ignore et continue */ }
        }

        //Debug.LogError("Aucun Arduino détecté !");
        return null;
    }
}
