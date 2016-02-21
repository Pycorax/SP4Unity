using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;

public class TestScoreWebAPI : MonoBehaviour
{
    public string ServerAPIAddress = "http://localhost/api/Score";

    // Use this for initialization
    void Start ()
    {

        send();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void send()
    {
        // Store the form data
        WWWForm form = new WWWForm();
        form.AddField("Name", "UnityTest");
        form.AddField("Score", 456);

        // Send the form data to the ServerAPIAddress
        WWW sender = new WWW(ServerAPIAddress, form);

        ////yield return sender;
        //while (true)
        //{
        //    if (sender.isDone)
        //    {
        //        break;
        //    }
        //}

        //if (!string.IsNullOrEmpty(sender.error))
        //{
        //    print("Error downloading: " + sender.error);
        //}
        //else
        //{
        //    // show the highscores
        //    Debug.Log(sender.text);
        //}

    }

    private void receive()
    {
        // Create a Web Request to the Web API
        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(ServerAPIAddress);

        // Obtain the Result Handle from the API
        var getResult = webReq.GetResponse();

        // Obtain the Stream from the Result Handle
        Stream receiveStream = getResult.GetResponseStream();

        // Pipe the stream to a higher level stream reader with the required encoding format. 
        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        Debug.Log(readStream.ReadToEnd());

        // Close the StreamReader
        readStream.Close();

        // Close the Result Handle
        getResult.Close();
    }
}
