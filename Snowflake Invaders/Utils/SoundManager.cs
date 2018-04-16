using System;
using System.Media;

static class SoundManager
{
    public static bool IsSoundEnabled { get; set; } = true;
    private static SoundPlayer sp;

    public static void LoadMusic(string filename)
    {
        if (IsSoundEnabled)
        {
            sp = new SoundPlayer();
            sp.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + filename;
            sp.PlayLooping();
        }
    }

    public static void EnableOrDisableMusic()
    {
        if (IsSoundEnabled)
        {
            sp.Stop();
            IsSoundEnabled = false;
        }
        else
        {
            sp.PlayLooping();
            IsSoundEnabled = true;
        }
    }
}
