using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
{
    public float AngleDragThreshold;
    public float DragThreshold;

    protected Vector2 startPos;
    protected MapTraversal.MapTraversalDirectionsIndex dragDirection = MapTraversal.MapTraversalDirectionsIndex.None;
    protected MapTraversal.MapTraversalDirectionsIndex dragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.None;

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag(eventData)) return;

        var direction = (startPos - eventData.position).normalized;
        var dirX = direction.x;
        var dirY = direction.y;

        var difx = 1 - Mathf.Abs(dirX);
        var dify = 1 - Mathf.Abs(dirY);

        if (difx < dify)
        {
            // X direction
            if (dirX > 0) TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Drag Left");
            if (dirX < 0) TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Drag Right");
        }
        else if (difx > dify)
        {
            // Y direction
            if (dirY < 0) TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Drag Up");
            if (dirY > 0) TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Drag Down");
        }      
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //if (!CanDrag(eventData)) return;
        //TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "OnBeginDrag");
        startPos = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //if (!CanDrag(eventData)) return;
        //TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "OnEndDrag");
    }

    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
        //if (!CanDrag(eventData)) return;

        eventData.useDragThreshold = false;
    }

    protected virtual bool CanDrag(PointerEventData eventData)
    {
        return true;
    }
}

