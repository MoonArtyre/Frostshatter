using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 goalPosition;
    private bool positionSatisifed;
    private Transform visualTransform;
    float lastSearch = 0;
    [SerializeField] private float orbitRadius, droneSpeed, satisfyRadius;
    [SerializeField] private Transform orbitingObject;
    [SerializeField] private LayerMask pathCheckMask;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        visualTransform = transform.GetChild(0);
        searchNewPosition();
    }

    private void Update()
    {
        if(body.velocity != Vector3.zero)
            visualTransform.forward = Vector3.Slerp(visualTransform.forward, body.velocity, Time.deltaTime * 10);

        lastSearch += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!positionSatisifed)
        {
            moveToNewPos();

            if (Vector3.Distance(transform.position, orbitingObject.position) > satisfyRadius * 2)
                searchNewPosition();
        }
        else
        {
            checkPositionSatisfied();
        }
    }

    void searchNewPosition()
    {
        if (lastSearch < 1f)
            return;

        lastSearch = 0;
        positionSatisifed = false;

        var newPosFound = false;
        var currentTries = 0;

        while (!newPosFound && currentTries < 100)
        {
            currentTries++;

            goalPosition = orbitingObject.position + new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f, 1f)).normalized * orbitRadius + Vector3.up * 2f;
            goalPosition += orbitingObject.forward / 2;
            var goalPosSphereCast = Physics.OverlapSphere(goalPosition,0.5f, pathCheckMask);
            Physics.Linecast(transform.position,goalPosition,out var goalPosLineCast, pathCheckMask);

            if(goalPosSphereCast.Length == 0 && goalPosLineCast.collider == null)
            {
                newPosFound = true;
            }
        }
    }

    void moveToNewPos()
    {
        var forceDir =  goalPosition - transform.position;
        body.AddForce(forceDir * droneSpeed / 100, ForceMode.Impulse);

        if(Vector3.Distance(transform.position,goalPosition) < satisfyRadius / 2)
        {
            positionSatisifed = true;
        }
    }

    void checkPositionSatisfied()
    {
        if (Vector3.Distance(transform.position, goalPosition) > satisfyRadius)
        {
            if (positionSatisifed)
            {
                searchNewPosition();
            }
        }

        if (Vector3.Distance(transform.position, orbitingObject.position + orbitingObject.forward) > satisfyRadius * 8f)
        {

                searchNewPosition();
        }
    }
}
