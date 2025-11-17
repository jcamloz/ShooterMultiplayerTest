using Fusion;
using System.Collections.ObjectModel;
using UnityEngine;

public class MarcadorController : NetworkBehaviour
{
    public TextMesh textoContador;
    [Networked]
    public int contador { get; set; }

    public override void Spawned()
    {
        if(HasStateAuthority)
            contador = 0;
    }

    public override void Render()
    {
        textoContador.text = contador.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(HasStateAuthority)
        {
            ReadOnlyDictionary<string, SessionProperty> totalOro = Runner.SessionInfo.Properties;
            if(totalOro.TryGetValue("Oro", out SessionProperty data))
            {
                contador = (int)data.PropertyValue;
            }
        }
    }
}
