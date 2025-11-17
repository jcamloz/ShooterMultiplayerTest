using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public override void Spawned()
    {
        if(HasInputAuthority)
        {
            Camera c = Camera.main;
            c.transform.SetParent(this.transform);
            c.transform.position = this.transform.position + new Vector3(0, 3.5f, -4);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority)
        {
            if (GetInput(out InputData inputData))
            {
                this.transform.position += this.transform.forward * inputData.movimiento * 5 * Runner.DeltaTime;
                this.transform.Rotate(Vector3.up, inputData.rotacion);
            }
        }
    }
}
