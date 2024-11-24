using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCenterObj : MonoBehaviour
{
    
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private GameObject unityChan;
    [SerializeField] private GameObject wizard;
    [SerializeField] private bool moveClockwise = true;
    [SerializeField] private float radiusOffset = -20f;
    private float angle = 0f;
    private float directon = -1;
    private Vector3 unityChanPosition;


    // Update is called once per frame
    void Update()
    {
        unityChanPosition=unityChan.transform.position;


        directon=moveClockwise ? -1 : 1;
        angle += Time.deltaTime * directon * speed;

        float x=Mathf.Cos(angle)*radiusOffset;
        float z= Mathf.Sin(angle)*radiusOffset;

        wizard.transform.position = new Vector3(unityChanPosition.x + x, 0, unityChanPosition.z + z);
        transform.LookAt(unityChanPosition);

    }
}
