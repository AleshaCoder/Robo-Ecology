using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Path : MonoBehaviour
{
    [SerializeField] private List<Point> _points = new List<Point>();
    [SerializeField] private Point _template;

    private List<Vector3> _pointsPositions = new List<Vector3>();
    private int _currentIndex = 0;

    public event UnityAction PathEnd;

    public Vector3 CurrentPoint
    {
        get
        {
            return GetPointFromPath(_currentIndex);
        }
    }

    public Vector3 NextPoint
    {
        get
        {
            _currentIndex += 1;
            if (_currentIndex >= _points.Count)
            {
                _currentIndex = 0;
                PathEnd?.Invoke();
            }
            return GetPointFromPath(_currentIndex);
        }
    }

    private Vector3 GetPointFromPath(int index)
    {
        if (_points.Count > index)
            return _points[index].Position;
        else if (_points.Count > 0)
            return _points[_points.Count - 1].Position;
        else
            return new Vector3(0, 0, 0);
    }

    private List<Vector3> GetNewPointsPositions()
    {
        List<Vector3> pointsPositions = new List<Vector3>();
        foreach (Point point in _points)
        {
            pointsPositions.Add(point.transform.position);
        }
        return pointsPositions;
    }

    public List<Vector3> GetPoints()
    {
        _pointsPositions = _points.Count == _pointsPositions.Count ? _pointsPositions : GetNewPointsPositions();
        return _pointsPositions;
    }

    public void SetPoints(List<Vector3> points)
    {
        _points.Clear();
        foreach (var item in points)
        {
            Point point = Instantiate(_template, item, Quaternion.identity);
            point.transform.parent = transform;
            _points.Add(point);
        }
    }

    public List<Vector3> GetRandomPoints(int pointsCount, Vector3 currentPosition)
    {
        List<Vector3> pointsPositions = GetPoints();

        for (int i = 0; i < pointsPositions.Count; i++)
        {
            pointsPositions[i] = pointsPositions[Random.Range(0, pointsPositions.Count)];
        }
        return pointsPositions;
    }
}