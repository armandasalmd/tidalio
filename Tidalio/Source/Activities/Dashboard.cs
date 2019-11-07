using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using System;
using static Android.Views.View;

namespace Tidalio
{
    [Activity(Label = "Dashboard", Theme = "@style/AppTheme.NoActionBar")]
    public class Dashboard : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnClickListener, IOnCompleteListener
    {
        TextView txtWelcome;
        EditText input_new_password;
        Button btnChangePass, btnLogout;
        RelativeLayout activity_dashboard;

        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dashboard_main);
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
            auth = AuthHelper.GetInstance(this).GetAuth();

            //View
            txtWelcome = FindViewById<TextView>(Resource.Id.dashboard_welcome);
            input_new_password = FindViewById<EditText>(Resource.Id.dashboard_newpassword);
            btnChangePass = FindViewById<Button>(Resource.Id.dashboard_btn_change_pass);
            btnLogout = FindViewById<Button>(Resource.Id.dashboard_btn_logout);
            activity_dashboard = FindViewById<RelativeLayout>(Resource.Id.activity_dashboard);

            btnChangePass.SetOnClickListener(this);
            btnLogout.SetOnClickListener(this);

            //Check session
            if (auth.CurrentUser != null)
                txtWelcome.Text = "Welcome , " + auth.CurrentUser.Email;
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
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
                LogoutUser();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            string display = "string"; //await Functions.GetCoords();

            DoSnackbar(display);
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
            if (v.Id == Resource.Id.dashboard_btn_change_pass)
                ChangePassword(input_new_password.Text);
            else if (v.Id == Resource.Id.dashboard_btn_logout)
                LogoutUser();
        }

        private void LogoutUser()
        {
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }

        private void ChangePassword(string newPassword)
        {
            FirebaseUser user = auth.CurrentUser;
            user.UpdatePassword(newPassword)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == true)
            {
                DoSnackbar("Password has been changed");
            }
        }

        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_dashboard, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}