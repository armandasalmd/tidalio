using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Tidalio
{
    public class SavedLocations : Fragment
    {
        /// <summary>
        /// UI components
        /// </summary>
        private RecyclerView recyclerView;
        private SavedLocationsAdapter adapter;
        private List<Location> mList;
        private AutoCompleteTextView autoComplete;
        private Button addLocationBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Fetch data from web API
            mList = GetDataList();
        }

        /// <summary>
        /// Initialize recyclerView
        /// </summary>
        /// <param name="view">root container</param>
        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            adapter = new SavedLocationsAdapter(Activity, mList);
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }

        /// <summary>
        /// Calls web api to get the data from the cloud
        /// </summary>
        /// <returns>Data that was received from the web API/cloud</returns>
        public List<Location> GetDataList()
        {
            List<Location> mResponse = TidalioApi.GetInstance()
                .FetchLocations(AuthHelper.GetInstance(Activity).CurrentUserEmail);
            return mResponse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragmentSavedLocations, container, false);
            InitRecycler(view);
            // binding layout views
            autoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
            addLocationBtn = view.FindViewById<Button>(Resource.Id.addNewLocationBtn);
            // adding listener that adds new location once clicked
            addLocationBtn.Click += AddLocationBtn_Click;
            return view;
        }

        /// <summary>
        /// Button related to adding/saving the new location
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">Event arguments</param>
        private void AddLocationBtn_Click(object sender, EventArgs e)
        {
            string dataToAdd = autoComplete.Text;
            if (dataToAdd != string.Empty && dataToAdd != null)
            {
                Location loc = new Location(dataToAdd);
                adapter.AddRow(loc);
                TidalioApi.GetInstance()
                    .PostLocationAsync(AuthHelper.GetInstance(Activity).CurrentUserEmail, loc);
                autoComplete.Text = string.Empty;

            }
        }
    }
}