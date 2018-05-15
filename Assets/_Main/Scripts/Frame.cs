﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Frame : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Animator animator;

    private readonly int HashActive = Animator.StringToHash("Active");
    private readonly int HashHoverOn = Animator.StringToHash("HoverOn");


    public void Awake()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        startPosition = transform.position;
        animator.SetBool(HashActive, true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = (Vector2)position;

        Frame frame;

        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        if (FrameCollection.Instance.FrameContainsPosition(this, Input.mousePosition, out frame))
        {
            frame.animator.SetBool(HashHoverOn, true);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Frame frame;

        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        if (FrameCollection.Instance.FrameContainsPosition(this, Input.mousePosition, out frame))
        {
            FrameCollection.Instance.SwitchBetween(this, frame);
        }
        else
        {
            transform.position = startPosition;
        }

        animator.SetBool(HashActive, false);
    }

}
