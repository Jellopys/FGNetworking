using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ServerSingleton : Singleton<ServerSingleton>
{
    public ServerManager serverManager;
    public bool isAuth = false;

    public async Task InitServerAsync()
    {
        serverManager = ScriptableObject.CreateInstance<ServerManager>();
        isAuth = await serverManager.InitAsync();
        serverManager.ServerManagerConfig(NetworkManager.Singleton, ApplicationData.IP(), ApplicationData.Port(), ApplicationData.QPort());
    }

    public async Task StartServerAsync()
    {
        await serverManager.StartServerAsync();
    }
}
