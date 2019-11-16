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
    [Activity(Label = "Sign Up", Theme ="@style/AppTheme")]
    public class SignUp : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {
        /// <summary>
        /// UI components
        /// </summary>
        Button btnSignup;
        TextView btnLogin, btnForgotPass;
        EditText input_email, input_password;
        RelativeLayout activity_sign_up;
        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);

            // initializing firebase auth
            auth = AuthHelper.GetInstance(this).GetAuth();
            
            // binding the views
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            btnForgotPass = FindViewById<TextView>(Resource.Id.signup_btn_forgot_password);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_password = FindViewById<EditText>(Resource.Id.signup_password);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            // setting on click listeners
            btnLogin.SetOnClickListener(this);
            btnForgotPass.SetOnClickListener(this);
            btnSignup.SetOnClickListener(this);
        }
        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.signup_btn_login)
            {
                // redirects to main login activity
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
            else if (v.Id == Resource.Id.signup_btn_forgot_password)
            {
                // start forget password activity
                StartActivity(new Intent(this, typeof(ForgotPassword)));
                Finish();
            }
            else if (v.Id == Resource.Id.signup_btn_register)
            {
                // on sign up click. Sign up the user
                SignUpUser(input_email.Text, input_password.Text);
            }
        }

        /// <summary>
        /// Notifies firebase about new account
        /// </summary>
        /// <param name="email">Email to sign up</param>
        /// <param name="password">Password for new account</param>
        private void SignUpUser(string email, string password)
        {
            auth.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this, this);
        }

        /// <summary>
        /// Firebase callback identifying status of sign up
        /// </summary>
        /// <param name="task">Firebase result containing status</param>
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
                DoSnackbar("Register successfully");
            else
                DoSnackbar("Register Failed");
        }

        /// <summary>
        /// Shows snackbar alert
        /// </summary>
        /// <param name="message">Message to display in snackbar</param>
        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_sign_up, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}