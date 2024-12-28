using System;

public class UIEvents
{
    public event Action onCloseAllScreens;
    public void CloseAllScreens()
    {
        if (onCloseAllScreens != null)
        {
            onCloseAllScreens();
        }
    }
}