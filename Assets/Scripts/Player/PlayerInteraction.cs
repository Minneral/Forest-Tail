using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Animator _animator;
    private Transform headBone; // Ссылка на трансформ головы

    private Transform focus;  // Цель фокуса
    private IInteractable currentFocus;  // Текущий объект фокуса


    private Transform rightHandBone; // Ссылка на трансформ правой руки
    public KeyCode interactKey = KeyCode.E; // Клавиша для взаимодействия
    public float handReachDuration = 1.0f; // Время для движения руки к объекту
    private bool isHandMoving = false;  // Флаг для проверки, тянется ли рука

    void Start()
    {
        try
        {
            _animator = GetComponent<Animator>();

            if (_animator == null)
                throw new MissingComponentException(nameof(Animator), gameObject.name, GetType().Name, "Make sure you have assigned AnimatorController component to player");

            headBone = _animator.GetBoneTransform(HumanBodyBones.Head);
            rightHandBone = _animator.GetBoneTransform(HumanBodyBones.RightHand);
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentFocus != null)
        {
            StartCoroutine(RemoveObject());  // Поднять руку и удалить объект
        }

        if (currentFocus != null && focus == null)
        {
            RemoveFocus();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        // Проверяем, реализует ли объект интерфейс IInteractable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
        Debug.Log("FOcused");

            SetFocus(interactable);
        }
    }
    // Обработка выхода из триггера 
    private void OnTriggerExit(Collider other)
    {
        // Проверяем, реализует ли объект интерфейс IInteractable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            RemoveFocus();
        }
    }

    // Устанавливаем фокус на объекте взаимодействия
    void SetFocus(IInteractable newFocus)
    {
        if (newFocus != currentFocus)  // Если новый фокус отличается от текущего
        {
            if (currentFocus != null)
            {
                currentFocus.OnDefocused();  // Если был предыдущий фокус, вызываем его завершение
            }

            currentFocus = newFocus;
            focus = currentFocus.GetTransform();  // Получаем объект, на котором будет фокус
            currentFocus.OnFocused();
        }
    }

    // Убираем фокус с объекта
    void RemoveFocus()
    {
        if (currentFocus != null)
        {
            currentFocus.OnDefocused();  // Сообщаем объекту о снятии фокуса
        }

        focus = null;
        currentFocus = null;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (currentFocus != null && focus != null)
        {
            // Персонаж смотрит на объект
            _animator.SetLookAtWeight(1f, 0f, 1f, 0.5f, 0.5f);
            _animator.SetLookAtPosition(focus.position);

            // Если рука движется к объекту, активируем IK для руки
            if (isHandMoving)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, focus.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, focus.rotation);
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
        else
        {
            // Отключаем IK, если нет фокуса
            _animator.SetLookAtWeight(0f);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }

    }
    IEnumerator RemoveObject()
    {
        isHandMoving = true;  // Активируем IK для руки

        yield return new WaitForSeconds(handReachDuration);  // Ждем, пока рука достигнет объекта

        if (currentFocus != null)
        {
            currentFocus.Interact(); // Выполняем взаимодействие с объектом
            RemoveFocus(); // Убираем фокус после взаимодействия
        }

        isHandMoving = false;  // Отключаем IK для руки
    }

}
