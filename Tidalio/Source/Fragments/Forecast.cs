using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
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
        AutoCompleteTextView autoComplete;
        LinearLayout root;

        ForecastCard cardModel;
        List<TidalStation> stations;
        Location locToShow;


        public Forecast() : base() { }
        public Forecast(Location loc) : base()
        {
            locToShow = loc;
        }

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
            string[] dayList = {
                "Today", "Tomorrow",
                Functions.GetDate(2),
                Functions.GetDate(3),
                Functions.GetDate(4)
            };
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

            hourSpinner.SetSelection(DateTimeOffset.Now.Hour);
        }

        public void OnSearch(double lat = 0, double lon = 0)
        {
            if (autoComplete.Text.Length >= 4)
            {
                string stationId = autoComplete.Text.Split(" ").Last();
                if (stationId.Length == 4 || stationId.Length == 5)
                {
                    foreach (TidalStation s in stations)
                    {
                        if (s.Id == stationId)
                        {
                            lat = s.Lat;
                            lon = s.Lon;
                            break;
                        }
                    }
                    if (cardModel != null && lat != 0 && lon != 0)
                    {
                        cardModel.Update(
                            lat, lon, 
                            autoComplete.Text, 
                            stationId, 
                            (int)(daySpinner.SelectedItemId), 
                            Functions.HoursDeltaToNow(hourSpinner.SelectedItem.ToString())
                            );
                        UpdateCardContents();
                    }
                    else
                    {
                        try
                        {
                            OnTidalStationMissingSearch();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        DoSnackbar("Tidal station was not found!");
                    }                    
                } else
                {
                    OnTidalStationMissingSearch();
                }
            } else
            {
                DoSnackbar("Location name is too short");
            }

            
        }

        public void OnTidalStationMissingSearch(double lat = 0, double lon = 0)
        {
            // search for forecast without tidal information
            double[] coords = new double[2];
            if (lat == 0 && lon == 0)
            {
                coords = Functions.CalculateCoordinates(autoComplete.Text);
                lat = coords[1];
                lon = coords[0];
            }
            cardModel.Update(
                lat, lon,
                autoComplete.Text,
                string.Empty,
                (int)(daySpinner.SelectedItemId),
                Functions.HoursDeltaToNow(hourSpinner.SelectedItem.ToString())
            );
            UpdateCardContents();
        }

        public void UpdateCardContents()
        {
            dateLabel.Text = cardModel.DateFormated;
            locationLabel.Text = cardModel.Location;
            summaryLabel.Text = $"{cardModel.Summary}|{cardModel.Temperature}";
            humidityLabel.Text = cardModel.Humidity;
            windSpeedLabel.Text = cardModel.WindSpeed;
            windDirectionLabel.Text = cardModel.WindDirection;
            tidalLabel.Text = cardModel.WaterLevel;
            forecastIcon.SetBackgroundResource(Functions.GetIconDrawable(cardModel.Icon));
            checkboxSaved.Checked = false;
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

        private void InitAutoComplete()
        {
            // TODO: in the future make this function async that content loads faster
            IList<string> completeOptions = new List<string>();
            stations = TidalApi.GetInstance().FetchStations();
            foreach(TidalStation s in stations)
            {
                completeOptions.Add(s.ToString());
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, completeOptions);
            autoComplete.Adapter = adapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragmentForecast, container, false);

            root = view.FindViewById<LinearLayout>(Resource.Id.root);
            checkboxSaved = view.FindViewById<CheckBox>(Resource.Id.checkboxSave);
            forecastIcon = view.FindViewById<ImageView>(Resource.Id.forecast_icon);
            autoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
            InitSpinners(view);
            InitLabels(view);
            UpdateCardContents();
            InitAutoComplete();
            // saved location fragment list item triggered sets locToShow
            if (locToShow != null)
            {
                autoComplete.Text = locToShow.Address;
                OnSearch(locToShow.Longitude, locToShow.Latitude);
            }

            checkboxSaved.CheckedChange += CheckboxSaved_CheckedChange;

            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void CheckboxSaved_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            string user_email = AuthHelper.GetInstance(Activity).CurrentUserEmail;
            if (cardModel.Location == "unknown")
            {
                ((CheckBox)sender).Checked = false;
                DoSnackbar("You cannot save empty forecast!");
                return;
            }
            if (e.IsChecked)
            {
                // post call to API
                if (!cardModel.IsSaved)
                {
                    TidalioApi.GetInstance().PostCardAsync(user_email, cardModel);
                    cardModel.IsSaved = true;
                }
            } else
            {
                // delete call to API
                if (cardModel.IsSaved)
                {
                    TidalioApi.GetInstance().DeleteCardAsync(user_email, cardModel);
                    cardModel.IsSaved = false;
                }
            }
        }

        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(root, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}