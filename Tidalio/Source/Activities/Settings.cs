using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using static Android.Views.View;

namespace Tidalio
{
    [Activity(Label = "Settings", Theme = "@style/AppTheme")]
    public class Settings : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {
        LinearLayout activity_root;
        EditText input_pwd;
        TextView username_view;
        Button btnChangePass, btnClear;
        FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            
            // initialize firebase auth instance
            auth = AuthHelper.GetInstance(this).GetAuth();

            // bind views to variables
            activity_root = FindViewById<LinearLayout>(Resource.Id.settings_root);
            input_pwd = FindViewById<EditText>(Resource.Id.settings_newpassword);
            btnChangePass = FindViewById<Button>(Resource.Id.settings_btn_change_pass);
            btnClear = FindViewById<Button>(Resource.Id.settings_clear);
            username_view = FindViewById<TextView>(Resource.Id.settings_username);

            username_view.SetText(auth.CurrentUser.Email, TextView.BufferType.Normal);

            // set on click listeners
            btnChangePass.SetOnClickListener(this);
            btnClear.SetOnClickListener(this);
        }

        /// <summary>
        /// Notifies firebase to change the password
        /// </summary>
        /// <param name="newPassword">New password</param>
        private void ChangePassword(string newPassword)
        {
            FirebaseUser user = AuthHelper.GetInstance(this).GetAuth().CurrentUser;
            user.UpdatePassword(newPassword)
                .AddOnCompleteListener(this);
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.settings_btn_change_pass)
            {
                if (input_pwd.Text == null || input_pwd.Text.Length < 5)
                    DoSnackbar("Choose differrent password");
                else
                    ChangePassword(input_pwd.Text);
            } else if (v.Id == Resource.Id.settings_clear)
            {
                DoSnackbar("Data clearing is not working!");
            }
        }

        /// <summary>
        /// Firebase callback identifying status of sign up
        /// </summary>
        /// <param name="task">Firebase result containing status</param>
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == true)
            {
                DoSnackbar("Password has been changed");
            } else
            {
                DoSnackbar("Error while changing the password");
            }
        }

        /// <summary>
        /// Shows snackbar alert
        /// </summary>
        /// <param name="message">Message to display in snackbar</param>
        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_root, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}