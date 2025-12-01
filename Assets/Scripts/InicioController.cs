using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InicioController : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner red;
    private NetworkSceneManagerDefault sceneManager;
    private ListaJugadoresController LJC;

    public InputField inputNewPartida;
    public Button botonPartida;
    public GameObject panelPartidas;

    private void Awake()
    {
        red = FindAnyObjectByType<NetworkRunner>();
        red.name = "Mi Shooter";
        red.ProvideInput = true;
        red.AddCallbacks(this);

        sceneManager = FindAnyObjectByType<NetworkSceneManagerDefault>();

        LJC = FindAnyObjectByType<ListaJugadoresController>();

        ConectarLoby();
    }

    public async void ConectarLoby()
    {
        await red.JoinSessionLobby(SessionLobby.ClientServer);
    }

    public async void ConectarSesion(StartGameArgs argumentos)
    {
        await red.StartGame(argumentos);
    }

    public void CrearPartida()
    {
        Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
        propiedades.Add("Oro", (SessionProperty)0);

        StartGameArgs argumentos = new StartGameArgs();
        argumentos.GameMode = GameMode.Host;
        argumentos.SessionName = inputNewPartida.text;
        argumentos.PlayerCount = 3;
        argumentos.SceneManager = sceneManager;
        argumentos.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        argumentos.SessionProperties = propiedades;

        ConectarSesion(argumentos);
    }
    public void ConectarPartida(string nombrePartida)
    {
        Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
        propiedades.Add("Oro", (SessionProperty)0);

        StartGameArgs argumentos = new StartGameArgs();
        argumentos.GameMode = GameMode.Client;
        argumentos.SessionName = nombrePartida;
        argumentos.PlayerCount = 3;
        argumentos.SceneManager = sceneManager;
        argumentos.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        argumentos.SessionProperties = propiedades;

        ConectarSesion(argumentos);
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
        if (runner.IsServer)
        {
            if(!LJC.listaSJ.ContainsKey(runner.SessionInfo))
            {
                LJC.listaSJ.Add(runner.SessionInfo, new Dictionary<PlayerRef, NetworkObject>());
            }
            LJC.listaSJ[runner.SessionInfo].Add(player, null);

            if(runner.IsSceneAuthority)
            {
                if (runner.SessionInfo.PlayerCount == runner.SessionInfo.MaxPlayers)
                {
                    runner.UnloadScene(SceneRef.FromIndex(0));
                    runner.LoadScene(SceneRef.FromIndex(2));
                }
                else
                {
                    runner.UnloadScene(SceneRef.FromIndex(0));
                    runner.LoadScene(SceneRef.FromIndex(1));
                }
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if(SceneManager.GetActiveScene().buildIndex==0)
        {
            var children = panelPartidas.transform.Cast<Transform>().ToArray();

            foreach (var child in children)
            {
                Destroy(child.gameObject);
            }
            foreach (SessionInfo s in sessionList)
            {
                Button boton = Instantiate(botonPartida, panelPartidas.transform);

                boton.GetComponentInChildren<Text>().text = s.Name + "(" + s.PlayerCount + "/" + s.MaxPlayers + ")";
                boton.onClick.AddListener( () => ConectarPartida(s.Name));
            }
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
