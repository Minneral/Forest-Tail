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

    public event Action onMemoriesPlayerWon;
    public void MemoriesPlayerWon()
    {
        if (onMemoriesPlayerWon != null)
        {
            onMemoriesPlayerWon();
        }
    }

    public event Action onMemoriesStart;
    public void MemoriesStart()
    {
        if (onMemoriesStart != null)
        {
            onMemoriesStart();
        }
    }


    public event Action onMemoriesEnd;
    public void MemoriesEnd()
    {
        if (onMemoriesEnd != null)
        {
            onMemoriesEnd();
        }
    }
}