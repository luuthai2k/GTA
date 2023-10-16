using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RagDoll : MonoBehaviour
{

    private enum ZombieState
    {
        Walking,
        Ragdoll,
        StandingUp,
        ResettingBones
    }


    [SerializeField]
    private string _standUpStateName;

    [SerializeField]
    private string _standUpClipName;

    [SerializeField]
    private float _timeToResetBones;

    [SerializeField]
    private Rigidbody[] _ragdollRigidbodies;
    [SerializeField]
    private ZombieState _currentState = ZombieState.Walking;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _timeToWakeUp;
    [SerializeField]
    private Transform _hipsBone;

    [SerializeField]
    private List<Transform> _bones;

    [SerializeField]
    private List<Vector3> pos;

    [SerializeField]
    private List<Quaternion> rotation; 

    [SerializeField]
    private float _elapsedResetBonesTime;

    [SerializeField]
    private Transform parentHip;

    void Awake()
    {
        _hipsBone = _animator.GetBoneTransform(HumanBodyBones.Hips);

        PopulateAnimationStartBoneTransforms(_standUpClipName);

        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case ZombieState.Walking:
                WalkingBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour();
                break;
            case ZombieState.StandingUp:
                StandingUpBehaviour();
                break;
            case ZombieState.ResettingBones:
                ResettingBonesBehaviour();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerRagdoll(Vector3.zero, Vector3.zero);
        }
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        Rigidbody hitRigidbody = FindHitRigidbody(hitPoint);

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        _currentState = ZombieState.Ragdoll;
        _timeToWakeUp = Random.Range(5, 10);
    }

    private Rigidbody FindHitRigidbody(Vector3 hitPoint)
    {
        Rigidbody closestRigidbody = null;
        float closestDistance = 0;

        foreach (var rigidbody in _ragdollRigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.position, hitPoint);

            if (closestRigidbody == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestRigidbody = rigidbody;
            }
        }

        return closestRigidbody;
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        //_characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        _animator.enabled = false;
        // _characterController.enabled = false;
    }

    private void WalkingBehaviour()
    {

    }

    private void RagdollBehaviour()
    {
        _timeToWakeUp -= Time.deltaTime;

        if (_timeToWakeUp <= 0)
        {
            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(_bones);

            _currentState = ZombieState.ResettingBones;
            _elapsedResetBonesTime = 0;
        }
    }

    private void StandingUpBehaviour()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_standUpStateName) == false)
        {
            _currentState = ZombieState.Walking;
        }
    }

    private void ResettingBonesBehaviour()
    {
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

        for (int boneIndex = 0; boneIndex < _bones.Count; boneIndex++)
        {
            _bones[boneIndex].DOLocalMove(pos[boneIndex], 2);
            _bones[boneIndex].DOLocalRotate(rotation[boneIndex] * Vector3.forward, 2);
          
        }
        Invoke("EndRagdoll", 3);
    }

    private void EndRagdoll()
    {
        _currentState = ZombieState.StandingUp;
        DisableRagdoll();

        _animator.Play(_standUpStateName);

    }

    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }

    private void AlignPositionToHips()
    {
        Vector3 pos = _hipsBone.position;

        transform.position = pos;

        _hipsBone.position = pos;
    }

    private void PopulateBoneTransforms(List<Transform> boneTransforms)
    {
        for (int boneIndex = 0; boneIndex < _bones.Count; boneIndex++)
        {
            boneTransforms[boneIndex].position = _bones[boneIndex].position;
            boneTransforms[boneIndex].rotation = _bones[boneIndex].rotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                
                break;
            }
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }
}

