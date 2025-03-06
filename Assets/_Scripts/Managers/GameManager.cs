public static class GameManager
{
    public static bool IsPlaying = false;
    public static bool IsGameover = false;
    public static bool IsUiOpened = false;

    public static double mileage = 0f;
    public static double mileageEnd = 0f;

    public static uint coin = 0;

    public static int life = 3;

    public static PlayerState playerState;

    public static void Reset()
    {
        IsPlaying = false;
        IsGameover = false;
        IsUiOpened = false;
        mileage = 0f;
        mileageEnd = 0f;
        coin = 0;
        life = 3;
        playerState = 0;
    }
}