using Android.App;
using Android.Content;
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

namespace Tidalio
{
    [Activity(Label = "Dashboard", Theme = "@style/AppTheme.NoActionBar")]
    public class Dashboard : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        LinearLayout activity_dashboard;
        FloatingActionButton fab;
        Fragment active_fragment;

        int active_fragment_id = Resource.Id.nav_forecast;


        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dashboard_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            View headerView = navigationView.GetHeaderView(0);
            
            //TextView navUsername = (TextView)headerView.findViewById<>();
            auth = AuthHelper.GetInstance(this).GetAuth();
            headerView.FindViewById<TextView>(Resource.Id.nav_user_email).SetText(auth.CurrentUser.DisplayName, TextView.BufferType.Normal);
            //navUsername.setText("Your Text Here");

            //Init Firebase

            //View
            activity_dashboard = FindViewById<LinearLayout>(Resource.Id.activity_dashboard);

            active_fragment = new Forecast();
            ShowFragment(active_fragment);
        }

        public void SearchLocation(Location loc)
        {
            active_fragment_id = Resource.Id.nav_forecast;
            fab.Visibility = ViewStates.Visible;
            active_fragment = new Forecast(loc);
            ShowFragment(active_fragment);
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

        [Obsolete]
        private void FabOnClick(object sender, EventArgs eventArgs)
        { 
            //string display = "string"; //await Functions.GetCoords();
            //DoSnackbar(display);
            if (active_fragment_id == Resource.Id.nav_forecast)
            {
                (active_fragment as Forecast).OnSearch();
            }
        }

        [Obsolete]
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_forecast)
            {
                active_fragment = new Forecast();
                active_fragment_id = Resource.Id.nav_forecast;
                fab.Visibility = ViewStates.Visible;
            }
            else if (id == Resource.Id.nav_saved_locations)
            {
                active_fragment = new SavedLocations();
                active_fragment_id = Resource.Id.nav_saved_locations;
                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_saved_forecasts)
            {
                active_fragment = new SavedForecasts();
                active_fragment_id = Resource.Id.nav_saved_forecasts;
                fab.Visibility = ViewStates.Invisible;
            }
            else if (id == Resource.Id.nav_settings)
            {
                StartActivity(new Android.Content.Intent(this, typeof(Settings)));
            }
            else if (id == Resource.Id.nav_logout)
            {
                LogoutUser();
            }
            
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);

            if (active_fragment != null)
                ShowFragment(active_fragment);
            return true;
        }

        [Obsolete]
        private void ShowFragment(Fragment fragment)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment_container, fragment);
            ft.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public void LogoutUser()
        {
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }


        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_dashboard, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}