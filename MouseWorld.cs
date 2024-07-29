using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaaneLayerMask;

    private void Awake() 
    {
        instance = this;
    }

    // 抓取滑鼠位於遊戲中地板的位置
    public static Vector3 GetPosition()
    {
        //從攝影機位置倒數標位置的雷射
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        // 雷射接觸到地板(mousePlaaneLayerMask)將資料保存於ratcastHit
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaaneLayerMask);
        return raycastHit.point;
    }
}
