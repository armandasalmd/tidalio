using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

namespace Tidalio
{
    public class SavedForecasts : Fragment
    {
        /// <summary>
        /// UI components
        /// </summary>
        private RecyclerView recyclerView;
        private SavedForecastsAdapter adapter;
        /// <summary>
        /// List of data models to display in recyclerView
        /// </summary>
        private List<ForecastCard> mList;

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
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView2);

            adapter = new SavedForecastsAdapter(Activity, mList);
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }

        /// <summary>
        /// Calls web api to get the data from the cloud
        /// </summary>
        /// <returns>Data that was received from the web API/cloud</returns>
        public List<ForecastCard> GetDataList()
        {
            // get logged user emai
            string user_email = AuthHelper.GetInstance(Activity).CurrentUserEmail;
            // get list of models to display in recycler
            List<ForecastCard> mResponse = TidalioApi.GetInstance().FetchForecasts(user_email);
            return mResponse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragmentSavedForecasts, container, false);
            InitRecycler(view);
            return view;
        }
    }
}