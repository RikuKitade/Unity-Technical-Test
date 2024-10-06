
namespace Platformer.Data
{
    /// <summary>
    /// Data structure to represent the login request JSON.
    /// </summary>
    [System.Serializable]
    public class LoginRequestData
    {
        public string name;
        public string password;
    }

    /// <summary>
    /// Data structure to represent the login response JSON.
    /// </summary>
    [System.Serializable]
    public class LoginResponseData
    {
        public bool success;
        public string message;
        public Data data;

        [System.Serializable]
        public class Data
        {
            public string accessToken;
        }
    }

    /// <summary>
    /// Data structure to represent the user details response JSON.
    /// </summary>
    [System.Serializable]
    public class UserDetailsResponse
    {
        public Data data;

        [System.Serializable]
        public class Data
        {
            public User user;
        }

        [System.Serializable]
        public class User
        {
            public string email;

        }

    }
}
