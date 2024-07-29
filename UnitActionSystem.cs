using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    // UnitActionSystem可讀取 不可修改
    public static UnitActionSystem Instance{ get; private set; }
    // 當選取角色替換事件發生
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    // 目前選取角色
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("UnitActionSystem多於1" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() 
    {
        SetSelectedUnit(selectedUnit);    
    }
    
   private void Update() 
   {
        if(isBusy)
        {
            return;
        }
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(TryHandleUnitSelection()) 
        {
            return;
        }
  
        HandleSelectedAction();
   }

   private void HandleSelectedAction()
   {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);

        }
   }
      private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);

        GridSystemVisual.Instance.UpdateGridVisual();
    }


   // 假如滑鼠位置於unit上，觸發SetSelectedUnit將選取unit設為該unit，並回傳true
   private bool TryHandleUnitSelection()
   {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        return false;
                    }
                    if(unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        
        return false;
   } 
   
   private void SetSelectedUnit(Unit unit)
   {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
   }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
   public Unit GetSelectedUnit()
   {
        return selectedUnit;
   }

   public BaseAction GetSelectedAction()
   {
        return selectedAction;
   }
}
