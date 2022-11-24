using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardStashView : MonoBehaviour, IVariableInventoryView
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] Graphic condition;
        [SerializeField] RectTransform conditionTransform;
        [SerializeField] RectTransform background;

        [SerializeField] float holdScrollPadding;
        [SerializeField] float holdScrollRate;

        [SerializeField] Color defaultColor;
        [SerializeField] Color positiveColor;
        [SerializeField] Color negativeColor;

        public StandardStashViewData StashData { get; private set; }

        public int CellCount => StashData.CapacityWidth * StashData.CapacityHeight;

        protected IVariableInventoryCell[] itemViews;
        protected CellCorner cellCorner;

        int? originalId;
        IVariableInventoryCellData originalCellData;
        Vector3 conditionOffset;

        Action<IVariableInventoryCell> onCellClick;
        Action<IVariableInventoryCell> onCellOptionClick;
        Action<IVariableInventoryCell> onCellEnter;
        Action<IVariableInventoryCell> onCellExit;

        public void SetCellCallback(
            Action<IVariableInventoryCell> onCellClick,
            Action<IVariableInventoryCell> onCellOptionClick,
            Action<IVariableInventoryCell> onCellEnter,
            Action<IVariableInventoryCell> onCellExit)
        {
            this.onCellClick = onCellClick;
            this.onCellOptionClick = onCellOptionClick;
            this.onCellEnter = onCellEnter;
            this.onCellExit = onCellExit;
        }

        public virtual void Apply(IVariableInventoryViewData data)
        {
            StashData = ((StandardStashViewData)data);

            if (itemViews == null || itemViews.Length != CellCount)
            {
                itemViews = new IVariableInventoryCell[CellCount];

                for (var i = 0; i < CellCount; i++)
                {
                    var itemView = Instantiate(cellPrefab, gridLayoutGroup.transform).GetComponent<StandardCell>();
                    itemViews[i] = itemView;

                    itemView.transform.SetAsFirstSibling();
                    itemView.SetCellCallback(
                        onCellClick,
                        onCellOptionClick,
                        onCellEnter,
                        onCellExit,
                        _ => scrollRect.enabled = false,
                        _ => scrollRect.enabled = true);
                    itemView.Apply(null);
                }

                background.SetAsFirstSibling();

                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = StashData.CapacityWidth;
                gridLayoutGroup.cellSize = itemViews.First().DefaultCellSize;
                gridLayoutGroup.spacing = itemViews.First().MargineSpace;
            }

            for (var i = 0; i < StashData.CellData.Length; i++)
            {
                itemViews[i].Apply(StashData.CellData[i]);
            }
        }

        public virtual void ReApply()
        {
            if (!StashData.IsDirty)
            {
                return;
            }

            Apply(StashData);
            StashData.IsDirty = false;
        }

        public virtual void OnPrePick(IVariableInventoryCell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return;
            }

            var (width, height) = GetRotateSize(stareCell.CellData);
            conditionTransform.sizeDelta = new Vector2(stareCell.DefaultCellSize.x * width, stareCell.DefaultCellSize.y * height);
        }

        public virtual bool OnPick(IVariableInventoryCell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return false;
            }

            var id = StashData.GetId(stareCell.CellData);
            if (id.HasValue)
            {
                originalId = id;
                originalCellData = stareCell.CellData;

                itemViews[id.Value].Apply(null);
                StashData.InsertInventoryItem(id.Value, null);
                return true;
            }

            return false;
        }

        public virtual void OnDrag(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell, PointerEventData pointerEventData)
        {
            if (stareCell == null)
            {
                return;
            }

            // auto scroll
            var pointerViewportPosition = GetLocalPosition(scrollRect.viewport, pointerEventData.position, pointerEventData.enterEventCamera);

            if (pointerViewportPosition.y < scrollRect.viewport.rect.min.y + holdScrollPadding)
            {
                var scrollValue = scrollRect.verticalNormalizedPosition * scrollRect.viewport.rect.height;
                scrollValue -= holdScrollRate;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollValue / scrollRect.viewport.rect.height);
            }

            if (pointerViewportPosition.y > scrollRect.viewport.rect.max.y - holdScrollPadding)
            {
                var scrollValue = scrollRect.verticalNormalizedPosition * scrollRect.viewport.rect.height;
                scrollValue += holdScrollRate;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollValue / scrollRect.viewport.rect.height);
            }

            // depends on anchor
            var pointerLocalPosition = GetLocalPosition(stareCell.RectTransform, pointerEventData.position, pointerEventData.enterEventCamera);
            var anchor = new Vector2(stareCell.DefaultCellSize.x * 0.5f, -stareCell.DefaultCellSize.y * 0.5f);
            var anchoredPosition = pointerLocalPosition + anchor;
            conditionOffset = new Vector3(
                Mathf.Floor(anchoredPosition.x / stareCell.DefaultCellSize.x) * stareCell.DefaultCellSize.x,
                Mathf.Ceil(anchoredPosition.y / stareCell.DefaultCellSize.y) * stareCell.DefaultCellSize.y);

            // cell corner
            var prevCorner = cellCorner;
            cellCorner = GetCorner((new Vector2(anchoredPosition.x % stareCell.DefaultCellSize.x, anchoredPosition.y % stareCell.DefaultCellSize.y) - anchor) * 0.5f);

            // shift the position only even number size
            var (width, height) = GetRotateSize(effectCell.CellData);
            var evenNumberOffset = GetEvenNumberOffset(width, height, stareCell.DefaultCellSize.x * 0.5f, stareCell.DefaultCellSize.y * 0.5f);
            conditionTransform.position = stareCell.RectTransform.position + ((conditionOffset + evenNumberOffset) * stareCell.RectTransform.lossyScale.x);

            // update condition
            if (prevCorner != cellCorner)
            {
                UpdateCondition(stareCell, effectCell);
            }
        }

        public virtual bool OnDrop(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (!itemViews.Any(item => item == stareCell))
            {
                return false;
            }

            // check target;
            var index = GetIndex(stareCell, effectCell.CellData, cellCorner);
            if (!index.HasValue)
            {
                return false;
            }

            if (!StashData.CheckInsert(index.Value, effectCell.CellData))
            {
                // check free space in case
                if (stareCell.CellData != null && stareCell.CellData is VariableInventorySystem.IStandardCaseCellData caseData)
                {
                    var id = caseData.CaseData.GetInsertableId(effectCell.CellData);
                    if (id.HasValue)
                    {
                        caseData.CaseData.InsertInventoryItem(id.Value, effectCell.CellData);

                        originalId = null;
                        originalCellData = null;
                        return true;
                    }
                }

                return false;
            }

            // place
            StashData.InsertInventoryItem(index.Value, effectCell.CellData);
            itemViews[index.Value].Apply(effectCell.CellData);

            originalId = null;
            originalCellData = null;
            return true;
        }

        public virtual void OnDropped(bool isDropped)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;

            if (!isDropped && originalId.HasValue)
            {
                // revert
                itemViews[originalId.Value].Apply(originalCellData);
                StashData.InsertInventoryItem(originalId.Value, originalCellData);
            }

            originalId = null;
            originalCellData = null;
        }

        public virtual void OnCellEnter(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            conditionTransform.gameObject.SetActive(effectCell?.CellData != null);
            (stareCell as StandardCell).SetHighLight(true);
        }

        public virtual void OnCellExit(IVariableInventoryCell stareCell)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;

            cellCorner = CellCorner.None;

            (stareCell as StandardCell).SetHighLight(false);
        }

        public virtual void OnSwitchRotate(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (stareCell == null)
            {
                return;
            }

            var (width, height) = GetRotateSize(effectCell.CellData);
            conditionTransform.sizeDelta = new Vector2(effectCell.DefaultCellSize.x * width, effectCell.DefaultCellSize.y * height);

            var evenNumberOffset = GetEvenNumberOffset(width, height, stareCell.DefaultCellSize.x * 0.5f, stareCell.DefaultCellSize.y * 0.5f);
            conditionTransform.position = stareCell.RectTransform.position + ((conditionOffset + evenNumberOffset) * stareCell.RectTransform.lossyScale.x);

            UpdateCondition(stareCell, effectCell);
        }

        protected virtual int? GetIndex(IVariableInventoryCell stareCell)
        {
            var index = (int?)null;
            for (var i = 0; i < itemViews.Length; i++)
            {
                if (itemViews[i] == stareCell)
                {
                    index = i;
                }
            }

            return index;
        }

        protected virtual int? GetIndex(IVariableInventoryCell stareCell, IVariableInventoryCellData effectCellData, CellCorner cellCorner)
        {
            var index = GetIndex(stareCell);

            // offset index
            var (width, height) = GetRotateSize(effectCellData);
            if (width % 2 == 0)
            {
                if ((cellCorner & CellCorner.Left) != CellCorner.None)
                {
                    index--;
                }
            }

            if (height % 2 == 0)
            {
                if ((cellCorner & CellCorner.Top) != CellCorner.None)
                {
                    index -= StashData.CapacityWidth;
                }
            }

            index -= (width - 1) / 2;
            index -= (height - 1) / 2 * StashData.CapacityWidth;
            return index;
        }

        protected virtual (int, int) GetRotateSize(IVariableInventoryCellData cell)
        {
            if (cell == null)
            {
                return (1, 1);
            }

            return (cell.IsRotate ? cell.Height : cell.Width, cell.IsRotate ? cell.Width : cell.Height);
        }

        protected virtual Vector2 GetLocalPosition(RectTransform parent, Vector2 position, Camera camera)
        {
            var localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, camera, out localPosition);
            return localPosition;
        }

        protected virtual CellCorner GetCorner(Vector2 localPosition)
        {
            // depends on pivot
            var corner = CellCorner.None;
            if (localPosition.x < Mathf.Epsilon)
            {
                corner |= CellCorner.Left;
            }

            if (localPosition.x > Mathf.Epsilon)
            {
                corner |= CellCorner.Right;
            }

            if (localPosition.y > Mathf.Epsilon)
            {
                corner |= CellCorner.Top;
            }

            if (localPosition.y < Mathf.Epsilon)
            {
                corner |= CellCorner.Bottom;
            }

            return corner;
        }

        protected virtual Vector3 GetEvenNumberOffset(int width, int height, float widthOffset, float heightOffset)
        {
            var evenNumberOffset = Vector3.zero;

            if (width % 2 == 0)
            {
                if ((cellCorner & CellCorner.Left) != CellCorner.None)
                {
                    evenNumberOffset.x -= widthOffset;
                }

                if ((cellCorner & CellCorner.Right) != CellCorner.None)
                {
                    evenNumberOffset.x += widthOffset;
                }
            }

            if (height % 2 == 0)
            {
                if ((cellCorner & CellCorner.Top) != CellCorner.None)
                {
                    evenNumberOffset.y += heightOffset;
                }

                if ((cellCorner & CellCorner.Bottom) != CellCorner.None)
                {
                    evenNumberOffset.y -= heightOffset;
                }
            }

            return evenNumberOffset;
        }

        protected virtual void UpdateCondition(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            var index = GetIndex(stareCell, effectCell.CellData, cellCorner);
            if ((index.HasValue && StashData.CheckInsert(index.Value, effectCell.CellData)))
            {
                condition.color = positiveColor;
            }
            else
            {
                // check free space in case
                if (stareCell.CellData != null &&
                    stareCell.CellData is VariableInventorySystem.IStandardCaseCellData caseData &&
                    caseData.CaseData.GetInsertableId(effectCell.CellData).HasValue)
                {
                    condition.color = positiveColor;
                }
                else
                {
                    condition.color = negativeColor;
                }
            }
        }
    }
}
