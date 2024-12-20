using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memories : MonoBehaviour
{
    [SerializeField] private MemoriesCardInfo[] cards; // Доступные карты
    [SerializeField] private GameObject cardPrefab; // Префаб карты
    [SerializeField] private Transform cardParent; // Родитель для размещения карт
    [SerializeField] private Vector2 gridSize = new Vector2(4, 4); // Размер сетки (столбцы, строки)
    [SerializeField] private float spacing = 1.5f; // Расстояние между картами
    [SerializeField] private float botChance = 0.7f; // Шанс, что бот найдет пару

    private List<MemoriesCard> cardList;
    private List<MemoriesCard> openedCards = new List<MemoriesCard>();
    private Dictionary<string, MemoriesCard> revealedCards = new Dictionary<string, MemoriesCard>();
    private int playerScore = 0;
    private int botScore = 0;
    private bool isCardLocked = false; // Флаг блокировки карт
    private bool isPlayerTurn = true; // Чей ход: true - игрок, false - бот

    private void Start()
    {
        cardList = CreateCardList();
        Shuffle(cardList);
        PlaceCardsOnGrid();
        UpdateTurnInfo();
    }

    private List<MemoriesCard> CreateCardList()
    {
        List<MemoriesCard> list = new List<MemoriesCard>();
        foreach (var card in cards)
        {
            list.Add(CreateCardInstance(card));
            list.Add(CreateCardInstance(card));
        }
        return list;
    }

    private MemoriesCard CreateCardInstance(MemoriesCardInfo cardInfo)
    {
        GameObject cardObject = Instantiate(cardPrefab, cardParent);
        MemoriesCard cardComponent = cardObject.GetComponent<MemoriesCard>();
        cardComponent.info = cardInfo;
        return cardComponent;
    }

    private void Shuffle(List<MemoriesCard> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            MemoriesCard value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void PlaceCardsOnGrid()
    {
        float startX = -(gridSize.x - 1) * spacing / 2;
        float startY = -(gridSize.y - 1) * spacing / 2;

        for (int i = 0; i < cardList.Count; i++)
        {
            int row = i / (int)gridSize.x;
            int col = i % (int)gridSize.x;
            Vector3 position = new Vector3(startX + col * spacing, startY + row * spacing, 0);
            cardList[i].transform.localPosition = position;
        }
    }

    public void CardClicked(MemoriesCard card)
    {
        if (!isPlayerTurn || openedCards.Contains(card) || card.isOpened || isCardLocked) return;

        OpenCardLogic(card);

        if (openedCards.Count == 2)
        {
            StartCoroutine(CheckMatch(() =>
            {
                if (CheckGameEnd()) return;
                if (!isPlayerTurn) StartCoroutine(BotMove());
            }));
        }
    }


    private void OpenCardLogic(MemoriesCard card)
    {
        if (!card.gameObject.activeSelf) return; // Игнорируем неактивные карты
        card.OpenCard();
        openedCards.Add(card);
        if (!revealedCards.ContainsKey(card.info.id))
        {
            revealedCards[card.info.id] = card;
        }
    }


    private IEnumerator CheckMatch(System.Action onComplete)
    {
        isCardLocked = true; // Блокировка карт

        yield return new WaitForSeconds(1f);

        if (openedCards.Count < 2) yield break; // Убедимся, что открыты две карты

        if (openedCards[0].info.id == openedCards[1].info.id)
        {
            Debug.Log("Match found!");
            if (isPlayerTurn) playerScore++;
            else botScore++;

            RemoveCardsFromGame();
        }
        else
        {
            Debug.Log("No match!");
            CloseOpenedCards();
            isPlayerTurn = !isPlayerTurn; // Передача хода
        }

        openedCards.Clear();
        UpdateTurnInfo();

        isCardLocked = false; // Разблокировка карт

        onComplete?.Invoke();
    }



    private void RemoveCardsFromGame()
    {
        foreach (var card in openedCards)
        {
            if (card != null && card.gameObject.activeSelf)
            {
                card.gameObject.SetActive(false);
            }

            // Удаляем карту из словаря открытых карт
            if (revealedCards.ContainsKey(card.info.id))
            {
                revealedCards.Remove(card.info.id);
            }
        }
    }



    private void CloseOpenedCards()
    {
        foreach (var card in openedCards)
        {
            card.CloseCard();
        }
    }

    private bool CheckGameEnd()
    {
        if (playerScore + botScore == cards.Length)
        {
            Debug.Log(playerScore > botScore ? "You Win!" : "You Lose!");
            return true;
        }
        return false;
    }

    private IEnumerator BotMove()
    {
        yield return new WaitForSeconds(1f);

        MemoriesCard firstCard = null;
        MemoriesCard secondCard = null;

        if (Random.value <= botChance && revealedCards.Count >= 2)
        {
            Debug.Log("Bot knows the pair!");
            foreach (var pair in revealedCards)
            {
                var other = FindPair(pair.Value);
                if (other != null)
                {
                    firstCard = pair.Value;
                    secondCard = other;
                    break;
                }
            }
        }

        if (firstCard == null || secondCard == null) // Если бот не нашел пару, выбираем случайно
        {
            Debug.Log("Bot chooses randomly.");
            var availableCards = GetAvailableCards();
            if (availableCards.Count >= 2)
            {
                firstCard = availableCards[Random.Range(0, availableCards.Count)];
                availableCards.Remove(firstCard);
                secondCard = availableCards[Random.Range(0, availableCards.Count)];
            }
        }

        if (firstCard != null && secondCard != null)
        {
            OpenCardLogic(firstCard);
            yield return new WaitForSeconds(0.5f);
            OpenCardLogic(secondCard);
        }

        yield return CheckMatch(() =>
        {
            if (!CheckGameEnd() && !isPlayerTurn)
            {
                StartCoroutine(BotMove()); // Бот продолжает ход, если угадал
            }
        });
    }


    private MemoriesCard FindPair(MemoriesCard card)
    {
        foreach (var other in cardList)
        {
            if (other.info.id == card.info.id && other != card && other.gameObject.activeSelf)
                return other;
        }
        return null;
    }


    private List<MemoriesCard> GetAvailableCards()
    {
        return cardList.FindAll(card => card.gameObject.activeSelf && !card.isOpened);
    }

    private void UpdateTurnInfo()
    {
        Debug.Log(isPlayerTurn ? "Player's turn" : "Bot's turn");
        Debug.Log($"Player Score: {playerScore}, Bot Score: {botScore}");
    }
}
