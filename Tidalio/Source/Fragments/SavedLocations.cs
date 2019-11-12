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
using Newtonsoft.Json;

namespace Tidalio
{
    public class SavedLocations : Fragment
    {
        private RecyclerView recyclerView;
        private SavedLocationsAdapter adapter;
        private JavaList<string> mList;
        private AutoCompleteTextView autoComplete;
        private Button addLocationBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mList = getDataList();
           // Create your fragment here
        }

        private void InitRecycler(View view)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            adapter = new SavedLocationsAdapter(mList);
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
            //adapter.AddRow()
        }
    }
}