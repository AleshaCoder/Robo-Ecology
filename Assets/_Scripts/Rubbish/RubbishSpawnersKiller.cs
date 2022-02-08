using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RubbishSpawnersKiller : MonoBehaviour
{    
    [SerializeField] private RubbishSpawnersIndicator _spawnersIndicator;
    [SerializeField] private GameObject _killer;
    [SerializeField] private bool _autoAttack = true;
    [SerializeField] private ParticleSystem _attackParticle;
    private RubbishRingSpawner _spawner;
    private Tween _tween;
    private bool _attacking = false;
    private IEnumerator MoveToSpawner(RubbishRingSpawner spawner, float time)
    {
        float currentTime = 0.0f;
        Vector3 vectorBetweenPositions = spawner.transform.position - _killer.transform.position;
        Vector3 vectorBetweenPositions0 = (spawner.transform.position - _killer.transform.position).normalized;
        Vector3 vectorIntoSpawner = -vectorBetweenPositions0 * spawner.transform.localScale.x;
        Vector3 vectorIntoKiller = -vectorBetweenPositions0 * _killer.transform.localScale.x;
        Vector3 targetVector = vectorBetweenPositions;
        Vector3 targetPosition = targetVector + _killer.transform.position;
        Vector3 startPosition = _killer.transform.position;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            _killer.transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / time);
            yield return new WaitForEndOfFrame();
        }
        _killer.transform.position = targetPosition;
        _attacking = false;
        _attackParticle.gameObject.SetActive(true);
        _attackParticle.transform.position = spawner.transform.position;
        _attackParticle.Play();
        spawner.KillSpawner();
    }

    private void Attack(RubbishRingSpawner spawner)
    {
        if (_attacking == true)
            return;
        _attacking = true;
        Vector3 vectorBetweenPositions = spawner.transform.position - _killer.transform.position;
        Vector3 vectorBetweenPositions0 = (spawner.transform.position - _killer.transform.position).normalized;
        Vector3 vectorIntoSpawner = -vectorBetweenPositions0 * spawner.transform.localScale.x;
        Vector3 vectorIntoKiller = -vectorBetweenPositions0 * _killer.transform.localScale.x;
        Vector3 targetVector = vectorBetweenPositions;
        Vector3 targetPosition = targetVector + _killer.transform.position;
        StartCoroutine(MoveToSpawner(spawner, 0.31f));
        //_killer.transform.position = targetPosition;
        //_tween = _killer.transform.DOMove(targetPosition, 0.31f).Play();
        //_tween.onComplete += spawner.KillSpawner;
        //_tween.onComplete += () => _attacking = false;
    }

    private void OnEnable()
    {
        _attackParticle.gameObject.SetActive(false);
        if (_autoAttack == true)
        {
            _spawnersIndicator.SpawnerHasFound += Attack;
        }
    }

    private void OnDisable()
    {
        if (_autoAttack == true)
        {
            _spawnersIndicator.SpawnerHasFound -= Attack;
        }
    }
}
