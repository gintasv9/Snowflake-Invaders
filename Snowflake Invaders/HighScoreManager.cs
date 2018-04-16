using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class HighScoreManager
{
    private const int NUMBER_OF_HIGH_SCORES = 5;
    private readonly int _score;
    private readonly string _path = Directory.GetCurrentDirectory() + @"\Game Data\High_score.txt";
    private List<string> _highScoreList;

    public HighScoreManager()
    {
        _score = GameEngine.FinalScore;
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

        // patogiau butu varda ir score issaugoti kaip struct / ir naudoti json
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
        highScoreNameWindow.AskForUserInput();

        return $"{highScoreNameWindow.PlayerName} - {GameEngine.Score}";
    }
}
