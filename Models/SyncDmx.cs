namespace LightsControl;

public class SyncDmx
{
    public int Address;
    float val;
    public float Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
            Osc.SendDmx(Address, value);
        }
    }

    public SyncDmx(int addr, int value)
    {
        Address = addr;
        Value = value;
    }
}