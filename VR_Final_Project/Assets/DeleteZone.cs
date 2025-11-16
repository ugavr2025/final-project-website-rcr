using UnityEngine;
using Unity.Netcode;

public class DeleteZone : MonoBehaviour
{
    public string sculptTag = "Sculpt";

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(sculptTag))
            return;

        Transform root = other.transform.root;
        GameObject rootObj = root.gameObject;

        NetworkObject netObj = rootObj.GetComponent<NetworkObject>();

        bool netReady = NetworkManager.Singleton &&
                        NetworkManager.Singleton.IsListening &&
                        netObj != null;

        if (netReady &&
            (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost))
        {
            netObj.Despawn();
        }
        else
        {
            Destroy(rootObj);
        }
    }
}
