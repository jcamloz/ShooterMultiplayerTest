using UnityEngine;
using Fusion;

public class OroController : NetworkBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (HasStateAuthority && collision.gameObject.CompareTag("Player"))
        {
            Runner.Despawn(this.Object);
        }
    }
}
