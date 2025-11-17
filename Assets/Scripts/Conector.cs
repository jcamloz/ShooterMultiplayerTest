using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Conector : MonoBehaviour
{
    private NetworkRunner red;
    void Start()
    {
        Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
        propiedades.Add("Oro", (SessionProperty)0);

        red = GetComponent<NetworkRunner>();
        red.name = "Primer juego multijugador";
        red.ProvideInput = true;
        
        StartGameArgs argumentos = new StartGameArgs();
        argumentos.GameMode = GameMode.AutoHostOrClient;
        argumentos.SessionName = "sala1";
        argumentos.PlayerCount = 3;
        argumentos.SceneManager = this.GetComponent<NetworkSceneManagerDefault>();
        argumentos.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        argumentos.SessionProperties = propiedades;

        red.StartGame(argumentos);
    }
}
