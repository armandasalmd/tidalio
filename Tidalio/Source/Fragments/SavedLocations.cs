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
    public class SavedLocations : Fragment
    {
        private RecyclerView recyclerView;
        private SavedLocationsAdapter adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            adapter = new SavedLocationsAdapter(getDataList());
            recyclerView.HasFixedSize = true;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
        }

        public static JavaList<String> getDataList()
        {
            return new JavaList<string>
            {
                "Sombrero Mata",
                "Shaka laka",
                "Upi trama",
                "Tolimi ketaa",
                "Indigo vaikas",
                "Kilop mita"
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.fragmentSavedLocations, container, false);
            InitRecycler(view);
            return view;
        }
    }
}