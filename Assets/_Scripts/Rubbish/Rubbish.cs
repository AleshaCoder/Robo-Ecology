using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public enum RubbishType : byte
{
    Yellow,
    Green,
    Pink,
    Blue,
    Paper,
    Bottle,
    Coke,
    BitBottle,
    BitCoke
}

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Rubbish : MonoBehaviour
{
    [SerializeField] private RubbishType _rubbishType;

    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Tween _tween;

    public Rigidbody Body { get; private set; }
    public bool DestroyedSoon { get; private set; }
    public bool Hiden { get; private set; }
    public RubbishType RubbishType => _rubbishType;

    public event UnityAction Magneted;
    public event UnityAction<Rubbish> Destroing;
    public event UnityAction<Rubbish> OnShow;
    public event UnityAction<Rubbish> OnHide;

    private IEnumerator HideAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Hide();
    }

    public void Magnet()
    {
        Magneted?.Invoke();
    }

    public void MarkNextDestruction()
    {
        DestroyedSoon = true;
    }

    public void Hide()
    {
        _collider.isTrigger = true;
        _meshRenderer.enabled = false;
        Body.isKinematic = true;
        Hiden = true;
        DestroyedSoon = false;        
        OnHide?.Invoke(this);
    }

    public void Show()
    {
        _collider.isTrigger = false;
        _meshRenderer.enabled = true;
        Body.isKinematic = false;
        Hiden = false;
        OnShow?.Invoke(this);
    }

    public void AnimJumpTo(Vector3 point, float duration = -1)
    {
        duration = duration <= 0 ? Random.Range(0.5f, 3.0f) : duration;
        StartCoroutine(HideAfter(duration));
        _tween = transform.DOJump(point, Random.Range(3.1f, 7.1f), 1, duration).Play();
    }

    private void Awake()
    {
        Body = GetComponent(typeof(Rigidbody)) as Rigidbody;
        _meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        _collider = GetComponent(typeof(Collider)) as Collider;
        DestroyedSoon = false;
    }

    private void OnDestroy()
    {
        Destroing?.Invoke(this);
    }
}
