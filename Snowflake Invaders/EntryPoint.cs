using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "* SNOWFLAKE INVADERS - Dodge them all! *";

        SoundManager.LoadMusic(@"\Music\M.O.O.N. - 'Hydrogen'.wav");

        try
        {
            SafeNativeMethods.DisableConsoleResize();
            SafeNativeMethods.CenterConsole();
            SafeNativeMethods.DisableConsoleMouseClicks();

            HighScoreManager hsm = new HighScoreManager();
            hsm.CreateHighScoreFile();

            GuiController guiController = new GuiController();
            guiController.ShowMenu();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
