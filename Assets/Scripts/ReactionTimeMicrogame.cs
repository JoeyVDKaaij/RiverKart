using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
struct Road
{
    public GameObject prefab;
    public float chanceOfSpawning;
    public bool safe;
}

public class ReactionTimeMicrogame : Microgame
{
    [Header("Reaction Time Settings.")]
    [SerializeField, Tooltip("")]
    private float spawnLocation;
    [SerializeField, Tooltip("")]
    private float roadMovement;
    [SerializeField, Tooltip("")]
    private float despawnLocation;
    [SerializeField, Tooltip("")]
    private Road[] possibleRoad;

    [SerializeField, Tooltip("")] 
    private InputActionReference brakeInputAction;
    [SerializeField, Tooltip("")]
    private float brakeSpeed = 0.1f;
    [SerializeField, Tooltip("")]
    private float increasingSpeed = 0.1f;

    private List<Transform> _roads = new List<Transform>();
    private float _currentSpeed;

    private void Start()
    {
        do
        {
            GenerateRoad(0);
            foreach (Transform road in _roads)
                road.position -= new Vector3(0, road.localScale.y / 2, 0);
        } 
        while (_roads.Count == 0 || _roads[0].position.y + _roads[0].localScale.y / 2 >
               despawnLocation - _roads[^1].localScale.y / 2);

        _currentSpeed = roadMovement;
        
        brakeInputAction.action.Enable();
    }

    private void OnDestroy()
    {
        brakeInputAction.action.Disable();
    }

    protected override void MicrogameUpdate(float pCurrentTime)
    {
        if (_roads.Count > 0)
        {
            if (_roads[^1].position.y + _roads[^1].localScale.y / 2 < spawnLocation - _roads[^1].localScale.y / 2)
            {
                GenerateRoad();
            }

            if (_roads[0].position.y + _roads[0].localScale.y / 2 < despawnLocation - _roads[^1].localScale.y / 2)
            {
                Destroy(_roads[0].gameObject);
                _roads.RemoveAt(0);
            }

            foreach (Transform road in _roads)
            {
                road.position += Vector3.down * _currentSpeed * Time.deltaTime;
            }
        }
        else
        {
            GenerateRoad();
        }
        
        UpdateSpeed();
    }

    private void GenerateRoad(int pIndex = -1)
    {
        int roadTemplateIndex = Random.Range(0, possibleRoad.Length);
        GameObject road;
        
        if (_roads.Count <= 0)
        {
            if (pIndex == -1)
            {
                road = Instantiate(possibleRoad[roadTemplateIndex].prefab,
                    transform.position + Vector3.up * spawnLocation + Vector3.down * 0.1f,
                    Quaternion.identity);
            }
            else
            {
                road = Instantiate(possibleRoad[pIndex].prefab,
                    transform.position + Vector3.up * spawnLocation + Vector3.down * 0.1f,
                    Quaternion.identity);
            }
        }
        else
        {
            if (pIndex == -1)
            {
                road = Instantiate(possibleRoad[roadTemplateIndex].prefab, 
                    _roads[^1].position + Vector3.up * _roads[^1].localScale.y, Quaternion.identity);
            }
            else
            {
                road = Instantiate(possibleRoad[pIndex].prefab, 
                    _roads[^1].position + Vector3.up * _roads[^1].localScale.y, Quaternion.identity);
            }
        }
        _roads.Add(road.transform);

        if (road.TryGetComponent(out RoadCheck roadCheck))
        {
            if (pIndex == -1)
                roadCheck.SetSafety(possibleRoad[roadTemplateIndex].safe, this);
            else 
                roadCheck.SetSafety(possibleRoad[pIndex].safe, this);
                
        }
    }

    private void UpdateSpeed()
    {
        if (brakeInputAction.action.IsPressed())
            _currentSpeed -= brakeSpeed * Time.deltaTime;
        else
            _currentSpeed += increasingSpeed * Time.deltaTime;
        
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, roadMovement);
    }

    protected override void OnDeadline()
    {
        // Result Screen

        SceneManager.LoadScene(0);
    }

    public float GetSpeed
    {
        get { return _currentSpeed; }
    }
}
