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
    class SavedForecastsAdapter : RecyclerView.Adapter
    {


        private readonly JavaList<ForecastCard> displayData;
        public JavaList<ForecastCard> DisplayData
        {
            get { return displayData; }
        }

        public SavedForecastsAdapter(JavaList<ForecastCard> data)
        {
            displayData = data;
        }

        public override int ItemCount
        {
            get { return displayData.Size(); }
        }

        public void AddRow(ForecastCard data)
        {
            displayData.Add(data);
            NotifyItemInserted(ItemCount - 1);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyViewholder h = holder as MyViewholder;
            UpdateCardContents(h, displayData[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.forecastCard, parent, false);
            MyViewholder holder = new MyViewholder(v);
            holder.checkboxSaved.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (!e.IsChecked)
                {
                    // On Checkbox uncheck
                    displayData.RemoveAt(holder.LayoutPosition);
                    NotifyItemRemoved(holder.LayoutPosition);
                }
            };
            return holder;
        }

        public void UpdateCardContents(MyViewholder vh, ForecastCard cardModel)
        {
            vh.dateLabel.Text = cardModel.DateFormated;
            vh.locationLabel.Text = cardModel.Location;
            vh.summaryLabel.Text = cardModel.Summary;
            vh.humidityLabel.Text = cardModel.Humidity;
            vh.windSpeedLabel.Text = cardModel.WindSpeed;
            vh.windDirectionLabel.Text = cardModel.WindDirection;
            vh.tidalLabel.Text = cardModel.WaterLevel;
            vh.SetIcon(vh.forecastIcon, cardModel.Icon);
        }

        internal class MyViewholder : RecyclerView.ViewHolder
        {
            public CheckBox checkboxSaved;
            public ImageView forecastIcon;

            public TextView dateLabel, locationLabel,
            summaryLabel, humidityLabel,
            windSpeedLabel, windDirectionLabel,
            tidalLabel;
            public MyViewholder(View itemView) : base(itemView)
            {
                checkboxSaved = itemView.FindViewById<CheckBox>(Resource.Id.checkboxSave);
                checkboxSaved.Checked = true;

                forecastIcon = itemView.FindViewById<ImageView>(Resource.Id.forecast_icon);
                InitLabels(itemView);
            }

            public void SetIcon(ImageView iconView, string icon)
            {
                int resId = Functions.GetIconDrawable(icon);
                iconView.SetBackgroundResource(resId);
            }
            public void InitLabels(View view)
            {
                dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
                locationLabel = view.FindViewById<TextView>(Resource.Id.locationLabel);
                summaryLabel = view.FindViewById<TextView>(Resource.Id.cardSummary);
                humidityLabel = view.FindViewById<TextView>(Resource.Id.cardHumidity);
                windSpeedLabel = view.FindViewById<TextView>(Resource.Id.cardWindSpeed);
                windDirectionLabel = view.FindViewById<TextView>(Resource.Id.cardWindDirection);
                tidalLabel = view.FindViewById<TextView>(Resource.Id.cardTidal);
            }
        }


    }
}