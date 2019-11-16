using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

namespace Tidalio
{
    public class SavedForecasts : Fragment
    {

        private RecyclerView recyclerView;
        private SavedForecastsAdapter adapter;
        private List<ForecastCard> mList;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mList = GetDataList();
            // Create your fragment here
        }

        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView2);

            adapter = new SavedForecastsAdapter(Activity, mList);
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }

        public List<ForecastCard> GetDataList()
        {
            string user_email = AuthHelper.GetInstance(Activity).CurrentUserEmail;
            List<ForecastCard> mResponse = TidalioApi.GetInstance().FetchForecasts(user_email);
            return mResponse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.fragmentSavedForecasts, container, false);
            InitRecycler(view);

            return view;
        }
    }
}