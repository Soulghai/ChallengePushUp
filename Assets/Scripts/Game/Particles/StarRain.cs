using UnityEngine;

public class StarRain : MonoBehaviour
{
    public ParticleSystem Ps;

    public void ChangeParticles(float intensivity)
    {
        var main = Ps.main;
        var emission = Ps.emission;
        emission.rateOverTime = intensivity / 2 * Ps.emission.rateOverTime.constant;
        main.startSpeed = intensivity * main.startSpeed.constant;
    }
}