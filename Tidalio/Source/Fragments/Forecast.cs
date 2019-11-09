using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Tidalio
{
    [Obsolete]
    public class Forecast : Fragment
    {
        Spinner daySpinner, hourSpinner;
        CheckBox checkboxSaved;
        ImageView forecastIcon;
        TextView dateLabel, locationLabel,
            summaryLabel, humidityLabel,
            windSpeedLabel, windDirectionLabel,
            tidalLabel;

        ForecastCard cardModel;

        public ForecastCard CardModel
        {
            get { return cardModel; }
            set
            {
                cardModel = value;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cardModel = new ForecastCard();
        }

        private void InitSpinners(View view)
        {
            string[] dayList = { "Today", "Tomorrow", "11/11/2019", "12/11/2019", "13/11/2019" };
            string[] hourList = new string[24]; // "0" to "23"
            for (int i = 0; i < 24; i++)
                hourList[i] = $"{i}:00";

            daySpinner = view.FindViewById<Spinner>(Resource.Id.dropdownPickDay);
            hourSpinner = view.FindViewById<Spinner>(Resource.Id.dropdownPickHour);

            var dayAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, dayList);
            var hourAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, hourList);

            dayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            hourAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            daySpinner.Adapter = dayAdapter;
            hourSpinner.Adapter = hourAdapter;
        }

        public void OnSearch()
        {
            // update card component
            cardModel = Constants.GetSampleForecastCard();
            UpdateCardContents();
        }

        public void UpdateCardContents()
        {
            dateLabel.Text = cardModel.DateFormated;
            locationLabel.Text = cardModel.Location;
            summaryLabel.Text = cardModel.Summary;
            humidityLabel.Text = cardModel.Humidity;
            windSpeedLabel.Text = cardModel.WindSpeed;
            windDirectionLabel.Text = cardModel.WindDirection;
            tidalLabel.Text = cardModel.WaterLevel;
            forecastIcon.SetBackgroundResource(Functions.GetIconDrawable(cardModel.Icon));
        }

        public void InitLabels(View view)
        {
            dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
            locationLabel = view.FindViewById<TextView>(Resource.Id.locationLabel);
            summaryLabel = view.FindViewById<TextView>(Resource.Id.cardSummary);
            humidityLabel = view.FindViewById<TextView>(Resource.Id.cardHumidity);
            windSpeedLabel = view.FindViewById<TextView>(Resource.Id.cardWindSpeed);
            windDirectionLabel = view.FindViewById<TextView>(Resource.Id.cardWindDirection);
            tidalLabel= view.FindViewById<TextView>(Resource.Id.cardTidal);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragmentForecast, container, false);

            checkboxSaved = view.FindViewById<CheckBox>(Resource.Id.checkboxSave);
            forecastIcon = view.FindViewById<ImageView>(Resource.Id.forecast_icon);
            InitSpinners(view);
            InitLabels(view);
            UpdateCardContents();

            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}