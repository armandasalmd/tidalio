﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;

namespace Tidalio
{
    public class AuthHelper
    {
        private Context context;
        private FirebaseApp app;
        private FirebaseAuth auth;
        private static AuthHelper instance;
        private AuthHelper(Context context)
        {
            this.context = context;
            if (app == null)
                app = FirebaseApp.InitializeApp(context, GetOptions());
            auth = FirebaseAuth.GetInstance(app);
        }
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
        public static AuthHelper GetInstance(Context context)
        {
            if (instance == null)
                instance = new AuthHelper(context);
            return instance;
        }
        public FirebaseAuth GetAuth()
        {
            if (auth != null)
                return auth;
            else
                return FirebaseAuth.GetInstance(app);
        }

        public FirebaseApp GetApp()
        {
            return app;
        }
    }
}