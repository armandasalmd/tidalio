using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace Tidalio
{
    public class SavedLocations : Fragment
    {
        private RecyclerView recyclerView;
        private SavedLocationsAdapter adapter;
        private JavaList<Location> mList;
        private AutoCompleteTextView autoComplete;
        private Button addLocationBtn;

        // private FirebaseFirestore database;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mList = GetDataList();
            // database = GetDatabase();
            // Create your fragment here
        }

        //private FirebaseFirestore GetDatabase()
        //{
        //    FirebaseFirestore database;
        //    var app = AuthHelper.GetInstance(Activity).GetApp();
        //    database = FirebaseFirestore.GetInstance(app);
        //    return database;
        //}

        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);


            // FetchData();
            adapter = new SavedLocationsAdapter(mList);
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }

        public static JavaList<Location> GetDataList()
        {
            return new JavaList<Location>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.fragmentSavedLocations, container, false);
            InitRecycler(view);
            autoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
            addLocationBtn = view.FindViewById<Button>(Resource.Id.addNewLocationBtn);
            addLocationBtn.Click += AddLocationBtn_Click;

            return view;
        }

        private void AddLocationBtn_Click(object sender, EventArgs e)
        {
            string dataToAdd = autoComplete.Text;
            if (dataToAdd != string.Empty && dataToAdd != null)
            {
                adapter.AddRow(dataToAdd);
                autoComplete.Text = string.Empty;

            }
        }

        //private void FetchData()
        //{
        //    database.Collection("locations").Get().AddOnSuccessListener(this);
        //}

        /*public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                mList.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    string address = item.Get("Address") != null ? item.Get("Address").ToString() : "undefined";
                    double lat = (double)item.Get("Latitude");
                    double lon = (double)item.Get("Longitude");
                    Location loc = new Location(address, lat, lon);
                    mList.Add(loc);
                }
            }
        }*/
    }
}

// load, add, delete