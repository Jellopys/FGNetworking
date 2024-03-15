using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> playerName;
    [SerializeField] private Text playerText;
    private UserData clientUserData;


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            SetClientNameServerRpc();
        }
        else if (!IsOwner)
        {
            playerText.text = playerName.Value.ToString();
        }
    }

    [ServerRpc]
    private void SetClientNameServerRpc()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        ulong networkId = networkObject.OwnerClientId;
        clientUserData = SavedClientInformationManager.GetUserData(networkId);
        playerName.Value = clientUserData.userName;
        playerText.text = playerName.Value.ToString();

        SetClientNameClientRpc(playerName.Value.ToString());
    }

    [ClientRpc]
    private void SetClientNameClientRpc(string clientName)
    {
        playerText.text = clientName;
    }
}
