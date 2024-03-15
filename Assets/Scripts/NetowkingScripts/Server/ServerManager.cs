using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using UnityEngine;

public class ServerManager : ScriptableObject, IDisposable
{
    private NetworkManager networkManager;
    private NetworkServer networkServer;
    public MultiplayAllocationService multiplayAllocationService;

    private string ip;
    private int port;
    private int qPort;



    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        return true;
    }

    public void ServerManagerConfig(NetworkManager networkManager, string ip, int port, int qPort)
    {
        this.networkManager = networkManager;
        this.networkServer = new NetworkServer(networkManager);
        multiplayAllocationService = new MultiplayAllocationService();
        this.ip = ip;
        this.port = port;
        this.qPort = qPort;
    }

    private bool OpenConnection()
    {
        UnityTransport unityTransport = networkManager.gameObject.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData(ip, (ushort)port);
        return true;
    }

    public async Task StartServerAsync()
    {
        await multiplayAllocationService.BeginServerCheck();

        if (!OpenConnection())
        {
            Debug.Log("It's not an error! Because our code should not work because I didnt finish the code.");
        }
    }

    public void Dispose()
    {
        multiplayAllocationService?.Dispose();
        networkServer?.Dispose();
    }
}
