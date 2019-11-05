using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Xamarin.Essentials;
using Tidalio.Source.Helpers;
using Android.Widget;
using Android.Content;
using Firebase;
using Firebase.Auth;
using static Android.Views.View;
using Android.Gms.Tasks;

// SHA1: AB:60:A1:C5:27:25:48:61:7F:2A:93:0F:A4:B0:20:DA:75:2E:EF:AA
namespace Tidalio
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnClickListener, IOnCompleteListener
    {

        Button btnLogin;
        EditText input_email, input_password;
        TextView btnSignUp, btnForgotPassword;

        RelativeLayout activity_main;
        public static FirebaseApp app;
        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            //Init Firebase
            InitFirebaseAuth();

            //View
            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_password = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgotPassword = FindViewById<TextView>(Resource.Id.login_btn_forgot_password);
            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);

            btnSignUp.SetOnClickListener(this);
            btnLogin.SetOnClickListener(this);
            btnForgotPassword.SetOnClickListener(this);
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
            .SetApplicationId("1:985581754028:android:95b71532953e2cf8fff109")
            .SetApiKey("AIzaSyAlASiFw6wz0lvAjhwMzsfH_rWAu5ycIAM")
            .SetDatabaseUrl("https://tidalioauth-8377f.firebaseio.com")
            .SetProjectId("tidalioauth-8377f")
            .SetStorageBucket("tidalioauth-8377f.appspot.com")
            .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);
            auth = FirebaseAuth.GetInstance(app);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            string display = "string"; //await Functions.GetCoords();
            

            Snackbar.Make(view, display, Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.login_btn_forgot_password)
            {
                StartActivity(new Android.Content.Intent(this, typeof(ForgotPassword)));
                Finish();
            }
            else if (v.Id == Resource.Id.login_btn_sign_up)
            {
                StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
                Finish();
            }
            else if (v.Id == Resource.Id.login_btn_login)
            {
                LoginUser(input_email.Text, input_password.Text);
            }
        }
        private void LoginUser(string email, string password)
        {
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                Finish();
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(activity_main, "Login Failed ", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}

