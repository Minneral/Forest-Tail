using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorTracker : MonoBehaviour
{
    public Canvas canvas; // Основной Canvas, к которому относятся UI-объекты

    void Update()
    {
        // Проверяем, есть ли EventSystem в сцене
        if (EventSystem.current == null) return;

        // Создаем список для хранения всех объектов, пересекаемых курсором
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // Выполняем Raycast
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // Проверяем пересекаемые объекты
        if (raycastResults.Count > 0)
        {
            // Самый верхний объект (первый в списке)
            GameObject topObject = raycastResults[0].gameObject;
            Debug.Log("Курсор над объектом: " + topObject.name);
        }
    }
}
