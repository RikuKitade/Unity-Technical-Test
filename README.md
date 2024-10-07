Name: Tai Cao
================

Approach Explanation

Task 1
------
I noticed that each token was present and interactable but just not visible during a test play. So my first suspects for why the tokens 
weren't showing up was either missing sprite references or the alpha channel on the sprite renderer is set to 0 on the Token object.
Upon inspection, each token had missing sprites under the idle animations in the Token Instance Script. When I checked the files for the TokenSpin.png,
Unity was not displaying the individual images of the sprite sheet, typically meaning the import settings was not configured properly.
I made the following changes to TokenSpin.png Import Settings to fix the sprite sheet:
1. Texture Type:  Default -> Sprite(2D and UI)
2. Sprite Mode: Single -> Multiple

Task 2
------
The first step here was finding where the current jump logic existed, which was found in PlayerController. To keep track of how many jumps
the player has performed in the air, I decided with using an integer rather than a boolean (i.e. hasDoubleJumped) to leave room for 
additional jumps. Because the jump logic for a single jump already existed, I knew I needed to update 2 areas:
1. Allow players to jump while in the air as long as the # of jumps is >= 2.
2. Have the landing state reset the number of jumps.
When updating the jump logic, I wanted to take a non-invasive approach and avoid restructuring if it's not necessary. In this case, 
updating a few conditions in ComputerVelocity(), Update(), and UpdateJumpState() did the job!

Task 3
------
UI Setup Step
When creating the UI for the token counter, I first made a canvas with screen space overlay as we want the UI to render on top of the screen
and to adjust its size to match the screen resolution. I also made sure the token counter text and counter image was anchored properly to
scale to different aspect ratios. For the email UI label to be used in Task 4 I made it in the same canvas since it's part of the same UI layer 
as the token counter.

Adding Score Logic
To manage the player's score and the score update animation, I made a TokenScoreController under the Mechanics namespace to encapsulate that logic.
Because the Execute() method in the PlayerTokenCollision class is triggered everytime a the player collects a coin, I decided to have TokenScoreController 
increment the token score there to keep token collision events in one place. To visualize a score update, I decided on having the score number perform a 
pop animation by lerping the score text scale. Because the coins can be collected rapidly, the score update animation resets everytime a coin is 
collected to prevent overlapping animations.

Task 4
------
Login UI Setup
For the login screen UI, I created another screen space canvas on top of the gameplay screen and had the interactable UI elements 
(name and password inputs / login button) scale to the screen resolution. 

API Integration
Below are the approaches I took for each step of the API integration for the login menu:
1. User Authentication Step
	- To handle the login endpoint, I created a coroutine which utilizes Unity's WebRequest to make the POST request once the login button is pressed. Because the API
	  accepts JSON data for the request body and returns responses in JSON format, I used JSONUtility to convert the data sent (username/password info)
	  and data received (JWT token). Because JSONUtility converts serializable classes into a JSON string and vice versa, I created all the necessary data classes 
	  for the endpoints under the Platformer.Data namespace.
	
2. Input/Error Handling
	- If the login request fails, the user will be presented with an error message. Below are the scenarios I handled for error/input handling:
		1. Displays appropriate error messaging when...
		   - User does not have internet connection.
		   - User leaves any fields blank. Web request won't be sent unless both name/password fields are filled out.
		   - User provides incorrect login info.
		   - Request takes too long for a response / API server is down. 
		2. To prevent unnecessary requests or potential button spamming, I created a 2 second cooldown for pressing the login button.
		3. Removes leading/trailing whitespace for user inputs

3. Handle API Response and Fetch Authenticated User Details
   - After succcessfully authenticating the user, I stored the fetched JWT token through PlayerPrefs. To retrieve the user's email, I created a GET WebRequest coroutine
	 that uses the JWT token as the Authorization header. Once we get a successful response from the request, JSONUtility is used here to grab the email from that response. 
	 In the case the GET request fails, an error message will also be presented to the user.
	
4. Update the Main UI and Complete Login
   - The email retrieved from the GET request above is then placed onto the UI Text component made in Task 3. To handle context 
	 switching from the login menu to gameplay, I used the existing MetaGameController class to close the login menu and start the game 
	 because it already handles switching control between different UI's. Lastly, I decided to have the player spawn in after the login
	 menu is closed to signify that the game has started once the login process is complete. 



================

# Unity Technical Test

## Overview

You will be using the [Unity Micro Platformer template repository](https://github.com/SkillionaireGames/Unity-Technical-Test.git) as the base for this take-home test. Please fork the repository and complete the tasks listed below. Submit your forked version with a link to the completed project.

The goal is to evaluate your ability to work with an existing Unity project, add new features, debug problems, and integrate with external APIs. Keep your implementation clean, well-structured, and documented where necessary.

## Tasks

### 1. Fix Existing Issue: Tokens Not Visible
The tokens in the scene are currently not visible even though they were added. Investigate why they are not showing up and make sure they are visible and animating correctly.

### 2. New Feature: Implement Double Jump
- Modify the player character to be able to perform a double jump.
- The character should be able to jump once more in the air after the initial jump.
- The jump count should reset when the character touches the ground.

### 3. Add UI: Token Counter & User Information
- Add a UI element to display the number of tokens the player has collected that animates when new Tokens are gained.
- Add another UI element to show the logged-in user's email address (retrieved from step 4).

### 4. User Authentication Integration
- Create a login screen at the beginning of the game that prompts for a username and password.
- Use the provided API to authenticate the user.

#### Endpoint Documentation
- [API Documentation](https://api-dev.skillionairegames.com/api/documentation#/)

#### Implementation Details
- Send the username and password to the login endpoint and present error messages or handle success.
  
**Login Information to use:**
  - Username: `Fakerson123`
  - Password: `fakepassword`
  
- If login is successful, locally store the JWT token returned by the API.
- Use the JWT token to call the `/auth/authenticated-user-details` endpoint to get the user's email.
- Display the logged-in user's email on the main UI after successful login.

## Submission Instructions

1. **Fork** the Unity Micro Platformer template repository.
2. Implement the changes according to the requirements above.
3. Provide a link to your forked repository that includes:
   - Source code
   - Any additional documentation if required
   - A brief summary of your approach to each task (in the project README file).

## Evaluation Criteria

1. **Correctness**: Does the implementation meet the requirements?
2. **Code Quality**: Is the code clean, maintainable, and well-structured?
3. **Problem-Solving**: Does the approach demonstrate solid problem-solving skills?
4. **Documentation**: Are your comments and code structure easy to follow?
5. **Completion**: Are all tasks fully completed as expected?

Good luck, and we look forward to reviewing your implementation!




