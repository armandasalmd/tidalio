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
        /// <summary>
        /// UI Components
        /// </summary>
        Spinner daySpinner, hourSpinner;
        CheckBox checkboxSaved;
        ImageView forecastIcon;
        TextView dateLabel, locationLabel,
            summaryLabel, humidityLabel,
            windSpeedLabel, windDirectionLabel,
            tidalLabel;
        AutoCompleteTextView autoComplete;
        LinearLayout root;

        /// <summary>
        /// Model properties
        /// </summary>
        ForecastCard cardModel;
        List<TidalStation> stations;
        Location locToShow;

        public Forecast() : base() { }

        /// <summary>
        /// Initialize fragment using preselected location. 
        /// It also automatically fetches location forecast
        /// </summary>
        /// <param name="loc">Location model object</param>
        public Forecast(Location loc) : base() => locToShow = loc;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cardModel = new ForecastCard();
        }

        /// <summary>
        /// Loads Day and Hour spinners with dropdown data. 
        /// Automatically selects current hour
        /// </summary>
        /// <param name="view">Root container</param>
        private void InitSpinners(View view)
        {
            string[] dayList = {
                "Today", "Tomorrow",
                Functions.GetDate(2),
                Functions.GetDate(3),
                Functions.GetDate(4)
            };
            string[] hourList = new string[24]; // "0" to "23"
            // generate hours 0 to 23 as a string
            for (int i = 0; i < 24; i++)
                hourList[i] = $"{i}:00";
            // bind spinner views to variables
            daySpinner = view.FindViewById<Spinner>(Resource.Id.dropdownPickDay);
            hourSpinner = view.FindViewById<Spinner>(Resource.Id.dropdownPickHour);
            // create spinner dropdown adapters
            var dayAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, dayList);
            var hourAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, hourList);

            dayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            hourAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            // set spinner dropdown adapters
            daySpinner.Adapter = dayAdapter;
            hourSpinner.Adapter = hourAdapter;
            // select current hour in the spinner
            hourSpinner.SetSelection(DateTimeOffset.Now.Hour);
        }

        /// <summary>
        /// A function that does validation, calls api according to 
        /// autoComplete component value and updates forecast card values
        /// </summary>
        /// <param name="lat">Optional location coordinate</param>
        /// <param name="lon">Optional location coordinate</param>
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
                    OnTidalStationMissingSearch();
            } else
                DoSnackbar("Location name is too short");
        }

        /// <summary>
        /// A function that calls dark sky api to get wheather data excluding tidal data
        /// </summary>
        /// <param name="lat">Optional. Location latitude</param>
        /// <param name="lon">Optional. Location longitude</param>
        public void OnTidalStationMissingSearch(double lat = 0, double lon = 0)
        {
            // Gets coordinates from autoComplete text location if not provided
            if (lat == 0 && lon == 0)
            {
                double[] coords = Functions.CalculateCoordinates(autoComplete.Text);
                lat = coords[1];
                lon = coords[0];
            }
            // Updates data model
            cardModel.Update(
                lat, lon,
                autoComplete.Text,
                string.Empty,
                (int)(daySpinner.SelectedItemId),
                Functions.HoursDeltaToNow(hourSpinner.SelectedItem.ToString())
            );
            // Updates UI components
            UpdateCardContents();
        }

        /// <summary>
        /// Updates every single UI component in the forecast card 
        /// using cardModel data
        /// </summary>
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

        /// <summary>
        /// Binds UI components to this class properties
        /// </summary>
        /// <param name="view">Root container</param>
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

        /// <summary>
        /// Fetches tidal station names and puts them into auto complete
        /// </summary>
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
            // Root view
            View view = inflater.Inflate(Resource.Layout.fragmentForecast, container, false);

            // Binding UI components to variables
            root = view.FindViewById<LinearLayout>(Resource.Id.root);
            checkboxSaved = view.FindViewById<CheckBox>(Resource.Id.checkboxSave);
            forecastIcon = view.FindViewById<ImageView>(Resource.Id.forecast_icon);
            autoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);

            // Initializing view components
            InitSpinners(view);
            InitLabels(view);
            UpdateCardContents();
            InitAutoComplete();

            // "Saved location" fragment row click sets locToShow
            if (locToShow != null)
            {
                autoComplete.Text = locToShow.Address;
                // Fetch and update forecast data
                OnSearch(locToShow.Longitude, locToShow.Latitude);
            }
            // Used to save forecast card which is later 
            // displayed in "Saved forecasts" fragment
            checkboxSaved.CheckedChange += CheckboxSaved_CheckedChange;

            return view;
        }

        /// <summary>
        /// On check - saved forecast card to web API.
        /// On uncheck - deletes forecast card from web API
        /// </summary>
        /// <param name="sender">Checkbox view</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Shows snackbar alert
        /// </summary>
        /// <param name="message">Message to display in snackbar</param>
        public void DoSnackbar(string message)
        {
            Snackbar snackBar = Snackbar.Make(root, message, Snackbar.LengthShort);
            snackBar.Show();
        }
    }
}