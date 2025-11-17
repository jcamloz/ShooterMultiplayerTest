using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GestorDeEventos : MonoBehaviour, INetworkRunnerCallbacks
{
    public GameObject jugador;
    public Text mensaje;
    private Dictionary<PlayerRef, NetworkObject> listaJugadores;
    private InputData inputData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputData = new InputData();
        listaJugadores = new Dictionary<PlayerRef, NetworkObject>();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        inputData.movimiento = 0;
        inputData.rotacion = 0;

        if(Input.GetKey(KeyCode.W))
        {
            inputData.movimiento = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputData.movimiento = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputData.rotacion = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputData.rotacion = 1;
        }

        inputData.disparo = Input.GetKey(KeyCode.Space);
        /*if (Input.GetKey(KeyCode.Space))
        {
            inputData.disparo = true;
        }*/

        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            //runner.Spawn(pelota, new Vector3(0, 3, 0), Quaternion.identity, player);
            //NetworkObject p = runner.Spawn(jugador, new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 2, UnityEngine.Random.Range(-10.0f, 10.0f)), Quaternion.identity, player);
            
            listaJugadores.Add(player, null);
            
            if(listaJugadores.Count == runner.SessionInfo.MaxPlayers && runner.IsSceneAuthority)
            {
                runner.UnloadScene(SceneRef.FromIndex(0));
                runner.LoadScene(SceneRef.FromIndex(1));
            }
        }
        mensaje.text = "Esperando..."+runner.ActivePlayers.Count()+"/"+runner.SessionInfo.MaxPlayers;
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer && listaJugadores.TryGetValue(player, out NetworkObject p))
        {
            runner.Despawn(p);
            listaJugadores.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if(runner.IsServer && SceneManager.GetActiveScene().buildIndex==1)
        {
            foreach(PlayerRef pR in listaJugadores.Keys.ToList())
            {
                if (listaJugadores[pR] == null)
                {
                    NetworkObject p = runner.Spawn(jugador, new Vector3(UnityEngine.Random.Range(9, -9), 2, UnityEngine.Random.Range(9, -9)), Quaternion.identity, pR);
                    listaJugadores[pR] = p;
                }
            }
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
