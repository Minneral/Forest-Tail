using System;

public class PuzzleEvents
{
    public event Action onMemoriesCardOpen;
    public void MemoriesCardOpen()
    {
        if (onMemoriesCardOpen != null)
        {
            onMemoriesCardOpen();
        }
    }
}