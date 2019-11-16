using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Tidalio
{
    /// <summary>
    /// An adapter for RecyclerView component 
    /// used in Saved lcoations fragment to display user saved location
    /// </summary>
    class SavedLocationsAdapter : RecyclerView.Adapter
    {
        /// <summary>
        /// List of data models to be displayed
        /// </summary>
        private readonly List<Location> displayData;
        /// <summary>
        /// Main activity object
        /// </summary>
        private Activity activity;
        public List<Location> DisplayData => displayData;

        /// <summary>
        /// Initialize adapter providing saved locations model data
        /// </summary>
        /// <param name="activ">Main activity</param>
        /// <param name="data">Data model to display</param>
        public SavedLocationsAdapter(Activity activ, List<Location> data)
        {
            displayData = data;
            activity = activ;
        }

        public override int ItemCount => displayData.Count();

        /// <summary>
        /// Add and show new row for recycler view
        /// </summary>
        /// <param name="data">Data row to add</param>
        public void AddRow(Location data)
        {
            displayData.Add(data);
            NotifyItemInserted(ItemCount - 1);
        }

        /// <summary>
        /// Initialize recyclerView row
        /// </summary>
        /// <param name="holder">Row view</param>
        /// <param name="position">Number showing row position</param>
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