using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

/// <summary>
/// A Singleton HighScoreSystem class that works with initializing and loading the HighScore.
/// You may also use this class to submit a high score to the server.
/// </summary>
public class HighScoreSystem
{
    /// <summary>
    /// Dictates the server API address which is hosted online.
    /// </summary>
    public string ServerAPIAddress = "http://catbang.kahwei.xyz/api/Score";

    private List<ScoreEntry> scores = new List<ScoreEntry>();
    public List<ScoreEntry> Scores { get { return scores; } }

    private static HighScoreSystem instance;

    public static HighScoreSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HighScoreSystem();
            }

            return instance;
        }
    }

    private HighScoreSystem()
    {
        DownloadScores();
    }

    /// <summary>
    /// Use this function to send a score to the server.
    /// </summary>
    /// <param name="score">The ScoreEntry you wish to send.</param>
    public void SendScore(ScoreEntry score)
    {
        // Store the form data
        WWWForm form = new WWWForm();
        form.AddField("Name", score.Name);
        form.AddField("Score", score.Score);

        // Send the form data to the ServerAPIAddress
        WWW sender = new WWW(ServerAPIAddress, form);
    }

    /// <summary>
    /// This downloads the scores and loads it into the scores List<>
    /// </summary>
    public void DownloadScores()
    {
        // Create a Web Request to the Web API
        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(ServerAPIAddress);

        // Obtain the Result Handle from the API
        WebResponse getResult;
        try
        {
            getResult = webReq.GetResponse();
        }
        catch (WebException)
        {
            Debug.Log("Unable to connect to  server!");
            return;
        }

        // Obtain the Stream from the Result Handle
        Stream receiveStream = getResult.GetResponseStream();

        // Pipe the stream to a higher level stream reader with the required encoding format. 
        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        string results = readStream.ReadToEnd();

        // Close the StreamReader
        readStream.Close();

        // Close the Result Handle
        getResult.Close();

        // Clear the current list of scores to prepare to load new ones
        scores.Clear();

        // Process the results by tokenizing
        string entry = "";
        foreach (var c in results)
        {
            // Check for entry delimiter
            if (c == ',')
            {
                // Process the score entry string
                ScoreEntry e = processString(entry);

                // Check if the processed value is valid
                if (e.IsValid)
                {
                    scores.Add(e);
                }

                // Clear the entry for the next item to process
                entry = "";
            }
            else
            {
                entry += c;
            }
        }
        
    }

    /// <summary>
    /// Function to process an individual score entry string into a ScoreEntry object.
    /// The format of a score entry string is as such: [name]-[score]
    /// </summary>
    /// <param name="str">The score entry string to process.</param>
    /// <returns></returns>
    private ScoreEntry processString(string str)
    {
        string name = "";
        string scoreStr = "";
        bool hitDash = false;

        // Tokenize the string
        foreach (var c in str)
        {
            if (hitDash)
            {
                scoreStr += c;
            }
            else
            {
                // Check for property delimiter
                if (c == '-')
                {
                    hitDash = true;
                    continue;
                }
                else
                {
                    name += c;
                }
            }
        }

        // Create the ScoreEntry from what we have
        return new ScoreEntry(name, Convert.ToInt32(scoreStr));
    }
}
