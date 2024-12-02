public interface IInteractable
{
    void Interact();
    void OnFocused();  // Метод, который вызывается, когда объект находится в фокусе
    void OnDefocused();  // Метод, который вызывается, когда объект выходит из фокуса
    UnityEngine.Transform GetTransform();  // Метод для получения позиции объекта

}
