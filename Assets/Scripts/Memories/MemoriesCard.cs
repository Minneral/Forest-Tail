using UnityEngine;
using UnityEngine.UI;

public class MemoriesCard : MonoBehaviour
{
    [HideInInspector] public MemoriesCardInfo info;
    public bool isOpened { get; private set; }

    [SerializeField] private Image cardImage; // Ссылка на компонент Image
    [SerializeField] private Button cardButton; // Ссылка на компонент Button
    [SerializeField] private Sprite cardBackImage; // Задняя сторона карты

    private void Start()
    {
        ResetCard();
        cardButton.onClick.AddListener(OnCardClick);
    }

    public void OpenCard()
    {
        if (isOpened) return;
        isOpened = true;
        cardImage.sprite = info.cardImage; // Показываем лицевую сторону
    }

    public void CloseCard()
    {
        if (!isOpened) return;
        isOpened = false;
        cardImage.sprite = cardBackImage; // Показываем обратную сторону
    }

    public void ResetCard()
    {
        isOpened = false;
        cardImage.sprite = cardBackImage;
    }

    private void OnCardClick()
    {
        FindObjectOfType<Memories>().CardClicked(this);
    }
}
