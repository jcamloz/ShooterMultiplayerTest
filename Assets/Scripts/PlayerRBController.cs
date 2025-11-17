using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerRBController : NetworkBehaviour
{
    NetworkRigidbody3D nrb;
    private GameObject boquilla;
    public GameObject pelota;

    public TextMesh textoVida;

    /*[Networked]
    public int vida { get; set; }*/

    public int vida;

    private void Awake()
    {
        nrb = GetComponent<NetworkRigidbody3D>();
        boquilla = this.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
    }

    public override void Spawned()
    {
        vida = 3;
        
        if (HasInputAuthority)
        {
            Camera c = Camera.main;
            c.transform.SetParent(this.transform);
            c.transform.position = this.transform.position + new Vector3(0, 3.5f, -4);
        }
    }

    public override void Render()
    {
        textoVida.text = vida.ToString();
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority)
        {
            if (GetInput(out InputData inputData))
            {
                nrb.Rigidbody.MovePosition(this.transform.position + this.transform.forward * inputData.movimiento * 5 * Runner.DeltaTime);
                Quaternion rotacion = Quaternion.Euler(new Vector3(0, inputData.rotacion, 0) * Runner.DeltaTime * 180);
                nrb.Rigidbody.MoveRotation(nrb.Rigidbody.rotation * rotacion);

                if(inputData.disparo)
                {
                    NetworkObject p = Runner.Spawn(pelota, boquilla.transform.position, Quaternion.identity, Object.InputAuthority);
                    p.gameObject.GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(boquilla.transform.up * 20, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(HasStateAuthority && collision.gameObject.CompareTag("Bala"))
        {
            RPC_QuitarVida();
        }

        if (HasStateAuthority && collision.gameObject.CompareTag("Oro"))
        {
            ReadOnlyDictionary<string, SessionProperty> Oro = Runner.SessionInfo.Properties;

            if(Oro.TryGetValue("Oro", out SessionProperty data))
            {
                int kilosOro = (int)data.PropertyValue;
                kilosOro++;

                Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
                propiedades.Add("Oro", (SessionProperty)kilosOro);

                Runner.SessionInfo.UpdateCustomProperties(propiedades);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_QuitarVida()
    {
        vida--;

        if(vida < 1 && HasStateAuthority)
        {
            Runner.Despawn(this.Object);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_DarVida()
    {
        vida++;
        RPC_DarVidaTodos(vida);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DarVidaTodos(int vida)
    {
        this.vida = vida;
    }

    private void Update()
    {
        if (HasInputAuthority && Input.GetKey(KeyCode.X))
        {
            RPC_DarVida();
        }
    }
}
