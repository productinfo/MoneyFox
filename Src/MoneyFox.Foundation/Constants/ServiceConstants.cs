﻿namespace MoneyFox.Foundation.Constants
{
    /// <summary>
    ///     Contains constants for the services used in the app.
    /// </summary>
    public class ServiceConstants
    {
        /// <summary>
        ///     Return url for the OneDrive authentication
        /// </summary>
        public const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        /// <summary>
        ///     Returns the base URL of the OneDrive Service
        /// </summary>
        public const string BASE_URL = "https://api.onedrive.com/v1.0";

        /// <summary>
        ///     Authentication url for the OneDrive authentication
        /// </summary>
        public const string AUTHENTICATION_URL = "https://login.live.com/oauth20_authorize.srf";

        /// <summary>
        ///     Logout url for the OneDrive authentication
        /// </summary>
        public const string LOGOUT_URL = "https://login.live.com/oauth20_logout.srf";

        /// <summary>
        ///     The Token URL is used to retrieve a access token in the code flow oauth
        /// </summary>
        public const string TOKEN_URL = "https://login.live.com/oauth20_token.srf";

        /// <summary>
        ///     String constant for the access token.
        /// </summary>
        public const string ACCESS_TOKEN = "access_token";
        
        /// <summary>
        ///     String constant for the refresh token.
        /// </summary>
        public const string REFRESH_TOKEN = "refresh_token";

        /// <summary>
        ///     String constant for the redirect uri.
        /// </summary>
        public const string REDIRECT_URI = "redirect_uri";

        /// <summary>
        ///     String constant for the client id.
        /// </summary>
        public const string CLIENT_ID = "client_id";

        /// <summary>
        ///     String constant for the scope.
        /// </summary>
        public const string SCOPE = "scope";

        /// <summary>
        ///     String constant for the response type.
        /// </summary>
        public const string RESPONSE_TYPE = "response_type";

        /// <summary>
        ///     String constant for the code.
        /// </summary>
        public const string CODE = "code";

        /// <summary>
        ///     Used to save an retrieve a OAUTH session for OneDrive in the key store.
        /// </summary>
        public const string KEY_STORE_TAG_ONEDRIVE = "OneDrive";

        /// <summary>
        ///     Scopes for OneDrive access
        /// </summary>
        public static string[] Scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin"};

        /// <summary>
        ///     Maximum number of attempts to sync the database
        /// </summary>
        public static int SyncAttempts = 2;

        /// <summary>
        ///     The amount of time to wait for the onedrive backup to be completed
        /// </summary>
        public static int BackupOperationTimeout = 10000;

        /// <summary>
        ///     The amount of time to wait before retrying to sync
        /// </summary>
        public static int BackupRepeatDelay = 2000;
    }
}