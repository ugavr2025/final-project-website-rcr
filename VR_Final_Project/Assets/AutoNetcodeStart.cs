using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class AutoNetcodeStart : MonoBehaviour
{
    [SerializeField] bool autoStart = true;

    void Start()
    {
        if (!autoStart) return;

        var nm = NetworkManager.Singleton;
        if (nm == null)
        {
            Debug.LogError("No NetworkManager.Singleton found in scene.");
            return;
        }

        var utp = nm.NetworkConfig.NetworkTransport as UnityTransport;
        if (utp == null)
        {
            Debug.LogError("UnityTransport not found on NetworkManager.");
            return;
        }

#if UNITY_ANDROID
        
        Debug.Log("[Netcode] Starting HOST on Android (Quest).");
        nm.StartHost();
#else
        
        
        string hostIp = "192.168.86.1";

       
        utp.SetConnectionData(hostIp, utp.ConnectionData.Port);

        Debug.Log($"[Netcode] Starting CLIENT, connecting to {hostIp}:{utp.ConnectionData.Port}");
        nm.StartClient();
#endif
    }
}
