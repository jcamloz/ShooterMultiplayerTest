using Fusion;
using UnityEngine;

public class BalaController : NetworkBehaviour
{
    public override void Spawned()
    {
        Invoke("Autodestruir", 1);
    }

    private void Autodestruir()
    {
        Runner.Despawn(this.Object);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (HasStateAuthority)
        {
            Runner.Despawn(this.Object);
        }
    }
}
