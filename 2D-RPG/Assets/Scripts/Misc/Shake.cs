using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : Singleton<Shake>
{
    private CinemachineImpulseSource source;

    protected override void Awake()
    {
        base.Awake();

        source = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeScreen()
    {
        source.GenerateImpulse();
    }
}
