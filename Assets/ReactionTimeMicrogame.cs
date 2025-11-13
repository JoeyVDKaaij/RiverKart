using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    private GameObject[] possibleRoadTemplates;

    private List<Transform> _roads = new List<Transform>();

    private void Start()
    {
        do
        {
            GenerateRoad();
            foreach (Transform road in _roads)
                road.position -= new Vector3(0, road.localScale.y / 2, 0);
        } 
        while (_roads.Count == 0 || _roads[0].position.y + _roads[0].localScale.y / 2 >
               despawnLocation - _roads[^1].localScale.y / 2);
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
                road.position += Vector3.down * roadMovement * Time.deltaTime;
            }
        }
        else
        {
            GenerateRoad();
        }
    }

    private void GenerateRoad()
    {
        int roadTemplateIndex = Random.Range(0, possibleRoadTemplates.Length);
        GameObject road;
        
        if (_roads.Count <= 0)
        {
            road = Instantiate(possibleRoadTemplates[roadTemplateIndex],
                transform.position + Vector3.up * spawnLocation + Vector3.down * 0.1f,
                Quaternion.identity);
        }
        else
        {
            road = Instantiate(possibleRoadTemplates[roadTemplateIndex], 
                _roads[^1].position + Vector3.up * _roads[^1].localScale.y, Quaternion.identity);
        }
        _roads.Add(road.transform);
    }

    protected override void OnDeadline()
    {
        // Result Screen

        SceneManager.LoadScene(0);
    }
}
