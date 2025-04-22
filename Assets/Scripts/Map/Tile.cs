using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : DragItem
{
    public Image MyImageBorder;
    public Image MyImageFill;

    public TextMeshProUGUI ValueText;
    public Image ValueImage;

    public int Value;
    [SerializeField]
    public MapSector Sector;

    public Sprite UsedSprite;
    public Sprite ValueOrthogonalSprite;
    public Sprite ValueDiagonalSprite;
    public Sprite ValueAllSprite;

    public Sprite VerticalRookSprite;
    public Sprite HorizontalRookSprite;

    public Sprite EmptySprite;

    public bool IsErrorHighlight = false;

    protected List<MapSectorType> dragAllowed = new List<MapSectorType>() { MapSectorType.ValueOrthogonal, MapSectorType.ValueDiagonal, MapSectorType.ValueAll, MapSectorType.VerticalRook, MapSectorType.HorizontalRook };

    public virtual void SetSector(MapSector sector)
    {
        Sector = sector;
        Sector.SectorTile = this;
        SetImage();
        SetText();
    }
    public virtual void TempAssigned()
    {
        Sector.SetSectorState(MapSectorState.TempAssigned);
    }
    public virtual void Assigned()
    {
        Sector.SetSectorState(MapSectorState.Assigned);
        if (GameSettings.Sound) AudioManager.Instance.Play("Click2");
    }

    public virtual void HighlightOn()
    {
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderHighlightColor) != null) MyImageBorder.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderHighlightColor);
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileFillHighlightColor) != null) MyImageFill.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileFillHighlightColor);
        IsErrorHighlight = false;
    }

    public virtual void HighlightNormal()
    {
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderNormalColor) != null) MyImageBorder.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderNormalColor);
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileFillNormalColor) != null) MyImageFill.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileFillNormalColor);
        IsErrorHighlight = false;
    }

    public virtual void HighlightError()
    {
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderErrorColor) != null) MyImageBorder.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderErrorColor);
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileFillErrorColor) != null) MyImageFill.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileFillErrorColor);
        IsErrorHighlight = true;
    }

    public virtual void HighlightAssigned()
    {
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderAssignedColor) != null) MyImageBorder.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileBorderAssignedColor);
        if (ColorManager.GetColorByName(ColorManager.ColorNames.TileFillAssignedColor) != null) MyImageFill.color = ColorManager.GetColorByName(ColorManager.ColorNames.TileFillAssignedColor); ;
        IsErrorHighlight = false;
    }

    protected virtual void SetImage()
    {
        if (Sector.Type == MapSectorType.None)
        {
            MyImageBorder.enabled = false;
            MyImageFill.enabled = false;
        }
        else
        {
            MyImageBorder.enabled = true;
            MyImageFill.enabled = true;
        }

        if (Sector.State == MapSectorState.Assigned)
        {
            HighlightAssigned();
        }
        else
        {
            HighlightNormal();
        }

        if (Sector.Type == MapSectorType.ValueOrthogonal || Sector.Type == MapSectorType.VerticalRook || Sector.Type == MapSectorType.HorizontalRook)
        {
            MyImageFill.sprite = ValueOrthogonalSprite;
        }
        else if (Sector.Type == MapSectorType.ValueDiagonal)
        {
            MyImageFill.sprite = ValueDiagonalSprite;
        }
        else if (Sector.Type == MapSectorType.ValueAll)
        {
            MyImageFill.sprite = ValueAllSprite;
        }
        else if (Sector.Type == MapSectorType.Used)
        {
            MyImageFill.sprite = UsedSprite;
        }
        else
        {
            //MyImageFill.sprite = EmptySprite;
        }

    }

    protected virtual void SetText()
    {
        Value = Sector.Value;
        if ((Value > 0 && Sector.State == MapSectorState.Used) &&
            (Sector.Type == MapSectorType.ValueOrthogonal || Sector.Type == MapSectorType.ValueDiagonal || Sector.Type == MapSectorType.ValueAll))
        {
            ValueImage.enabled = false;
            ValueText.text = Value.ToString();
            ValueText.enabled = true;
        }
        else if ((Value <= 0 && Sector.State == MapSectorState.Used) &&
                (Sector.Type == MapSectorType.VerticalRook || Sector.Type == MapSectorType.HorizontalRook))
        {
            if (Sector.Type == MapSectorType.HorizontalRook) ValueText.text = "H";
            if (Sector.Type == MapSectorType.VerticalRook) ValueText.text = "I";
            ValueText.enabled = true;
            ValueImage.enabled = false;
            //if (Sector.Type == MapSectorType.VerticalRook) ValueImage.sprite = VerticalRookSprite;
            //else if (Sector.Type == MapSectorType.HorizontalRook) ValueImage.sprite = HorizontalRookSprite;

            //ValueImage.enabled = true;
        }
        else
        {
            ValueText.text = "";
            ValueText.enabled = false;
            ValueImage.enabled = false;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag(eventData))
        {
            dragDirection = MapTraversal.MapTraversalDirectionsIndex.None;
            dragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.None;
            GameEventSystem.Drag_OnDragStop(false);
            return;
        }

        var direction = (startPos - eventData.position).normalized;
        var dirX = direction.x;
        var dirY = direction.y;

        var difx = 1 - Mathf.Abs(dirX);
        var dify = 1 - Mathf.Abs(dirY);

        var newDragDirection = MapTraversal.MapTraversalDirectionsIndex.None;
        var newDragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.None;

        var angleThreshold = AngleDragThreshold;
        if (Sector.Type == MapSectorType.ValueAll) angleThreshold += 0.25f;

        if (dirX > -angleThreshold && dirX < angleThreshold && dirY > 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.Down;
        else if (dirX > -angleThreshold && dirX < angleThreshold && dirY < 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.Up;
        else if(dirX > 0 && dirY > -angleThreshold && dirY < angleThreshold) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.Left;
        else if(dirX < 0 && dirY > -angleThreshold && dirY < angleThreshold) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.Right;
        else if(dirX > 0 && dirY > 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.DownLeft;
        else if (dirX > 0 && dirY < 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.UpLeft;
        else if (dirX < 0 && dirY < 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.UpRight;
        else if (dirX < 0 && dirY > 0) newDragDirection = MapTraversal.MapTraversalDirectionsIndex.DownRight;

        if (difx < dify)
        {
            // X direction
            if (dirX > 0) newDragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.Left;
            if (dirX < 0) newDragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.Right;
        }
        else if (difx > dify)
        {
            // Y direction
            if (dirY < 0) newDragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.Up;
            if (dirY > 0) newDragOrthDirection = MapTraversal.MapTraversalDirectionsIndex.Down;
        }

        if (newDragDirection == MapTraversal.MapTraversalDirectionsIndex.None || newDragOrthDirection == MapTraversal.MapTraversalDirectionsIndex.None ||
             (newDragDirection == dragDirection && newDragOrthDirection == dragOrthDirection) || 
             Sector == null) return;

        GameEventSystem.Drag_OnDragStop(false);

        dragDirection = newDragDirection;
        dragOrthDirection = newDragOrthDirection;

        GameEventSystem.Drag_OnDragStart(Sector, dragDirection, dragOrthDirection);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        //if (!CanDrag(eventData)) return;

        dragDirection = MapTraversal.MapTraversalDirectionsIndex.None;

        GameEventSystem.Drag_OnDragStop(CanDrag(eventData));
    }

    protected override bool CanDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(eventData.pressPosition, eventData.position) < DragThreshold) return false;

        if (Sector == null) return false;

        if (Sector.Type == MapSectorType.None || Sector.State != MapSectorState.Used) return false;

        return dragAllowed.Contains(Sector.Type);
    }
}
