using Android.App;
using Android.Content;
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
    [Activity(Label = "Forgot Password", Theme ="@style/AppTheme")]
    public class ForgotPassword : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {
        /// <summary>
        /// UI Components
        /// </summary>
        private EditText input_email;
        private Button btnResetPass;
        private TextView btnBack;
        private RelativeLayout activity_forgot;

        FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgotPassword);

            // initialiazing firebase auth
            auth = AuthHelper.GetInstance(this).GetAuth();

            // binding the view to variables
            input_email = FindViewById<EditText>(Resource.Id.forgot_email);
            btnResetPass = FindViewById<Button>(Resource.Id.forgot_btn_reset);
            btnBack = FindViewById<TextView>(Resource.Id.forgot_btn_back);
            activity_forgot = FindViewById<RelativeLayout>(Resource.Id.activity_forgot);

            btnResetPass.SetOnClickListener(this);
            btnBack.SetOnClickListener(this);
        }


        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.forgot_btn_back)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
            else if (v.Id == Resource.Id.forgot_btn_reset)
            {
                if (input_email.Text == null || input_email.Text == "" || !input_email.Text.Contains("@"))
                    DoSnackbar("Please enter valid email address");
                else
                    ResetPassword(input_email.Text);
            }
        }

        /// <summary>
        /// Notifies firebase to send an email with reset password link
        /// </summary>
        /// <param name="email">User email to reset password</param>
        private void ResetPassword(string email)
        {
            auth.SendPasswordResetEmail(email)
                .AddOnCompleteListener(this, this);
        }

        /// <summary>
        /// Firebase callback identifying status of sign up
        /// </summary>
        /// <param name="task">Firebase result containing status</param>
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == false)
            {
                Snackbar snackBar = Snackbar.Make(activity_forgot, "Reset password failed", Snackbar.LengthShort);
                snackBar.Show();
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(activity_forgot, "Reset password link sent to email : " + input_email.Text, Snackbar.LengthShort);
                snackBar.Show();
            }
        }

        /// <summary>
        /// Shows snackbar alert
        /// </summary>
        /// <param name="message">Message to display in snackbar</param>
        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_forgot, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}