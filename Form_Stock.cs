/*
Nhi Nguyen - U19914074
COP 4365
Project 3
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Project_1.Candlestick;

namespace Project_1
{
    /// <summary>
    /// Form for viewing and analyzing stock data with candlestick charts.
    /// </summary>
    public partial class Form_Stock : Form
    {
        // Holds the entire list of Candlestick data loaded from the file
        private List<Candlestick> candlesticks = null;

        // Holds the filtered Candlestick data (within a selected date range) for data binding
        private BindingList<Candlestick> filteredCandlesticks = null;

        private string selectedCompany = ""; // Stores the selected company name
        private string selectedFrequency = ""; // Stores the selected frequency (Daily, Weekly, or Monthly)

        /// <summary>
        /// Initializes a new instance of the Form_Stock class, setting up necessary components.
        /// </summary>
        public Form_Stock()
        {
            InitializeComponent(); // Initializes form components
            candlesticks = new List<Candlestick>(1024); // Preallocate space for 1024 Candlestick objects
            filteredCandlesticks = new BindingList<Candlestick>(); // Initialize filtered candlestick list
            InitializeComboBoxes(); // Populate combo boxes for company names and filter frequency options
            InitializeOpenFileDialog(); // Set up the file dialog for opening stock data files
            ConfigureChart(); // Configure chart settings (e.g., avoid gaps in X values)
            ConfigureMouseEvents();
        }

        public Form_Stock(string pathname, DateTime start, DateTime end)
        {
            // intialize necessary components for the form to start
            InitializeComponent();
            InitializeComboBoxes(); // Populate combo boxes for company names and frequency filters
            InitializeOpenFileDialog(); // Set up the file dialog for opening stock data files
            ConfigureChart(); // Configure chart settings
            ConfigureMouseEvents();

            // Assign the start time to the input DateTime start parameter
            dateTimePicker_startDate.Value = start;

            // Assign the end time to the input DateTime end parameter
            dateTimePicker_endDate.Value = end;

            // Assign the list of candlesticks to the return of the LoadCandlestickData method that takes the pathname input parameters
            LoadCandlestickData(pathname);

            // Filter the candlesticks using the ApplyDateFilter method with start and end parameters
            ApplyDateFilter(start, end);

            // Normalize the chart based on the BindingList of candlesticks (avoid gaps)
            NormalizeChart();

            // Display the candlesticks on the chart
            BindDataToViews();

            // Clear any previous annotations from the chart
            chart_stockDisplay.Annotations.Clear();

            // Detect peaks and valleys in the candlestick data and annotate the chart
            DetectPeakandValley();
        }

        /// <summary>
        /// Populates the combo boxes with company names and frequency filters.
        /// </summary>
        private void InitializeComboBoxes()
        {
            // Add company options to the comboBox_companyName
            comboBox_companyName.Items.AddRange(new string[] { "All Companies", "AAPL", "DIS", "IBM", "INTC", "PAYX" });

            // Add frequency options to the comboBox_filters
            comboBox_filters.Items.AddRange(new string[] { "Daily", "Weekly", "Monthly" });

            // Add pattern options to the comboBox_pattern for filtering candlestick patterns
            comboBox_pattern.Items.AddRange(new string[] { "All", "Bearish", "Bullish", "Neutral", "Marubozu", "Hammer", "Doji", "Dragonfly Doji", "Gravestone Doji" });

            // Set default selections for company name, frequency filter, and pattern
            comboBox_companyName.SelectedIndex = 0; // Default to the first company
            comboBox_filters.SelectedIndex = 0; // Default to the first frequency option
        }

        private void InitializeOpenFileDialog()
        {
            // Allow the user to select multiple files (stock data files)
            this.openFileDialog_load.Multiselect = true;

            // Set the title for the file dialog
            this.openFileDialog_load.Title = "Select Stock(s)";
        }

        private void ConfigureChart()
        {
            // Clear any existing annotations from the chart
            chart_stockDisplay.Annotations.Clear();

            // Configure the "Series_OHLC" series to avoid gaps in the X axis
            chart_stockDisplay.Series["Series_OHLC"].IsXValueIndexed = true;

            // Configure the "Series_Volume" series to avoid gaps in the X axis
            chart_stockDisplay.Series["Series_Volume"].IsXValueIndexed = true;
        }

        /// <summary>
        /// Event handler that updates the selected frequency when a new option is chosen.
        /// </summary>
        private void comboBox_filters_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assign the selected frequency to the variable
            selectedFrequency = comboBox_filters.SelectedItem.ToString(); // Get the selected frequency (Daily, Weekly, or Monthly)
        }

        /// <summary>
        /// Event handler that updates the selected company when a new option is chosen.
        /// </summary>
        private void comboBox_companyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assign the selected company name to the variable
            selectedCompany = comboBox_companyName.SelectedItem.ToString(); // Get the selected company name (e.g., AAPL, DIS, IBM)
        }

        /// <summary>
        /// Constructs the filter string for OpenFileDialog based on the selected company and frequency.
        /// </summary>
        private string ConstructFilterString()
        {
            // Base filter for showing all files
            string filter = "All Files|*.*"; // Allow all file types by default

            // Append specific filter options based on the frequency
            switch (selectedFrequency)
            {
                case "Daily":
                    if (selectedCompany == "All Companies") { filter += $"|Daily|*-Day.csv"; } // If "All Companies" is selected, show all Daily files
                    else { filter += $"|{selectedCompany} Daily|{selectedCompany}-Day.csv"; } // If a specific company is selected, show Daily files for that company
                    break;
                case "Weekly":
                    if (selectedCompany == "All Companies") { filter += $"|Weekly|*-Week.csv"; } // If "All Companies" is selected, show all Weekly files
                    else { filter += $"|{selectedCompany} Weekly|{selectedCompany}-Week.csv"; } // If a specific company is selected, show Weekly files for that company
                    break;
                case "Monthly":
                    if (selectedCompany == "All Companies") { filter += $"|Monthly|*-Month.csv"; } // If "All Companies" is selected, show all Monthly files
                    else { filter += $"|{selectedCompany} Monthly|{selectedCompany}-Month.csv"; } // If a specific company is selected, show Monthly files for that company
                    break;
            }

            return filter; // Return the constructed filter string for the OpenFileDialog
        }

        /// <summary>
        /// Handles the event when the load button is clicked.
        /// </summary>
        private void button_load_Click(object sender, EventArgs e)
        {
            this.Text = "Loading stock data..."; // Update form title to show loading status while data is being loaded
            openFileDialog_load.Filter = ConstructFilterString(); // Apply the dynamic filter string created based on the selected frequency and company
            openFileDialog_load.ShowDialog(); // Display the OpenFileDialog to the user to select files
            ClearFinalizedRectangle();
        }

        /// <summary>
        /// Event handler for when a file is selected in OpenFileDialog.
        /// </summary>
        private void openFileDialog_load_FileOk(object sender, CancelEventArgs e)
        {
            DisplayStock(); // Call the method to display the selected stock data on the form
        }

        /// <summary>
        /// Displays stock data for one or more selected files.        
        /// </summary>
        private void DisplayStock()
        {
            int numOfFiles = openFileDialog_load.FileNames.Count(); // Get the number of selected files
            DateTime startDate = dateTimePicker_startDate.Value; // Selected start date from the date picker
            DateTime endDate = dateTimePicker_endDate.Value; // Selected end date from the date picker

            // Loop through each selected file
            for (int i = 0; i < numOfFiles; i++)
            {
                string pathName = openFileDialog_load.FileNames[i]; // Get the file path for the current file
                string ticker = Path.GetFileNameWithoutExtension(pathName); // Extract the ticker symbol from the file name (without extension)
                Form_Stock form_Stock; // Declare a new form for stock display

                if (i == 0)
                {
                    form_Stock = this; // If it's the first file, use the current form as the main form
                                       // This method reads data from the file and transforms it into a list of candlestick data
                    LoadCandlestickData(pathName);
                    // Apply date filter to the candlestick data and store it in a BindingList
                    ApplyDateFilter(startDate, endDate);
                    // Normalize the chart area (remove gaps or adjust the data presentation)
                    NormalizeChart();
                    // Bind the filtered candlestick data to the views (data grid and chart)
                    BindDataToViews();
                    // Set the form's title to indicate it's the "parent" form along with the ticker symbol
                    form_Stock.Text = "Parent: " + ticker;
                    chart_stockDisplay.Annotations.Clear(); // Clear any previous chart annotations
                    DetectPeakandValley(); // Detect and mark peaks and valleys in the candlestick data
                }
                // If it's not the first file, create a new child form to display the stock data
                else
                {
                    // Construct a new Form_Stock object for each additional file selected
                    form_Stock = new Form_Stock(pathName, startDate, endDate);
                    // Set the form's title to indicate it's a "child" form along with the ticker symbol
                    form_Stock.Text = "Child: " + ticker;
                    // Show the new form to the screen
                    form_Stock.Show();
                }
            }
        }

        /// <summary>
        /// Reads and parses stock data from the selected file.
        /// </summary>
        private void LoadCandlestickData(string filename)
        {
            // Load the file and parse it into a list of Candlestick objects
            candlesticks = ParseFile(filename); // Call ParseFile to process the file and store the result in candlesticks
            label_stockName.Text = Path.GetFileNameWithoutExtension(filename); // Display the stock name (filename without extension) on the label
        }

        /// <summary>
        /// Parses the stock data file and returns a list of Candlestick objects.
        /// </summary>
        private List<Candlestick> ParseFile(string filename)
        {
            var parsedData = new List<Candlestick>(); // Holds the parsed data in a list of Candlestick objects
            const string header = "Date,Open,High,Low,Close,Volume"; // Expected CSV header format

            using (var reader = new StreamReader(filename)) // Open the file for reading
            {
                parsedData.Clear(); // Clear any existing data in parsedData before reading new data
                string line = reader.ReadLine(); // Read the first line (header)

                if (line == header) // Ensure file has the correct header format
                {
                    // Read each line after the header, create a Candlestick object from it, and add it to the list
                    while ((line = reader.ReadLine()) != null) // Read until the end of the file
                    {
                        var candlestick = new Candlestick(line); // Create a Candlestick object from the current line
                        parsedData.Add(candlestick); // Add the Candlestick object to the parsedData list
                    }
                }
                else
                {
                    this.Text = $"Invalid File Format: {filename}"; // Display an error message if the header format is incorrect
                }
            }

            return parsedData; // Return the list of Candlestick objects parsed from the file
        }

        /// <summary>
        /// Filters candlestick data by date range and updates the BindingList for data binding.
        /// </summary>
        private void ApplyDateFilter(DateTime startDate, DateTime endDate)
        {
            // Filter candlesticks within the specified date range and update the filtered list
            filteredCandlesticks = new BindingList<Candlestick>(
                candlesticks.Where(cs => cs.Date >= startDate && cs.Date <= endDate).ToList() // Filter the candlesticks list by the specified date range
            );
        }

        /// <summary>
        /// Binds the filtered candlestick data to the DataGridView and Chart controls.
        /// </summary>
        private void BindDataToViews()
        {
            chart_stockDisplay.DataSource = filteredCandlesticks; // Bind the filtered candlestick data to the Chart control
            chart_stockDisplay.DataBind(); // Refresh the chart display to show the updated data
        }

        /// <summary>
        /// Normalizes the chart display by expanding the Y-axis range slightly.
        /// </summary>
        private void NormalizeChart()
        {
            // Check if filtered data list is empty
            if (filteredCandlesticks.Count == 0 && openFileDialog_load.FileNames.Count() > 0)
            {
                // Show error message if there’s no data to display
                MessageBox.Show("Please Check Date Input", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if there’s no data to normalize
            }

            // Normalize chart with the minimum "low" and maximum "high" values from filtered data
            NormalizeChart(
                filteredCandlesticks.Min(obj => obj.Low), // Get the minimum low value from the filtered candlesticks
                filteredCandlesticks.Max(obj => obj.High) // Get the maximum high value from the filtered candlesticks
            );
        }

        /// <summary>
        /// Adjusts the chart Y-axis based on the min and max values with a 2% margin.
        /// </summary>
        /// <param name="min">The minimum "low" value.</param>
        /// <param name="max">The maximum "high" value.</param>
        private void NormalizeChart(decimal min, decimal max)
        {
            // Calculate adjusted values with a 2% margin for both min and max values
            decimal adjustedMin = min - (min * 0.02m); // Decrease the minimum by 2%
            decimal adjustedMax = max + (max * 0.02m); // Increase the maximum by 2%

            // Set the Y-axis minimum and maximum in the chart area to the adjusted values
            chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = (double)adjustedMin; // Set the min value on the Y-axis
            chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = (double)adjustedMax; // Set the max value on the Y-axis
        }

        /// <summary>
        /// Handles the Click event for the "Update" button. 
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            // Apply the date filter using the selected start and end dates from the date pickers
            ApplyDateFilter(dateTimePicker_startDate.Value, dateTimePicker_endDate.Value);
            // Normalize the chart based on the filtered candlestick data
            NormalizeChart();
            // Bind the filtered data to the views (chart, etc.)
            BindDataToViews();
            // Clear any existing annotations from the chart
            chart_stockDisplay.Annotations.Clear();
            // Detect peaks and valleys in the data and annotate them on the chart
            DetectPeakandValley();
            ClearFinalizedRectangle();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event for the pattern combo box. 
        /// </summary>
        private void comboBox_pattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assign the currently selected item in the combo box to the selectedPattern string
            string selectedPattern = comboBox_pattern.SelectedItem.ToString();
            // Call the Pattern method to update the chart based on the selected pattern
            Pattern(selectedPattern);
            ClearFinalizedRectangle();
        }

        /// <summary>
        /// Filters the candlestick data based on the selected pattern and updates the chart, clearing previous annotations and detecting new peaks and valleys.
        /// </summary>
        /// <param name="selectedPattern">The candlestick pattern selected from the combo box.</param>
        private void Pattern(string selectedPattern)
        {
            // Create a new BindingList to store all filtered candlesticks
            BindingList<Candlestick> allFilteredCandlesticks = new BindingList<Candlestick>();
            // Add all filtered candlesticks to the new list
            foreach (Candlestick candlestick in filteredCandlesticks)
            {
                allFilteredCandlesticks.Add(candlestick);
            }

            // If the selected pattern is not "All", filter the candlesticks based on the pattern
            if (selectedPattern != "All")
            {
                // Loop through the filtered candlesticks to apply the selected pattern filter
                for (int i = 0; i < allFilteredCandlesticks.Count(); i++)
                {
                    // Create a SmartCandlestick object for each candlestick to check its pattern
                    SmartCandlestick smc = new SmartCandlestick(allFilteredCandlesticks[i]);
                    // Check if the current candlestick matches the selected pattern
                    if ((smc.IsBearish == false && selectedPattern == "Bearish")
                        || (smc.IsBullish == false && selectedPattern == "Bullish")
                        || (smc.IsNeutral == false && selectedPattern == "Neutral")
                        || (smc.IsDoji == false && selectedPattern == "Doji")
                        || (smc.IsMarubozu == false && selectedPattern == "Marubozu")
                        || (smc.IsHammer == false && selectedPattern == "Hammer")
                        || (smc.IsDragonflyDoji == false && selectedPattern == "Dragonfly Doji")
                        || (smc.IsGravestoneDoji == false && selectedPattern == "Gravestone Doji"))
                    {
                        // If the candlestick does not match the selected pattern, remove it from the filtered list
                        Candlestick temp = allFilteredCandlesticks[i];
                        filteredCandlesticks.Remove(temp);
                    }
                }
            }

            // If there are no candlesticks left after filtering, show an error message
            if (filteredCandlesticks.Count() == 0)
            {
                MessageBox.Show("There is no " + selectedPattern + " candlestick in this period!"); // Display message box with the error
            }
            // If there is at least one candlestick after choosing a pattern
            else
            {
                // Normalize the chart again after filtering the data
                NormalizeChart();
                // Bind the updated data to the views
                BindDataToViews();
                // Clear any annotations from the chart
                chart_stockDisplay.Annotations.Clear();
                // Detect and annotate peaks and valleys again after filtering
                DetectPeakandValley();

            }
            // Update the filtered candlesticks list to the new filtered list
            filteredCandlesticks = allFilteredCandlesticks;
        }

        /// <summary>
        /// Identifies peaks and valleys in the filtered candlestick data and adds annotations to the chart.
        /// A peak is the highest point surrounded by lower values, and a valley is the lowest point surrounded by higher values.
        /// </summary>
        private void DetectPeakandValley()
        {
            // Iterate through each candlestick in the filtered data
            for (int i = 0; i < filteredCandlesticks.Count(); i++)
            {
                // Create a SmartCandlestick object for the current candlestick to evaluate its properties
                SmartCandlestick smc = new SmartCandlestick(filteredCandlesticks[i]);

                // Handle the first candlestick (edge case: no previous candlestick)
                if (i == 0)
                {
                    // Check if the current candlestick is a peak compared to the next candlestick
                    if (smc.IsPeak(filteredCandlesticks[i].High - 1, filteredCandlesticks[i + 1].High))
                    {
                        AddPeakAnnotation(i); // Annotate the peak on the chart
                    }
                    // Check if the current candlestick is a valley compared to the next candlestick
                    if (smc.IsValley(filteredCandlesticks[i].Low + 1, filteredCandlesticks[i + 1].Low))
                    {
                        AddValleyAnnotation(i); // Annotate the valley on the chart
                    }
                }
                // Handle the last candlestick (edge case: no next candlestick)
                else if (i == filteredCandlesticks.Count() - 1)
                {
                    // Check if the current candlestick is a peak compared to the previous candlestick
                    if (smc.IsPeak(filteredCandlesticks[i - 1].High, filteredCandlesticks[i].High - 1))
                    {
                        AddPeakAnnotation(i); // Annotate the peak on the chart
                    }
                    // Check if the current candlestick is a valley compared to the previous candlestick
                    if (smc.IsValley(filteredCandlesticks[i - 1].Low, filteredCandlesticks[i].Low + 1))
                    {
                        AddValleyAnnotation(i); // Annotate the valley on the chart
                    }
                }
                // Handle all intermediate candlesticks
                else
                {
                    // Check if the current candlestick is a peak compared to its neighbors (previous and next)
                    if (smc.IsPeak(filteredCandlesticks[i - 1].High, filteredCandlesticks[i + 1].High))
                    {
                        AddPeakAnnotation(i); // Annotate the peak on the chart
                    }
                    // Check if the current candlestick is a valley compared to its neighbors (previous and next)
                    if (smc.IsValley(filteredCandlesticks[i - 1].Low, filteredCandlesticks[i + 1].Low))
                    {
                        AddValleyAnnotation(i); // Annotate the valley on the chart
                    }
                }
            }
        }

        /// <summary>
        /// Adds a green "P" annotation at the specified index to mark a peak on the chart.
        /// </summary>
        /// <param name="i">The index of the candlestick in the filtered list representing the peak.</param>
        private void AddPeakAnnotation(int i)
        {
            // Add a green annotation at the peak point on the chart
            TextAnnotation peakAnnotation = new TextAnnotation
            {
                Text = "P", // Set the annotation text to "Peak"
                ForeColor = Color.Green, // Set the color to green
                Font = new Font("Arial", 10, FontStyle.Bold), // Set the font style
                AnchorDataPoint = chart_stockDisplay.Series["Series_OHLC"].Points[i], // Anchor it to the corresponding data point
            };
            // Add the peak annotation to the chart
            chart_stockDisplay.Annotations.Add(peakAnnotation);
        }

        /// <summary>
        /// Adds a red "V" annotation at the specified index to mark a valley on the chart.
        /// </summary>
        /// <param name="i">The index of the candlestick in the filtered list representing the valley.</param>
        private void AddValleyAnnotation(int i)
        {
            // Add a red annotation at the valley point on the chart
            TextAnnotation valleyAnnotation = new TextAnnotation
            {
                Text = "V", // Set the annotation text to "Valley"
                ForeColor = Color.Red, // Set the color to red
                Font = new Font("Arial", 10, FontStyle.Bold), // Set the font style
                AnchorDataPoint = chart_stockDisplay.Series["Series_OHLC"].Points[i], // Anchor it to the corresponding data point
            };
            // Add the valley annotation to the chart
            chart_stockDisplay.Annotations.Add(valleyAnnotation);
        }

        private bool isDrawing; // Tracks if the user is actively selecting
        private Point selectionStart; // Start point of the rectangle (in pixels)
        private Point selectionEnd; // End point of the rectangle (in pixels)
        private Rectangle? finalizedRectangle = null; // Represents the finalized selection rectangle
        List<Candlestick> selectedWave; // Stores the selected wave of candlesticks

        /// <summary>
        /// Configures mouse event handlers for wave selection on the chart.
        /// </summary>
        private void ConfigureMouseEvents()
        {
            chart_stockDisplay.MouseDown += chart_stockDisplay_MouseDown; // Triggered when the mouse button is pressed
            chart_stockDisplay.MouseMove += chart_stockDisplay_MouseMove; // Triggered when the mouse moves
            chart_stockDisplay.MouseUp += chart_stockDisplay_MouseUp; // Triggered when the mouse button is released
            chart_stockDisplay.Paint += chart_stockDisplay_Paint; // Triggered when the chart needs to be redrawn
        }

        /// <summary>
        /// Handles mouse down events to begin the wave selection process.
        /// </summary>
        private void chart_stockDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            ClearFinalizedRectangle(); // Clear any previously finalized rectangle
            if (e.Button == MouseButtons.Left) // Proceed only if the left mouse button is clicked
            {
                var result = chart_stockDisplay.HitTest(e.X, e.Y); // Identify the chart element under the mouse cursor

                // Check if a data point (candlestick) was clicked
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    // Retrieve the index of the clicked candlestick
                    int clickedPointIndex = result.PointIndex;

                    // Verify if the candlestick has a Peak (P) or Valley (V) annotation
                    bool isPeakOrValley = false;
                    foreach (var annotation in chart_stockDisplay.Annotations)
                    {
                        if (annotation is TextAnnotation textAnnotation)
                        {
                            // Check if the annotation corresponds to the clicked candlestick
                            if (textAnnotation.AnchorDataPoint == chart_stockDisplay.Series["Series_OHLC"].Points[clickedPointIndex] &&
                                (textAnnotation.Text == "P" || textAnnotation.Text == "V"))
                            {
                                isPeakOrValley = true; // Mark the candlestick as a valid selection point
                                break;
                            }
                        }
                    }

                    // Enable selection only if the candlestick is a Peak or Valley
                    if (isPeakOrValley)
                    {
                        isDrawing = true; // Enable drawing mode for the selection rectangle
                        selectionStart = SnapToCandlestick(e.Location); // Snap the start point to the nearest candlestick
                        selectionEnd = e.Location; // Initialize the selection end point
                    }
                    else
                    {
                        // Show a message if the selection is invalid
                        MessageBox.Show("Selection is only allowed on Peaks (P) or Valleys (V). Please reselect.");
                    }
                }
                else
                {
                    // Show a message if the mouse click is not on a candlestick
                    MessageBox.Show("Please click on a candlestick.");
                }
            }
        }

        /// <summary>
        /// Handles mouse move events to dynamically update the selection rectangle.
        /// </summary>
        private void chart_stockDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing) // Update only if drawing mode is enabled
            {
                selectionEnd = SnapToCandlestick(e.Location); // Snap the end point to the nearest candlestick
                chart_stockDisplay.Invalidate(); // Trigger a redraw to visualize the rectangle
            }
        }

        /// <summary>
        /// Handles mouse up events to finalize the wave selection process.
        /// </summary>
        private void chart_stockDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false; // Disable drawing mode
                selectionEnd = SnapToCandlestick(e.Location); // Finalize the end point position

                // Calculate the dimensions of the finalized rectangle
                int x = Math.Min(selectionStart.X, selectionEnd.X);
                int y = Math.Min(selectionStart.Y, selectionEnd.Y);
                int width = Math.Abs(selectionStart.X - selectionEnd.X);
                int height = Math.Abs(selectionStart.Y - selectionEnd.Y);
                finalizedRectangle = new Rectangle(x, y, width, height); // Store the finalized rectangle
                chart_stockDisplay.Invalidate(); // Trigger a final redraw

                // Process the candlesticks within the selected range
                HandleSelection();
            }
        }

        /// <summary>
        /// Snaps the mouse position to the nearest candlestick on the X-axis.
        /// </summary>
        private Point SnapToCandlestick(Point mousePosition)
        {
            var xAxis = chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisX;

            // Convert the mouse's X position in pixels to the corresponding X-axis value
            double xValue = xAxis.PixelPositionToValue(mousePosition.X);

            // Determine the nearest candlestick index based on the X-axis value
            double a = Math.Abs(xValue - Math.Floor(xValue));
            double b = Math.Abs(xValue - Math.Round(xValue));
            int nearestIndex;
            if (a < b)
            {
                nearestIndex = (int)Math.Floor(xValue);
            }
            else
            {
                nearestIndex = (int)Math.Round(xValue);
            }
            // Ensure the index is within bounds
            nearestIndex = Math.Max(0, Math.Min(nearestIndex, filteredCandlesticks.Count));

            // Convert the index back to pixel coordinates for snapping
            double snappedXValue = nearestIndex; // X-axis value of the nearest candlestick
            int snappedXPixel = (int)xAxis.ValueToPixelPosition(snappedXValue);

            // Return the adjusted position with the original Y-coordinate
            return new Point(snappedXPixel, mousePosition.Y);
        }

        /// <summary>
        /// Processes the candlesticks in the selected range and validates the wave.
        /// </summary>
        private void HandleSelection()
        {
            var xAxis = chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisX;

            // Convert the snapped pixel positions to X-axis values (candlestick indices)
            double xStart = xAxis.PixelPositionToValue(selectionStart.X);
            double xEnd = xAxis.PixelPositionToValue(selectionEnd.X);

            // Determine the start and end indices of the selected range
            int startIndex = (int)Math.Floor(xStart);
            int endIndex = (int)Math.Floor(xEnd);

            // Ensure indices are within valid bounds
            startIndex = Math.Max(0, Math.Min(startIndex, filteredCandlesticks.Count - 1));
            endIndex = Math.Max(0, Math.Min(endIndex, filteredCandlesticks.Count - 1));

            // Extract the selected candlesticks based on the calculated range
            var selectedCandlesticks = filteredCandlesticks
                .Skip(Math.Min(startIndex, endIndex))
                .Take(Math.Abs(endIndex - startIndex) + 1)
                .ToList();

            // Validate the selected wave and display appropriate messages
            if (ValidateWaveSelection(selectedCandlesticks))
            {
                selectedWave = selectedCandlesticks; // Store the validated wave
                MessageBox.Show($"Wave selected! Start: {selectedCandlesticks.First().Date}, End: {selectedCandlesticks.Last().Date}");
            }
            else
            {
                // Show an error if the wave is invalid
                MessageBox.Show("Wave is not VALID!", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the chart's Paint event to visualize selection rectangles and waves.
        /// </summary>
        private void chart_stockDisplay_Paint(object sender, PaintEventArgs e)
        {
            var xAxis = chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisX;
            var yAxis = chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisY;

            if (isDrawing || finalizedRectangle.HasValue)
            {
                // Determine the range of candlesticks in the selection
                double xStartValue = xAxis.PixelPositionToValue(selectionStart.X);
                double xEndValue = xAxis.PixelPositionToValue(selectionEnd.X);

                // Find nearest indices for candlesticks
                int startIndex = Math.Max(0, Math.Min((int)Math.Floor(xStartValue), filteredCandlesticks.Count - 1));
                int endIndex = Math.Max(0, Math.Min((int)Math.Floor(xEndValue), filteredCandlesticks.Count - 1));

                // Get the candlesticks in the range
                var selectedCandlesticks = filteredCandlesticks
                    .Skip(Math.Min(startIndex, endIndex))
                    .Take(Math.Abs(endIndex - startIndex) + 1)
                    .ToList();

                if (selectedCandlesticks.Any() && isDrawing)
                {
                    // Find the max high and min low of selected candlesticks
                    decimal maxHigh = selectedCandlesticks.Max(c => c.High);
                    decimal minLow = selectedCandlesticks.Min(c => c.Low);

                    // Convert these values to pixel positions for drawing
                    int yTop = (int)yAxis.ValueToPixelPosition((double)maxHigh);
                    int yBottom = (int)yAxis.ValueToPixelPosition((double)minLow);


                    // Calculate rectangle dimensions
                    int xLeft = Math.Min(selectionStart.X, selectionEnd.X);
                    int xRight = Math.Max(selectionStart.X, selectionEnd.X);
                    int width = xRight - xLeft;
                    int height = yBottom - yTop;

                    // Draw the rectangle
                    using (Brush brush = new SolidBrush(Color.FromArgb(40, Color.Blue)))
                    using (Pen pen = new Pen(Color.Blue, 1))
                    {
                        e.Graphics.FillRectangle(brush, xLeft, yTop, width, height);
                        e.Graphics.DrawRectangle(pen, xLeft, yTop, width, height);
                    }
                    DrawFibonacciLevels(e, selectedCandlesticks); // Draw Fibonacci levels for the selection
                }

                if (selectedWave != null && !isDrawing)
                {
                    // Draw a finalized wave rectangle and Fibonacci levels
                    decimal maxHigh = selectedWave.Max(c => c.High);
                    decimal minLow = selectedWave.Min(c => c.Low);
                    // Convert these values to pixel positions for drawing
                    int yTop = (int)yAxis.ValueToPixelPosition((double)maxHigh);
                    int yBottom = (int)yAxis.ValueToPixelPosition((double)minLow);

                    startIndex = filteredCandlesticks.IndexOf(selectedWave.First());
                    endIndex = filteredCandlesticks.IndexOf(selectedWave.Last());

                    int xLeft = (int)xAxis.ValueToPixelPosition(startIndex + 1);
                    int xRight = (int)xAxis.ValueToPixelPosition(endIndex + 1);


                    int width = xRight - xLeft;
                    int height = yBottom - yTop;

                    using (Brush brush = new SolidBrush(Color.FromArgb(40, Color.Blue)))
                    using (Pen pen = new Pen(Color.Blue, 1))
                    {
                        e.Graphics.FillRectangle(brush, xLeft, yTop, width, height);
                        e.Graphics.DrawRectangle(pen, xLeft, yTop, width, height);
                    }
                    DrawFibonacciLevels(e, selectedWave); // Draw Fibonacci levels for the wave
                }
            }
        }

        /// <summary>
        /// Validates the wave selection.
        /// </summary>
        /// <param name="candlesticks">The selected candlestick range.</param>
        /// <returns>True if valid, otherwise false.</returns>
        private bool ValidateWaveSelection(List<Candlestick> candlesticks)
        {
            if (candlesticks.Count < 2) return false; // Wave must have at least 2 candlesticks

            // Determine the rectangle bounds from the first and last candlesticks
            var first = candlesticks.First();
            var last = candlesticks.Last();
            decimal maxHigh = Math.Max(first.High, last.High);
            decimal minLow = Math.Min(first.Low, last.Low);

            // Check each intermediate candlestick against the rectangle bounds

            for (int i = 1; i < candlesticks.Count - 1; i++)
            {
                var current = candlesticks[i];
                if (current.High > maxHigh || current.Low < minLow)
                {
                    return false; // Invalid wave: An intermediate candlestick exceeds rectangle bounds
                }
            }
            return true; // The wave is valid
        }

        /// <summary>
        /// Clears the finalized selection rectangle.
        /// </summary>
        private void ClearFinalizedRectangle()
        {
            finalizedRectangle = null; // Reset the rectangle
            selectedWave = null; // Clear the selected wave
            chart_stockDisplay.Invalidate(); // Trigger a redraw
        }

        /// <summary>
        /// Draws Fibonacci levels on the chart for the given candlestick selection.
        /// </summary>
        private void DrawFibonacciLevels(PaintEventArgs e, List<Candlestick> selectedCandlesticks)
        {
            // Return early if no candlesticks are selected
            if (selectedCandlesticks == null || selectedCandlesticks.Count == 0)
                return;

            // Get the Y-axis of the chart for pixel conversion
            var yAxis = chart_stockDisplay.ChartAreas["ChartArea_OHLC"].AxisY;

            // Calculate the highest and lowest candlestick values
            decimal maxHigh = selectedCandlesticks.Max(c => c.High);
            decimal minLow = selectedCandlesticks.Min(c => c.Low);

            // Define Fibonacci retracement levels based on the range
            var levels = new Dictionary<string, decimal> //Downward wave
    {
        { "100%", maxHigh },
        { "76.4%", minLow + (maxHigh - minLow) * 0.764m },
        { "61.8%", minLow + (maxHigh - minLow) * 0.618m },
        { "50%", minLow + (maxHigh - minLow) * 0.5m },
        { "38.2%", minLow + (maxHigh - minLow) * 0.382m },
        { "23.6%", minLow + (maxHigh - minLow) * 0.236m },
        { "0%", minLow }
    };
            // Upward wave
            if (selectedCandlesticks.First().Low < selectedCandlesticks.Last().Low)
            {
                levels = new Dictionary<string, decimal>
    {
        { "0%", maxHigh },
        { "23.6%", minLow + (maxHigh - minLow) * 0.764m },
        { "38.2%", minLow + (maxHigh - minLow) * 0.618m },
        { "50%", minLow + (maxHigh - minLow) * 0.5m },
        { "61.8%", minLow + (maxHigh - minLow) * 0.382m },
        { "76.4%", minLow + (maxHigh - minLow) * 0.236m },
        { "100%", minLow }
    };
            }

            // Create pens and fonts for drawing on the chart
            using (Pen pen = new Pen(Color.DarkBlue, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            using (Font font = new Font("Arial", 9))
            using (Brush textBrush = new SolidBrush(Color.Black))
            {
                // Iterate over each Fibonacci level to draw lines and labels
                foreach (var level in levels)
                {
                    // Calculate the pixel position for the current level
                    int yPosition = (int)yAxis.ValueToPixelPosition((double)level.Value);

                    // Draw a horizontal line for the level
                    e.Graphics.DrawLine(pen, chart_stockDisplay.ChartAreas[0].Position.X, yPosition,
                        chart_stockDisplay.Width, yPosition);

                    // Add a label indicating the Fibonacci level
                    e.Graphics.DrawString(level.Key, font, textBrush, new PointF(chart_stockDisplay.Width - 60, yPosition - 10));
                }
            }
        }

        double percentLeeway = 0; // Default leeway value for beauty calculation
        /// <summary>
        /// Updates the percent leeway value and recalculates the beauty score on the chart
        /// </summary>
        private void numericUpDown_leeway_ValueChanged(object sender, EventArgs e)
        {
            // Update the percent leeway when the user changes the value
            percentLeeway = (double)numericUpDown_leeway.Value;
            RecalculateBeautyAndUpdateChart(); // Recalculate beauty with the new leeway
        }

        /// <summary>
        /// Counts the number of confirmations for a candlestick against Fibonacci levels within a given leeway
        /// </summary>
        private int CountConfirmations(Candlestick candlestick, List<double> fibonacciLevels, double leeway)
        {
            int confirmations = 0; // Initialize the counter for confirmations
            List<decimal> ohlc = new List<decimal> { candlestick.Open, candlestick.High, candlestick.Low, candlestick.Close };

            // Compare each OHLC value against Fibonacci levels within the specified leeway
            foreach (double ohlcValue in ohlc)
            {
                foreach (double fibLevel in fibonacciLevels)
                {
                    if (Math.Abs(ohlcValue - fibLevel) <= leeway)
                    {
                        confirmations++; // Increment count if within leeway
                    }
                }
            }

            return confirmations; // Return the total number of confirmations
        }

        /// <summary>
        /// Calculates the beauty score for a wave of candlesticks based on Fibonacci levels
        /// </summary>
        private double CalculateWaveBeauty(List<double> fibonacciLevels, List<Candlestick> waveCandlesticks)
        {
            double totalBeauty = 0; // Initialize beauty score
            double leeway = percentLeeway / 100 * (double)waveCandlesticks[0].Close; // Calculate leeway based on price

            // Sum confirmations for each candlestick in the wave
            foreach (var candlestick in waveCandlesticks)
            {
                totalBeauty += CountConfirmations(candlestick, fibonacciLevels, leeway);
            }

            return totalBeauty; // Return the beauty score
        }

        /// <summary>
        /// Computes the beauty score for a range of prices based on Fibonacci levels
        /// </summary>
        private List<(double price, double beauty)> ComputeBeautyOverPrices(double low, double high, List<Candlestick> waveCandlesticks, int steps)
        {
            var results = new List<(double price, double beauty)>(); // Initialize results list
            double stepSize = (high - low) / steps; // Calculate price increment per step

            // Case 1: Upward wave
            if (waveCandlesticks.First().Low < waveCandlesticks.Last().Low)
            {
                for (double price = high - 0.25 * (high - low); price <= high + 0.25 * (high - low); price += stepSize)
                {
                    var fibonacciLevels = ComputeFibonacciLevels(low, price);
                    double beauty = CalculateWaveBeauty(fibonacciLevels, waveCandlesticks);
                    results.Add((price, beauty)); // Add beauty calculation to results
                }
            }
            // Case 2: Downward wave
            else if (waveCandlesticks.First().High > waveCandlesticks.Last().High)
            {
                for (double price = low + 0.25 * (high - low); price >= low - 0.25 * (high - low); price -= stepSize)
                {
                    var fibonacciLevels = ComputeFibonacciLevels(price, high);
                    double beauty = CalculateWaveBeauty(fibonacciLevels, waveCandlesticks);
                    results.Add((price, beauty)); // Add beauty calculation to results
                }
            }

            return results; // Return the list of beauty values over prices
        }

        /// <summary>
        /// Computes Fibonacci levels for a given range of low and high prices
        /// </summary>
        List<double> ComputeFibonacciLevels(double low, double high)
        {
            double range = high - low; // Calculate the range between low and high prices
            return new List<double>
    {
        low,
        low + 0.236 * range,
        low + 0.382 * range,
        low + 0.5 * range,
        low + 0.618 * range,
        low + 0.764 * range,
        high
    }; // Return Fibonacci levels
        }

        /// <summary>
        /// Plots the beauty chart on the volume chart area
        /// </summary>
        private void PlotBeautyChart(List<(double price, double beauty)> beautyData)
        {
            // Clear the lower chart area (volume chart)
            var volumeArea = chart_stockDisplay.ChartAreas["ChartArea_Volume"];
            volumeArea.AlignWithChartArea = null;
            volumeArea.AxisX.Title = "Price";  // Set X-axis title
            volumeArea.AxisY.Title = "Beauty"; // Set Y-axis title

            // Remove existing series targeting the volume chart area
            foreach (var series in chart_stockDisplay.Series.Where(s => s.ChartArea == "ChartArea_Volume").ToList())
            {
                chart_stockDisplay.Series.Remove(series);
            }

            // Create a new series for beauty data
            var beautySeries = chart_stockDisplay.Series.Add("Series_Beauty");
            beautySeries.IsXValueIndexed = true;
            beautySeries.ChartType = SeriesChartType.Line; // Line chart
            beautySeries.ChartArea = "ChartArea_Volume"; // Bind to volume chart area
            beautySeries.XValueType = ChartValueType.UInt64;
            beautySeries.YValueType = ChartValueType.UInt64;

            // Add data points to the series
            foreach (var (price, beauty) in beautyData)
            {
                beautySeries.Points.AddXY(price, beauty);
            }
        }

        /// <summary>
        /// Recalculates the beauty chart based on the selected wave and price range
        /// </summary>
        private void RecalculateBeautyAndUpdateChart()
        {
            if (selectedWave != null) // Ensure a wave is selected
            {
                // Determine low and high values for the selected wave
                double low = (double)Math.Min(selectedWave.First().Low, selectedWave.Last().Low);
                double high = (double)Math.Max(selectedWave.First().High, selectedWave.Last().High);

                // Compute beauty values over price range and update the chart
                var beautyData = ComputeBeautyOverPrices(low, high, selectedWave, 20);
                PlotBeautyChart(beautyData);
            }
        }

        /// <summary>
        /// Handles the button click to calculate and display beauty at a user-specified price
        /// </summary>
        private void button_CalculateBeauty_Click(object sender, EventArgs e)
        {
            if (selectedWave != null && double.TryParse(textBox_PriceInput.Text, out double selectedPrice))
            {
                // Get low and high values for the selected wave
                double low = (double)Math.Min(selectedWave.First().Low, selectedWave.Last().Low);
                double high = (double)Math.Max(selectedWave.First().High, selectedWave.Last().High);

                // Validate the selected price and calculate beauty if valid
                if (selectedWave.First().Low < selectedWave.Last().Low && selectedPrice <= high + 0.5 * (high - low))
                {
                    var fibonacciLevels = ComputeFibonacciLevels(low, selectedPrice);
                    double beauty = CalculateWaveBeauty(fibonacciLevels, selectedWave);

                    // Display the calculated beauty score
                    MessageBox.Show($"Beauty at price ${selectedPrice:F2} is {beauty:F2}", "Beauty Calculation");
                }
                else if (selectedWave.First().High > selectedWave.Last().High && selectedPrice >= low - 0.5 * (high - low))
                {
                    var fibonacciLevels = ComputeFibonacciLevels(selectedPrice, high);
                    double beauty = CalculateWaveBeauty(fibonacciLevels, selectedWave);

                    // Display the calculated beauty score
                    MessageBox.Show($"Beauty at price {selectedPrice:F2} is {beauty:F2}", "Beauty Calculation");
                }
                else
                {
                    // Notify user if the price is invalid
                    MessageBox.Show("Selected price is outside the valid range.", "Invalid Input");
                }
            }
        }

    }
}