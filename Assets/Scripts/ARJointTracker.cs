using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class ARJointTracker : MonoBehaviour
{
    ARHumanBodyManager arHumanBodyManager;

    [SerializeField]
    GameObject jointPrefab;

    Dictionary<int, GameObject> jointObjects;

    void Awake()
    {
        arHumanBodyManager = GetComponent<ARHumanBodyManager>();
        jointObjects = new Dictionary<int, GameObject>();
    }

    private void OnEnable()
    {
        arHumanBodyManager.humanBodiesChanged += OnHumanBodiedChanged;
    }

    private void OnDisable()
    {
        arHumanBodyManager.humanBodiesChanged -= OnHumanBodiedChanged;
    }

    void OnHumanBodiedChanged(ARHumanBodiesChangedEventArgs eventArgs)
    {
        foreach (ARHumanBody humanBody in eventArgs.updated)
        {
            NativeArray<XRHumanBodyJoint> joints = humanBody.joints;

            foreach(XRHumanBodyJoint joint in joints)
            {
                GameObject obj;
                if (!jointObjects.TryGetValue(joint.index, out obj))
                {
                    obj = Instantiate(jointPrefab);
                    jointObjects.Add(joint.index, obj);
                }

                if (joint.tracked)
                {
                    // update joint transform
                    obj.transform.parent = humanBody.transform;
                    obj.transform.localPosition = joint.anchorPose.position * humanBody.estimatedHeightScaleFactor;
                    obj.transform.localRotation = joint.anchorPose.rotation;
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
