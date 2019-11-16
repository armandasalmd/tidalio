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
        private readonly List<Location> displayData;
        private Activity activity;
        public List<Location> DisplayData
        {
            get { return displayData; }
        }

        public SavedLocationsAdapter(Activity context, List<Location> data)
        {
            displayData = data;
            activity = context;
        }

        public override int ItemCount
        {
            get { return displayData.Count(); }
        }

        public void AddRow(Location data)
        {
            displayData.Add(data);
            NotifyItemInserted(ItemCount - 1);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyViewholder h = holder as MyViewholder;
            h.rowText.Text = displayData[position].Address;
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.savedLocationRow, parent, false);
            MyViewholder holder = new MyViewholder(v);
            holder.deleteButton.Click += (object sender, EventArgs e) => {
                TidalioApi.GetInstance().DeleteLocationAsync(AuthHelper.GetInstance(activity).CurrentUserEmail, displayData[holder.LayoutPosition]);
                displayData.RemoveAt(holder.LayoutPosition);
                NotifyItemRemoved(holder.LayoutPosition);
            };
            holder.root.Click += (object sender, EventArgs e) => {
                if (activity is Dashboard)
                {
                    Dashboard d = (Dashboard)activity;
                    d.SearchLocation(displayData[holder.LayoutPosition]);
                }
            };

            return holder;
        }

        internal class MyViewholder : RecyclerView.ViewHolder
        {
            public TextView rowText;
            public ImageView deleteButton;
            public View root;
            public MyViewholder(View itemView) : base(itemView)
            {
                root = itemView.FindViewById<View>(Resource.Id.row_root);
                rowText = itemView.FindViewById<TextView>(Resource.Id.rowText);
                deleteButton = itemView.FindViewById<ImageView>(Resource.Id.delete_row_btn);
                
            }

        }
    }
}