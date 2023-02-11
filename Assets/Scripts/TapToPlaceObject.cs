using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class TapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public bool destroy = false;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager= GetComponent<ARRaycastManager>();
        aRPlaneManager= GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

        if (IsPointerOverUIObject(finger)) return;

        if (objectToPlace == null) return;

        Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit) && raycastHit.transform.tag == "BuildingBlock")
        {
            if (destroy)
            {
                Destroy(raycastHit.transform.gameObject);
                return;
            }
            else
            {
                var cameraPosition = Camera.main.transform.position;

                var blockPosition = raycastHit.transform.position;
                Transform blockTransform = raycastHit.transform;

                Vector3 rayHitToBlockVector = raycastHit.point - blockTransform.position;
                Pose newBlockPosition = GetPositionToPlace(blockTransform, rayHitToBlockVector);

                GameObject obj = Instantiate(objectToPlace, newBlockPosition.position, newBlockPosition.rotation);

                return;
            }
        }

        if (aRRaycastManager.Raycast((finger.currentTouch.screenPosition), hits, TrackableType.Planes))
        {
            foreach(ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(objectToPlace, pose.position, pose.rotation);
            }
        }
    }

    public void SetDestroy()
    {
        if (!destroy) destroy = true;
        else destroy = false;
    }

    public void SetObjectToPlace(GameObject objectToPlace)
    {
        this.objectToPlace = objectToPlace;
    }

    public void DestroyAllObjects()
    {
        var objects = GameObject.FindGameObjectsWithTag("BuildingBlock");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }
    }

    private bool IsPointerOverUIObject(EnhancedTouch.Finger finger)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(finger.currentTouch.screenPosition.x, finger.screenPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private Pose GetPositionToPlace(Transform blockPosition, Vector3 vectorToBlock)
    {
        Pose ret = new(blockPosition.position, blockPosition.rotation);

        Vector3 absVector = new Vector3(
            Math.Abs(vectorToBlock.x),
            Math.Abs(vectorToBlock.y),
            Math.Abs(vectorToBlock.z)
            );

        float max = Math.Max(absVector.x, Math.Max(absVector.y, absVector.z));

        if (absVector.x == max)
        {
            if (vectorToBlock.x > 0)
            {
                ret.position.x += blockPosition.right.x / 10;
                ret.position.z += blockPosition.right.z / 10;
            }
            else
            {
                ret.position.x -= blockPosition.right.x / 10;
                ret.position.z -= blockPosition.right.z / 10;
            }
        }

        if (absVector.y == max)
        {
            if (vectorToBlock.y > 0) ret.position.y += 0.1f;
            else ret.position.y -= 0.1f;
        }

        if (absVector.z == max)
        {
            if (vectorToBlock.z > 0)
            {
                ret.position.z += blockPosition.forward.z / 10;
                ret.position.x += blockPosition.forward.x / 10;
            }
            else
            {
                ret.position.z -= blockPosition.forward.z / 10;
                ret.position.x -= blockPosition.forward.x / 10;
            }
        }

        return ret;
    }
}
