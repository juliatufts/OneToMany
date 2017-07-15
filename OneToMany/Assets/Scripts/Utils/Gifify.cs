using UnityEngine;
using System.Collections;
using System.IO;

/*
	Gifify.cs
	
	Author: Kevin "Gaeel" Bradshaw
	
	Drop this into a UnityProject, attach it to a GameObject, customise the values if necessary, and then hold down the key you selected to capture images that you can gifify later.

	YoinkPL v0.1:
	All software in this project under YoinkPL unless otherwise noted.
	YoinkPL allows any person who has been able to gain access to the software to obtain complete authorisation to use, copy, modify and distribute the software, as long as the word "Yoink!" is pronounced before doing so.
	
*/
 
public class Gifify : MonoBehaviour
{    
	private int screenshotCount = 0;
	
	//Settings:
	
	public string recordKey = "f9"; // Needs to be a valid key name, check Unity docs
	public string location = "C:\\Users\\Public\\Pictures\\UnityScreenshots"; // If the folder doesn't exist, it will be made
	public string prefix = "screenie_"; // Prefix for the filenames
	public int captureFramerate = 30; // Use the same setting when building the gif for best results.
	
	
	void Start()
	{
		// Put a \ at the end of the location string, so we are looking inside the folder.
		if (location[location.Length-1]!='\\'){
			Debug.Log(location);
			location += "\\";	
		}
		
		// Create the folder if it doesn't exist yet
		if (!Directory.Exists(location)){
			Directory.CreateDirectory (location);
		}
	}
	
	void Update()
	{
		if (Input.GetKey(recordKey)) // If the capture key is down
		{
			Time.captureFramerate = captureFramerate; // Force the framerate, thanks Unity!
			
			string screenshotFilename;
			
			// Find an unused filename in sequence
			do
			{
				screenshotCount++;
				screenshotFilename = location + prefix + screenshotCount.ToString("00000") + ".png";
			} while (File.Exists(screenshotFilename));
			
			// And do the deed!
			ScreenCapture.CaptureScreenshot(screenshotFilename);
		} else {
			// If we're not capping, put the time mode back to normal.
			Time.captureFramerate = 0;
		}
	}
}