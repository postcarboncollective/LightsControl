namespace LightsControl;

public class SyncInt
{
    int val;
    public int Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
        }
    }

    public SyncInt(int value)
    {
        Value = value;
    }
}