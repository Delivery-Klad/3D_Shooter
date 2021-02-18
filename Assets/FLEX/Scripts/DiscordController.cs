using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;

public class DiscordController : MonoBehaviour
{
	public Discord.Discord discord;
	private long startTime;

	void Start()
	{
		startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
		discord = new Discord.Discord(811922428766715904, (System.UInt64)Discord.CreateFlags.Default);
		var activityManager = discord.GetActivityManager();
		var activity = new Discord.Activity
		{
			State = "In lobby",
			Details = "Testing",
			Timestamps =
			{
				Start = startTime
			},
			Assets =
			{
				LargeImage = "1"
			},
			Party =
			{
				Size =
				{
					MaxSize = 20,
					CurrentSize = 1
				}
			},
		};
		activityManager.UpdateActivity(activity, (res) =>
		{
			if (res == Discord.Result.Ok)
			{
				Debug.Log("Everything is fine!");
			}
		});
	}

	void Update()
	{
		discord.RunCallbacks();
	}
}
