using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour {

    private LineRenderer line;

    public Transform[] points = new Transform[3];

    private int numPoints = 50;
    public Vector3[] positions = new Vector3[50];

    public float distance;
    [Range (0,1)]
    public float distanceAmount = 1;

	void Start () {

        line = GetComponent<LineRenderer>();
        line.positionCount = numPoints;

        Transform nextPoint;

        if(transform.GetSiblingIndex() + 1 < transform.parent.childCount)
        {
            nextPoint = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
            SetMidPoint(nextPoint);
            DrawQuadraticCurve();
        }

    }

    void SetMidPoint(Transform nextPoint)
    {
        distance = Vector3.Distance(transform.GetChild(0).position, nextPoint.GetChild(0).position);

        GameObject pivot = new GameObject();
        GameObject point = new GameObject();

        pivot.transform.parent = transform.parent.parent;

        point.transform.parent = pivot.transform;
        point.transform.position += (Vector3.forward / 2) + Vector3.forward * distance * distanceAmount;

        pivot.transform.localEulerAngles = new Vector3((transform.localEulerAngles.x + nextPoint.localEulerAngles.x) / 2,
            (transform.localEulerAngles.y + nextPoint.localEulerAngles.y) / 2, 0);

        points[0] = transform.GetChild(0);
        points[1] = point.transform;
        points[2] = nextPoint.GetChild(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DrawQuadraticCurve();
        }
    }

    private void DrawQuadraticCurve()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)numPoints;
            positions[i] = CalculateQuadraticBezierPoint(t, points[0].position, points[1].position, points[2].position);
        }

        line.SetPositions(positions);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
