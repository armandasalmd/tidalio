using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Tidalio
{
    /// <summary>
    /// An adapter for RecyclerView component 
    /// used in Saved forecasts fragment to display forecast cards
    /// </summary>
    class SavedForecastsAdapter : RecyclerView.Adapter
    {
        /// <summary>
        /// List of data models to be displayed
        /// </summary>
        private readonly List<ForecastCard> displayData;
        
        /// <summary>
        /// Main activity context
        /// </summary>
        private Context context;

        public List<ForecastCard> DisplayData => displayData;

        /// <summary>
        /// Initialize adapter providing forecast model data
        /// </summary>
        /// <param name="ctx">Activity context</param>
        /// <param name="data">Display data for each row</param>
        public SavedForecastsAdapter(Context ctx, List<ForecastCard> data)
        {
            displayData = data;
            context = ctx;
        }

        public override int ItemCount => displayData.Count();

        /// <summary>
        /// Add and show new row for recycler view
        /// </summary>
        /// <param name="data">Data row to add</param>
        public void AddRow(ForecastCard data)
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
            UpdateCardContents(h, displayData[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.forecastCard, parent, false);
            MyViewholder holder = new MyViewholder(v);
            holder.checkboxSaved.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                // remove saved forecast card from web API and recyclerView
                if (!e.IsChecked)
                {
                    string user_email = AuthHelper.GetInstance(context).CurrentUserEmail;
                    TidalioApi.GetInstance().DeleteCardAsync(user_email, displayData[holder.LayoutPosition]);
                    // On Checkbox uncheck
                    displayData.RemoveAt(holder.LayoutPosition);
                    NotifyItemRemoved(holder.LayoutPosition);
                }
            };
            return holder;
        }

        /// <summary>
        /// Fills data from data model into the view
        /// </summary>
        /// <param name="vh">Row view</param>
        /// <param name="cardModel">Model data for row</param>
        public void UpdateCardContents(MyViewholder vh, ForecastCard cardModel)
        {
            vh.dateLabel.Text = cardModel.DateFormated;
            vh.locationLabel.Text = cardModel.Location;
            vh.summaryLabel.Text = cardModel.Summary + "|" + cardModel.Temperature;
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