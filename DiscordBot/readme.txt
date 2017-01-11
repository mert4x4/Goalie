GOALIE! The Discord bot for all your Rocket League celebratory music needs!'

Version 1.3

This bot will play a randomly selected .mp3 file in a discord channel whenever your team scores a goal in Rocket League.
When running the application the bot won't join a channel until the user specified in the config.txt file
has started up Rocket League (user must have it set so discord shows current game), at which point the bot 
will follow that user into any voice channel they move (as long as the bot as adequate permissions)

I've found using audio clips that are 14 seconds long with fade in's and fade out's works best (audacity is good for this)
A sample has been included in the music folder.

Installation
------------------------------
1.	Go to https://discordapp.com/developers/applications/me and login to your discord account

2.	Select "New Application"

3.	Enter a name for the bot (I personally suggest Goalie) and add a picture if you like (not required)

4.	Click "Create Application"

5.	On the next page click "Create a Bot User"

6.	Click on "click to reveal" next to "token" 

7.	Copy and paste your token into the config.txt located within the Goalie directory after "Token="
	(Warning: Do not share this token, if you do anybody who has it can control your bot)

8.	Copy the Client ID of the bot (In the App details section) and use it to replace "CLIENTIDHERE" in the link below
	https://discordapp.com/api/oauth2/authorize?client_id=CLIENTIDHERE&scope=bot&permissions=0

9.	Select the server you wish to add the bot to and hit Authorize

10.	In the config.txt inside of the Goalie directory, put the username of the user that the bot should follow after "User="

11. Put .mp3 files to be played in the "music" folder

12. Run Goalie.exe and score some goals!

Known Bugs
------------------------------
None

Last Tested on Discord Build Dec 14th, 2016 and Rocket League Build 1508090

If you find any more bugs please message me on reddit (/u/garrettjones331) or add me on steam (http://steamcommunity.com/id/garrettjones/)

Change Log
------------------------------
V 1.3
Updated to handle Rocket League Update

V 1.2
Added Exception handler's for many common errors

V 1.1
Fixed issue where bot would stop working after one game
Fixed issue where bot wouldn't detect for many users (Thanks dem0n5!)

