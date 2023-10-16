using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPooling : MonoBehaviour
{
    public static FxPooling ins;
    public ParticleSystem _muzzleFlash;
    private List<ParticleSystem> muzzleFlashPool = new List<ParticleSystem>();
    public ParticleSystem _metallHitEffect;
    private List<ParticleSystem> metallHitEffectPool = new List<ParticleSystem>();
    public ParticleSystem _stoneHitEffect;
    private List<ParticleSystem> stoneHitEffectPool = new List<ParticleSystem>();
    public ParticleSystem _woodHitEffect;
    private List<ParticleSystem> woodHitEffectPool = new List<ParticleSystem>();
    public ParticleSystem _bloodEffect;
    private List<ParticleSystem> bloodEffectPool = new List<ParticleSystem>();
    private void Start()
    {
        ins = this;
    }
    public ParticleSystem GetwoodHitEffectPool(Vector3 pos)
    {
        return GetPool(pos, _woodHitEffect, woodHitEffectPool);
    }
    public ParticleSystem GetbloodEffectPool(Vector3 pos)
    {
        return GetPool(pos, _bloodEffect, bloodEffectPool);
    }
    public ParticleSystem GetmuzzleFlashPool(Vector3 pos)
    {
        return GetPool(pos, _muzzleFlash, muzzleFlashPool);
    }
    public ParticleSystem GetmetallHitEffectPool(Vector3 pos)
    {
        return GetPool(pos, _metallHitEffect, metallHitEffectPool);
    }
    public ParticleSystem GetstoneHitEffectPool(Vector3 pos)
    {
        return GetPool(pos, _stoneHitEffect, stoneHitEffectPool);
    }
    public ParticleSystem GetPool(Vector3 pos, ParticleSystem particleSystem, List<ParticleSystem> Pool)
    {
        foreach (var particle in Pool)
        {
          
            if (!particle.gameObject.activeInHierarchy)
            {
                particle.gameObject.SetActive(true);              
                particle.gameObject.transform.position = pos;
                particle.gameObject.transform.rotation = Quaternion.identity;
                return particle;
            }
        }
        ParticleSystem newparticle = Instantiate(particleSystem, pos,Quaternion.identity);
        muzzleFlashPool.Add(newparticle);
        return newparticle;
    }

    public void ReturnPool(ParticleSystem particleSystem, float time)
    {
        StartCoroutine(CouroutineReturnPool(particleSystem, time));
    }
    IEnumerator  CouroutineReturnPool(ParticleSystem particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        particleSystem.gameObject.SetActive(false);

    }
}
