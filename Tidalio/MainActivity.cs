using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static Android.Views.View;
using Android.Gms.Tasks;

// The main login screen activity
namespace Tidalio
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {

        Button btnLogin;
        EditText input_email, input_password;
        TextView btnSignUp, btnForgotPassword;
        RelativeLayout activity_main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);

            //Init Firebase
            AuthHelper.GetInstance(this);

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

            //btnLogin.CallOnClick(); // TODO: delete when in production
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
            //email = "test@test.com"; // TODO: remove in production
            //password = "test123"; // TODO: remove in production
            if (email == null || email == "")
                DoSnackbar("Please enter the email");
            else if (password == null || password == "")
                DoSnackbar("Please enter the password");
            else if (!email.Contains("@"))
                DoSnackbar("Enter valid email address");
            else
                AuthHelper.GetInstance(this).GetAuth().SignInWithEmailAndPassword(email, password).AddOnCompleteListener(this);
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                Finish();
            }
            else
                DoSnackbar("Login Failed");
        }

        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(activity_main, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}

