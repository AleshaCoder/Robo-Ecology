using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class RubbishRingSpawner : AchievementGiver
{
    [SerializeField] private SpawnerType _type = SpawnerType.Type1;
    [SerializeField] private int _numberOfSpawners;
    [SerializeField] private float _ringRadius, _tiltAngle;
    [SerializeField] private float _revivalTime;
    [SerializeField] private RubbishSpawner _spawnerPrefab;
    [SerializeField] private PathMovement _pathMovement;
    [SerializeField] private RubbishPool _rubbishPool;
    [SerializeField] private Rubbish[] _rubbishPrefabs;
    [SerializeField] private Material _deadMaterial;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] public UnityEvent OnKill;

    private MeshRenderer _renderer;
    private Material _standartMaterial;
    private List<RubbishSpawner> _spawners = new List<RubbishSpawner>();
    private bool _wasInit = false;
    private int _updateIndex = 0;
    private int _needUpdateIndex = 10;

    public bool Killing { get; private set; }
    public SpawnerType Type => _type;

    public Path GetPath()
    {
        return _pathMovement.GetPath();
    }

    public PathMovement GetPathMovement()
    {
        return _pathMovement;
    }

    public void KillSpawner()
    {
        if (Killing == false)
        {
            Achievement.AddFullness(1);
            OnKill?.Invoke();
            StopSpawners();
            StartCoroutine(AnimKill());
        }
    }

    public void StopSpawners()
    {
        foreach (var spawner in _spawners)
        {
            spawner.Stop();
        }
        _particles.Stop();
    }

    public void StartSpawners()
    {
        foreach (var spawner in _spawners)
        {
            spawner.Play();
        }
        _particles.Play();
    }

    private IEnumerator AnimKill()
    {
        int tact = Random.Range(1, 10);
        float timeBetweenTact = Random.Range(0.1f, 0.5f);
        _pathMovement.StopMoving();

        while (tact > 0)
        {
            tact--;
            _renderer.sharedMaterial = _deadMaterial;
            yield return new WaitForSeconds(timeBetweenTact / 2);
            _renderer.sharedMaterial = _standartMaterial;
            yield return new WaitForSeconds(timeBetweenTact / 2);
        }

        Killing = true;
        _renderer.enabled = false;
        GetComponent<Collider>().isTrigger = true;        
        StartCoroutine(WaitRevival());
    }

    private IEnumerator WaitRevival()
    {
        var time = Mathf.Abs(_revivalTime);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        _renderer.enabled = true;
        _renderer.sharedMaterial = _standartMaterial;
        GetComponent<Collider>().isTrigger = false;
        Killing = false;
        _pathMovement.StartMoving();
        StartCoroutine(CheckVisibility());
    }

    private IEnumerator CheckVisibility()
    {
        while (Killing == false)
        {
            StartSpawners();
            while (CameraVisibility.IsObjectVisible(_renderer) == true)
            {
                yield return new WaitForSeconds(1f);
            }
            StopSpawners();
            while (CameraVisibility.IsObjectVisible(_renderer) == false)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void SpawnRubbish(GameObject spawner, float RubbishVelocity)
    {
        Rubbish spawn = null;
        Transform origin = spawner.transform;
        if (_rubbishPool.HasHiden)
        {
            foreach (var rubbish in _rubbishPrefabs)
                if (_rubbishPool.TryGetHidenRubbish(rubbish.RubbishType, out spawn))
                {
                    spawn.Show();
                    break;
                }
        }
        if (spawn == null)
        {
            if (_rubbishPool.Full)
            {
                return;
            }
            Rubbish prefab = _rubbishPrefabs[Random.Range(0, _rubbishPrefabs.Length)];
            spawn = Instantiate(prefab);
        }

        if (spawn == null)
            return;

        spawn.transform.parent = _rubbishPool.transform;
        spawn.transform.position = origin.position;
        spawn.Body.velocity = origin.up * RubbishVelocity;
        _rubbishPool.AddRubbish(spawn);
    }

    private void CreateSpawner(int index)
    {
        Transform rotater = new GameObject("Rotater").transform;
        rotater.SetParent(transform, false);
        rotater.localRotation = Quaternion.Euler(0f, index * 360f / _numberOfSpawners, 0f);
        GameObject go = new GameObject();
        go.transform.SetParent(rotater, false);
        go.transform.localPosition = new Vector3(0f, _renderer.bounds.size.y / 1.5f, _ringRadius);
        go.transform.localRotation = Quaternion.Euler(_tiltAngle, 0f, 0f);
        RubbishSpawner spawner = new RubbishSpawner(go, _spawnerPrefab.RubbishVelocity, _spawnerPrefab.TimeBetweenSpawns);
        spawner.OnSpawn += SpawnRubbish;
        _spawners.Add(spawner);
    }

    public void Init(bool killing)
    {
        _renderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        _standartMaterial = _renderer.sharedMaterial;
        for (int i = 0; i < _numberOfSpawners; i++)
        {
            CreateSpawner(i);
        }
        StopSpawners();
        StartCoroutine(CheckVisibility());
        _wasInit = true;
        if (_rubbishPool == null)
            _rubbishPool = FindObjectOfType(typeof(RubbishPool)) as RubbishPool;
        if (killing == true)
        {
            _renderer.enabled = false;
            _particles.Stop();
            GetComponent<Collider>().isTrigger = true;
            StopCoroutine(CheckVisibility());
            Killing = true;
            StartCoroutine(WaitRevival());
        }
    }

    private IEnumerator TryInit()
    {
        yield return new WaitForEndOfFrame();
        if (_wasInit == false)
        {
            Init(false);
        }
    }

    private void Start()
    {
        StartCoroutine(TryInit());
        Achievement = AchievmentCollection.instance.GetAchievement(AchievmentCollection.TrashRobots);
    }

    private void FixedUpdate()
    {
        if (Killing == true)
            return;
        if (_wasInit == false)
            return;
        _updateIndex++;
        if (_updateIndex != _needUpdateIndex)
            return;        
        foreach (var item in _spawners)
        {
            item.TrySpawn(Time.fixedDeltaTime * _updateIndex);
        }
        _updateIndex = 0;
    }
}
