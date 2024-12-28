using UnityEngine;

public abstract class QuestReward : MonoBehaviour
{
    public ItemPack[] items;
    protected GameObject player;
    protected PlayerStats stats;
    protected Inventory inventory;

    void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        inventory = player.GetComponent<Inventory>();
    }

    void ClaimItems()
    {
        if (items != null && items.Length > 0)
        {
            foreach (ItemPack item in items)
            {
                if (item.item != null)
                    for (int i = 0; i < item.amount; i++)
                        inventory.AddItem(item.item);
            }
        }
    }

    public void Execute()
    {
        Initialize();
        ClaimItems();
        ClaimRewards();
    }

    [System.Serializable]
    public class ItemPack
    {
        public Item item;
        public int amount;

        public ItemPack()
        {
            this.item = null;
            this.amount = 1;
        }

        public ItemPack(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public virtual void ClaimRewards()
    {

    }
}