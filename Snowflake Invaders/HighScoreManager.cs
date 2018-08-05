using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class HighScoreManager
{
    private const int NUMBER_OF_HIGH_SCORES = 5;
    private readonly int _score;
    private readonly string _filename = @"High_score.txt";
    private readonly string _directory = Directory.GetCurrentDirectory() + @"\Game Data\";
    private string _path;
    private List<string> _highScoreList;

    public string PlayerName { get; private set; }

    public HighScoreManager()
    {
        _path = _directory + _filename;
        _score = GameEngine.FinalScore;
    }

    public void CreateHighScoreFile()
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
                tw.WriteLine($"{i}. Unknown Player - 0");
            }

            tw.Close();
        }
    }

    public List<string> GetHighScore()
    {
        _highScoreList = new List<string>();
        string line;

        StreamReader file = new StreamReader(_path);

        while ((line = file.ReadLine()) != null)
        {
            _highScoreList.Add(line);
        }

        file.Close();
        return _highScoreList;
    }

    public void SetHighScore()
    {
        List<string> oldHighScore = GetHighScore();
        List<string> attempToHighScore = new List<string>();
        int counter = 1;

        foreach (var scoreLine in oldHighScore)
        {
            int score = GetScoreFromLine(scoreLine);
            string name = GetNameFromLine(scoreLine);

            attempToHighScore.Add($"{name} - {score}");
            counter++;
        }

        int lastScore = GetScoreFromLine(attempToHighScore[attempToHighScore.Count - 1]);
        if (lastScore <= GameEngine.FinalScore && GameEngine.FinalScore != 0)
        {
            attempToHighScore.Add(GetHighScoreInput());
        }

        StreamWriter file = File.CreateText(_path);
        file.Flush();

        // name & score could be saved as struct / json
        var newHighScoreList = from line in attempToHighScore
                               orderby GetScoreFromLine(line) descending
                               select line;

        List<string> finalScores = newHighScoreList.OrderByDescending(x => GetScoreFromLine(x)).ToList();

        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            string text = $"{i + 1}. {finalScores[i]}";
            file.WriteLine(text);
        }

        file.Close();
    }

    public int GetScoreFromLine(string highScoreLine)
    {
        return Convert.ToInt32(highScoreLine.Substring(highScoreLine.LastIndexOf(" - ") + 3));
    }

    public string GetNameFromLine(string highScoreLine)
    {
        if (highScoreLine.Contains(". "))
        {
            return highScoreLine.Substring(highScoreLine.LastIndexOf(". ") + 2, highScoreLine.LastIndexOf(" - ") - (highScoreLine.LastIndexOf(". ") + 2));
        }
        else
        {
            return highScoreLine.Substring(0, highScoreLine.LastIndexOf(" - "));
        }
    }

    public string GetHighScoreInput()
    {
        HighScoreNameWindow highScoreNameWindow = new HighScoreNameWindow();
        highScoreNameWindow.Render();

        Console.SetCursorPosition((GameEngine.GameScreenWidth - 10) / 2, 8);
        Console.CursorVisible = true;

        PlayerName = CheckForValidInputAndCorrect(Console.ReadLine(), out string errorMsg);
        if (PlayerName == null)
        {
            Console.SetCursorPosition((GameEngine.GameScreenWidth - errorMsg.Length) / 2, 9);
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMsg);
            Console.ResetColor();
            Thread.Sleep(1000);
            GetHighScoreInput();
        }

        Console.CursorVisible = false;

        return $"{PlayerName} - {GameEngine.Score}";
    }

    private string CheckForValidInputAndCorrect(string playerName, out string errorMsg)
    {
        errorMsg = "Maximum player name length is 20 symbols!";

        if (string.IsNullOrWhiteSpace(playerName))
        {
            return "Unknown Player";
        }

        if (playerName.Length > 20)
        {
            return null;
        }
        else if (playerName.Contains('-') || playerName.Contains('.'))
        {
            var correctedPlayerName = from x in playerName
                                      where x != '-' && x != '.'
                                      select x;

            return string.Join("", correctedPlayerName);
        }

        return playerName;
    }
}
