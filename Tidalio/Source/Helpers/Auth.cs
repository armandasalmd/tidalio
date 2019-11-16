using Android.Content;
using Firebase;
using Firebase.Auth;

namespace Tidalio
{
    /// <summary>
    /// Singleton design class that handles interaction with firebase auth server
    /// </summary>
    public class AuthHelper
    {
        private Context context;
        /// <summary>
        /// Firebase variables
        /// </summary>
        private FirebaseApp app;
        private FirebaseAuth auth;
        private static AuthHelper instance;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        private AuthHelper(Context context)
        {
            this.context = context;
            if (app == null)
                app = FirebaseApp.InitializeApp(context, GetOptions());
            auth = FirebaseAuth.GetInstance(app);
        }

        /// <summary>
        /// Generates firebase configuration object
        /// </summary>
        /// <returns>Firebase configuration object</returns>
        private FirebaseOptions GetOptions()
        {
            var options = new FirebaseOptions.Builder()
                .SetApplicationId(Constants.AuthApplicationId)
                .SetApiKey(Constants.AuthApiKey)
                .SetDatabaseUrl(Constants.AuthDatabaseUrl)
                .SetProjectId(Constants.AuthProjectId)
                .SetStorageBucket(Constants.AuthStorageBucket)
                .Build();
            return options;
        }

        /// <summary>
        /// Get singleton instance of this class
        /// </summary>
        /// <param name="context">Activity context</param>
        /// <returns></returns>
        public static AuthHelper GetInstance(Context context)
        {
            if (instance == null)
                instance = new AuthHelper(context);
            return instance;
        }

        /// <summary>
        /// Extracts main firebase auth object
        /// </summary>
        /// <returns>Main auth object</returns>
        public FirebaseAuth GetAuth()
        {
            return auth != null ? auth : FirebaseAuth.GetInstance(app);
        }

        /// <summary>
        /// Returns logged user email
        /// </summary>
        public string CurrentUserEmail { get { return auth.CurrentUser.Email; } }
    }
}