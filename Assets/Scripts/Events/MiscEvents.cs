using System;

public class MiscEvents
{
    public event Action onCoinCollected;
    public void CoinCollected() 
    {
        if (onCoinCollected != null) 
        {
            onCoinCollected();
        }
    }

    public event Action onAppleCollected;
    public void AppleCollected() 
    {
        if (onAppleCollected != null) 
        {
            onAppleCollected();
        }
    }
}