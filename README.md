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


=====

Task 1
------
I noticed that each token was present and interactable but just not visible during a test play. So my first suspects for why the tokens 
weren't showing up was either missing sprite references or the alpha channel on the sprite renderer is set to 0 on the Token object.
Upon inspection, each token had missing sprites under the idle animations in the Token Instance Script. When I chekced the files for the TokenSpin.png
Unity was not displaying the individual images of the sprite sheet, typically meaning the import settings was not set up properly.
Solution: 
Made the following changes to TokenSpin.png Import Settings
1.Texture Type:  Default -> Sprite(2D and UI)
2. Sprite Mode: Single -> Multiple