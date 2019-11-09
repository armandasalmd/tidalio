using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Tidalio
{
    class SavedLocationsAdapter : RecyclerView.Adapter
    {
        private readonly JavaList<String> displayData;

        public SavedLocationsAdapter(JavaList<String> data)
        {
            displayData = data;
        }

        public override int ItemCount
        {
            get { return displayData.Size(); }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyViewholder h = holder as MyViewholder;
            h.rowText.Text = displayData[position];

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.savedLocationRow, parent, false);
            MyViewholder holder = new MyViewholder(v);
            return holder;
        }


        internal class MyViewholder : RecyclerView.ViewHolder
        {
            public TextView rowText;

            public MyViewholder(View itemView) : base(itemView)
            {
                rowText = itemView.FindViewById<TextView>(Resource.Id.rowText);
            }
        }
    }
}