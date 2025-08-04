using UnityEngine;
using ParrelSync;
using FishNet;
using FishNet.Transporting;

public enum ConnectionType
{
    Host,
    Client
}

public class ConnectionHandler : MonoBehaviour
{
    public ConnectionType connectionType;

#if UNITY_EDITOR
    void OnEnable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
    }

    void OnDisable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Stopping)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
#endif

    void Start()
    {
#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone())
        {
            InstanceFinder.ClientManager.StartConnection();
        }
        else
        {
            if (connectionType == ConnectionType.Host)
            {
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
            }
            else
            {
                InstanceFinder.ClientManager.StartConnection();
            }
        }
#endif
        
        #if DEDICATED_SERVER
        InstanceFinder.ServerManager.StartConnection();
        #endif
    }
}
