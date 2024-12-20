using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARInteractionsManager : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject aRPointer;
    private GameObject item3DModel;
    private GameObject itemSelected;
    private bool isInitialPosition;
    private bool isOverUI;
    private Vector2 initialTouchPos;
    private bool isOver3DModel;
    public GameObject Item3DModel
    {
        set
        {
            item3DModel = value;
            item3DModel.transform.position = aRPointer.transform.position;
            item3DModel.transform.parent = aRPointer.transform;
            isInitialPosition = true;
        }
    }

    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindAnyObjectByType<ARRaycastManager>();
        GameManager.instance.OnMainMenu += SetItemPosition;
    }

    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                aRPointer.SetActive(true);
                isInitialPosition = false;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                isOverUI = isTapOverUI(touchPosition);
                isOver3DModel = isTapOver3dModel(touchPosition);
            }

            if (touchOne.phase == TouchPhase.Moved)
            {
                if (aRRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    if (!isOverUI && isOver3DModel)
                    {
                        transform.position = hitPose.position;
                    }
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touchtwo = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Began || touchtwo.phase == TouchPhase.Began)
                {
                    initialTouchPos = touchtwo.position - touchOne.position;
                }
                if(touchOne.phase == TouchPhase.Moved || touchtwo.phase == TouchPhase.Began)
                {
                    Vector2 currentTouchPos = touchtwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPos,currentTouchPos);
                    item3DModel.transform.rotation = Quaternion.Euler(0,item3DModel.transform.eulerAngles.y - angle,0);
                    initialTouchPos = currentTouchPos;
                }
            }
            if (isOver3DModel && item3DModel == null && !isOverUI)
            {
                GameManager.instance.ARPosition();
                item3DModel = itemSelected;
                itemSelected = null;
                aRPointer.SetActive(true);
                transform.position = item3DModel.transform.position;
                item3DModel.transform.parent  =aRPointer.transform;
            }
        }
    }

    private bool isTapOver3dModel(Vector2 touchPosition)
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);
        if(Physics.Raycast(ray,out RaycastHit hit3DModel));
        {
            if(hit3DModel.collider.CompareTag("item"))
            {
                itemSelected = hit3DModel.transform.gameObject;
                return true;
            }
        }
        return false;
    }

    private bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;
    }

    private void SetItemPosition()
    {
        if (item3DModel != null)
        {
            item3DModel.transform.parent = null;
            aRPointer.SetActive(false);
            item3DModel = null;
        }
    }
    public void DeleteItem()
    {
        Destroy(item3DModel);
        aRPointer.SetActive(false);
        GameManager.instance.MainMenu();
    }
}
