using LightsControl.Shared;

namespace LightsControl;

public enum Page
{
    Functions = 0,
    Advanced = 1,
}

public static class Global
{
    public static Page Page;
    public static MainLayout MainLayout;
}