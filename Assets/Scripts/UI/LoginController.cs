using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Platformer.Data;
namespace Platformer.UI
{
    public class LoginController : MonoBehaviour
    {
        // The API URL's used to authenticate the user and retrieve user email.
        private const string loginUrl = "https://api-dev.skillionairegames.com/api/auth/login"; 
        private const string userDetailsUrl = "https://api-dev.skillionairegames.com/api/auth/authenticated-user-details";

        // The JWT token received after successful login.
        private string jwtToken;
        // Text UI element to display the logged-in user's email.
        public TextMeshProUGUI userEmailText;
        public Button loginButton;

        // Username and password input fields used for login
        public TMP_InputField userNameInput;
        public TMP_InputField passwordInput;

        // Reference to the error Text below the login and password input field
        // to be used in case the web requests fail or the fields are empty
        public TextMeshProUGUI errorText;

        // This reference is used to switch from the login screen into the gameplay screen
        public MetaGameController metaGameController;


        private void Start()
        {
            loginButton.onClick.AddListener(Login);
        }

        /// <summary>
        /// Initiates the login process with the username and password. Login request 
        /// will not start unless both fields are filled out with non-space text.
        /// </summary>
        public void Login()
        {
            if (isValid(userNameInput.text, passwordInput.text)){
                StartCoroutine(StartLoginCooldown());
                StartCoroutine(PostLoginRequest(userNameInput.text, passwordInput.text));
            }
        }

        /// <summary>
        /// Sends a POST request to the login endpoint with the name and password.
        /// </summary>
        private IEnumerator PostLoginRequest(string name, string password)
        {
            // Create the request data object after removing trailing/leading
            // white space from inputs and convert it to a JSON string.
            var loginData = new LoginRequestData { name = name.Trim(), password = password.Trim() };
            string jsonData = JsonUtility.ToJson(loginData);

            // Create a POST UnityWebRequest for the login endpoint.
            using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                // Set headers to support our API's JSON data format.
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");
                //Set a timeout in case the response takes too long
                request.timeout = 10;
                // Send the request and wait for a response.
                yield return request.SendWebRequest();

                // Handle the response.
                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Extract the JWT token from the response and store it locally.
                    jwtToken = ExtractTokenFromResponse(request.downloadHandler.text);
                    PlayerPrefs.SetString("JWT_Token", jwtToken);
                    Debug.Log("Login successful! JWT Token: " + jwtToken);

                    // Call the next API to get the authenticated user's details.
                    StartCoroutine(GetUserDetails(jwtToken));
                }
                else
                {
                    Debug.LogError("Login failed: " + request.error);
                    DisplayErrorMessage(request);
                }
            }
        }

        /// <summary>
        /// Sends a GET request to retrieve authenticated user details using the JWT token.
        /// </summary>
        /// <param name="token">The JWT token for authentication.</param>
        private IEnumerator GetUserDetails(string token)
        {
            // Create a UnityWebRequest for the user details endpoint.
            using (UnityWebRequest request = UnityWebRequest.Get(userDetailsUrl))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                // Set the authorization header using the JWT token
                request.SetRequestHeader("Authorization", "Bearer " + token);
                request.timeout = 10;
                // Send the request and wait for a response.
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Parse the email from the server response and updates the main UI.
                    string userEmail = ExtractEmailFromResponse(request.downloadHandler.text);
                    DisplayUserEmail(userEmail);
                    // Closes the login menu to proceed to game start.
                    metaGameController.ToggleLoginMenu(false);
                    loginButton.enabled = false;
                    Debug.Log("Login successful! User email: " + userEmail);
                }
                else
                {
                    Debug.LogError("Failed to retrieve user details: " + request.error);
                    DisplayErrorMessage(request);
                }
            }
        }

        /// <summary>
        /// Makes the login button uninteractable for a period of time after pressing
        /// to prevent spamming and unnecessary calls if the user double clicks.
        /// </summary>
        private IEnumerator StartLoginCooldown()
        {
            loginButton.interactable = false;
            yield return new WaitForSecondsRealtime(2.0f);
            loginButton.interactable = true;
        }

        /// <summary>
        /// Extracts the JWT token from the login response.
        /// </summary>
        /// <returns>The JWT token as a string.</returns>
        private string ExtractTokenFromResponse(string response)
        {
            var responseData = JsonUtility.FromJson<LoginResponseData>(response);
            return responseData.data.accessToken;
        }

        /// <summary>
        /// Extracts the email from the user details response.
        /// </summary>
        private string ExtractEmailFromResponse(string response)
        {
            var userDetails = JsonUtility.FromJson<UserDetailsResponse>(response);
            return userDetails.data.user.email;
        }

        /// <summary>
        /// Displays an error message on the login UI if the login request fails.
        /// </summary>
        /// <param name="request">The request from an API call after failure.</param>
        private void DisplayErrorMessage(UnityWebRequest request)
        {
            errorText.gameObject.SetActive(true);
            // If there is a connection error or if the request takes too long, 
            // dispay the error text appropriately. 
            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                errorText.text = "Network Error. Please check your connection.";
            }

            else if(request.error == "Request timeout")
            {
                errorText.text = "Login request timed out. Please try again.";
            }
            else
            {
                errorText.text = "Incorrect credentials. Please try again.";
            }
        }

        /// <summary>
        /// Checks to see if both username and password is filled out.
        /// Displays error message if either input field is empty.
        /// </summary>
        private bool isValid(string name, string password)
        {
            if(name.Trim() == "" || password.Trim() == "")
            {
                errorText.text = "Please fill out both fields.";
                errorText.gameObject.SetActive(true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Displays the user's email on the main game UI.
        /// </summary>
        private void DisplayUserEmail(string email)
        {
            userEmailText.text = "user: " + email;
        }
    }
}
