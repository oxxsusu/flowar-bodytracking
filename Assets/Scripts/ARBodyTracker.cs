using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARBodyTracker : MonoBehaviour
{

    [SerializeField]
    GameObject bodyPrefab; 

    [SerializeField]
    Vector3 offset;

    ARHumanBodyManager humanBodyManager;

    GameObject bodyObject;

    void Awake() 
    {
        humanBodyManager = (ARHumanBodyManager)GetComponent<ARHumanBodyManager>();
    }

    private void OnEnable() {
        humanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
    }

    private void OnDisable() {
        humanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
    }

    void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs) {

        foreach (ARHumanBody humanBody in eventArgs.added)
        {
            bodyObject = Instantiate(bodyPrefab, humanBody.transform);
        }

        foreach (ARHumanBody humanBody in eventArgs.updated)
        {
            if (bodyObject != null) 
            {
                bodyObject.transform.position = humanBody.transform.position + offset; // offset만큼 떨어진 위치에 오브젝트 배치
                bodyObject.transform.rotation = humanBody.transform.rotation; 
                bodyObject.transform.localScale = humanBody.transform.localScale;
            }
        }

        foreach (ARHumanBody humanBody in eventArgs.removed)
        {
            if (bodyObject != null)
            {
                Destroy(bodyObject);
            }
        }
    }
}
