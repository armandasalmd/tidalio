using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Tidalio
{
    public class SavedForecasts : Fragment
    {

        private RecyclerView recyclerView;
        private SavedForecastsAdapter adapter;
        private JavaList<ForecastCard> mList;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mList = new JavaList<ForecastCard>();
            GeneratedBulkCards(3);
            // Create your fragment here
        }

        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView2);

            adapter = new SavedForecastsAdapter(mList);
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }


        private void GeneratedBulkCards(int count)
        {
            //ForecastCard card = Constants.GetSampleForecastCard();
            for (int i = 0; i < count; i++)
                mList.Add(Constants.GetSampleForecastCard());
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