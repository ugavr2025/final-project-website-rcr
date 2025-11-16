using UnityEngine;
using Unity.Netcode;

public class NetworkStarter : MonoBehaviour
{
    public void StartHost()
    {
        if (NetworkManager.Singleton != null && !NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Started HOST");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton != null && !NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Started CLIENT");
        }
    }
}
