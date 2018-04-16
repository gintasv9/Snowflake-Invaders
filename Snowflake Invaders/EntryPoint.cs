using System;
using System.IO;
using System.Text;

class Program
{
    private static readonly string _filename = @"High_score.txt";
    private static readonly string _directory = Directory.GetCurrentDirectory() + @"\Game Data\";

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "* SNOWFLAKE INVADERS - Dodge them all! *";

        SafeNativeMethods.DisableConsoleResize();
        SafeNativeMethods.CenterConsole();

        CreateHighScoreFile();

        SoundManager.LoadMusic(@"\Music\M.O.O.N. - 'Hydrogen'.wav");

        GuiController guiController = new GuiController();
        guiController.ShowMenu();
    }

    static void CreateHighScoreFile()
    {
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }

        string path = _directory + _filename;
        if (!File.Exists(path))
        {
            File.Create(path).Close();
            TextWriter tw = File.AppendText(path);

            for (int i = 1; i <= 5; i++)
            {
                tw.WriteLine($"{i}. Unknown - 0");
            }

            tw.Close();
        }
    }
}
