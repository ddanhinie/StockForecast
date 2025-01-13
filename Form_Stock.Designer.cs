using System;

namespace Project_1
{
    partial class Form_Stock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button_load = new System.Windows.Forms.Button();
            this.openFileDialog_load = new System.Windows.Forms.OpenFileDialog();
            this.dateTimePicker_startDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_endDate = new System.Windows.Forms.DateTimePicker();
            this.label_startDate = new System.Windows.Forms.Label();
            this.label_endDate = new System.Windows.Forms.Label();
            this.chart_stockDisplay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fileSystemWatcher_load = new System.IO.FileSystemWatcher();
            this.comboBox_companyName = new System.Windows.Forms.ComboBox();
            this.comboBox_filters = new System.Windows.Forms.ComboBox();
            this.label_frequency = new System.Windows.Forms.Label();
            this.label_companyName = new System.Windows.Forms.Label();
            this.label_stockName = new System.Windows.Forms.Label();
            this.button_update = new System.Windows.Forms.Button();
            this.comboBox_pattern = new System.Windows.Forms.ComboBox();
            this.label_pattern = new System.Windows.Forms.Label();
            this.numericUpDown_leeway = new System.Windows.Forms.NumericUpDown();
            this.label_leeway = new System.Windows.Forms.Label();
            this.button_CalculateBeauty = new System.Windows.Forms.Button();
            this.textBox_PriceInput = new System.Windows.Forms.TextBox();
            this.label_PriceInput = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart_stockDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher_load)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_leeway)).BeginInit();
            this.SuspendLayout();
            // 
            // button_load
            // 
            this.button_load.BackColor = System.Drawing.Color.SlateBlue;
            this.button_load.Location = new System.Drawing.Point(601, 12);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(275, 98);
            this.button_load.TabIndex = 0;
            this.button_load.Text = "Load Stock(s)";
            this.button_load.UseVisualStyleBackColor = false;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // openFileDialog_load
            // 
            this.openFileDialog_load.Filter = "All Files|*.csv|Monthly|*Month.csv|Weekly|*Week.csv|Daily|*Day.csv|AAPL|AAPL-*.cs" +
    "v|DIS|DIS-*.csv|IBM|IBM-*.csv|INTC|INTC-*csv|PAYX|PAYX-*csv";
            this.openFileDialog_load.FilterIndex = 2;
            this.openFileDialog_load.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_load_FileOk);
            // 
            // dateTimePicker_startDate
            // 
            this.dateTimePicker_startDate.Location = new System.Drawing.Point(36, 54);
            this.dateTimePicker_startDate.Name = "dateTimePicker_startDate";
            this.dateTimePicker_startDate.Size = new System.Drawing.Size(519, 38);
            this.dateTimePicker_startDate.TabIndex = 1;
            this.dateTimePicker_startDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_endDate
            // 
            this.dateTimePicker_endDate.Location = new System.Drawing.Point(36, 145);
            this.dateTimePicker_endDate.Name = "dateTimePicker_endDate";
            this.dateTimePicker_endDate.Size = new System.Drawing.Size(519, 38);
            this.dateTimePicker_endDate.TabIndex = 2;
            // 
            // label_startDate
            // 
            this.label_startDate.AutoSize = true;
            this.label_startDate.Location = new System.Drawing.Point(30, 19);
            this.label_startDate.Name = "label_startDate";
            this.label_startDate.Size = new System.Drawing.Size(202, 32);
            this.label_startDate.TabIndex = 3;
            this.label_startDate.Text = "Pick Start Date";
            // 
            // label_endDate
            // 
            this.label_endDate.AutoSize = true;
            this.label_endDate.Location = new System.Drawing.Point(30, 110);
            this.label_endDate.Name = "label_endDate";
            this.label_endDate.Size = new System.Drawing.Size(193, 32);
            this.label_endDate.TabIndex = 4;
            this.label_endDate.Text = "Pick End Date";
            // 
            // chart_stockDisplay
            // 
            this.chart_stockDisplay.BackColor = System.Drawing.Color.Lavender;
            chartArea1.Name = "ChartArea_OHLC";
            chartArea2.AlignWithChartArea = "ChartArea_OHLC";
            chartArea2.Name = "ChartArea_Volume";
            this.chart_stockDisplay.ChartAreas.Add(chartArea1);
            this.chart_stockDisplay.ChartAreas.Add(chartArea2);
            this.chart_stockDisplay.Location = new System.Drawing.Point(-6, 196);
            this.chart_stockDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.chart_stockDisplay.Name = "chart_stockDisplay";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Chartreuse";
            series1.Name = "Series_OHLC";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueMembers = "High,Low,Open,Close";
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.UInt64;
            series2.ChartArea = "ChartArea_Volume";
            series2.Name = "Series_Volume";
            series2.XValueMember = "Date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series2.YValueMembers = "Volume";
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.UInt64;
            this.chart_stockDisplay.Series.Add(series1);
            this.chart_stockDisplay.Series.Add(series2);
            this.chart_stockDisplay.Size = new System.Drawing.Size(3000, 1600);
            this.chart_stockDisplay.TabIndex = 11;
            this.chart_stockDisplay.Text = "chart1";
            this.chart_stockDisplay.Click += new System.EventHandler(this.comboBox_filters_SelectedIndexChanged);
            this.chart_stockDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.chart_stockDisplay_Paint);
            this.chart_stockDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart_stockDisplay_MouseDown);
            this.chart_stockDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_stockDisplay_MouseMove);
            this.chart_stockDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_stockDisplay_MouseUp);
            // 
            // fileSystemWatcher_load
            // 
            this.fileSystemWatcher_load.EnableRaisingEvents = true;
            this.fileSystemWatcher_load.SynchronizingObject = this;
            // 
            // comboBox_companyName
            // 
            this.comboBox_companyName.FormattingEnabled = true;
            this.comboBox_companyName.Location = new System.Drawing.Point(949, 53);
            this.comboBox_companyName.Name = "comboBox_companyName";
            this.comboBox_companyName.Size = new System.Drawing.Size(239, 39);
            this.comboBox_companyName.TabIndex = 10;
            this.comboBox_companyName.SelectedIndexChanged += new System.EventHandler(this.comboBox_companyName_SelectedIndexChanged);
            // 
            // comboBox_filters
            // 
            this.comboBox_filters.FormattingEnabled = true;
            this.comboBox_filters.Location = new System.Drawing.Point(949, 144);
            this.comboBox_filters.Name = "comboBox_filters";
            this.comboBox_filters.Size = new System.Drawing.Size(239, 39);
            this.comboBox_filters.TabIndex = 11;
            this.comboBox_filters.SelectedIndexChanged += new System.EventHandler(this.comboBox_filters_SelectedIndexChanged);
            // 
            // label_frequency
            // 
            this.label_frequency.AutoSize = true;
            this.label_frequency.Location = new System.Drawing.Point(943, 109);
            this.label_frequency.Name = "label_frequency";
            this.label_frequency.Size = new System.Drawing.Size(122, 32);
            this.label_frequency.TabIndex = 12;
            this.label_frequency.Text = "Duration";
            // 
            // label_companyName
            // 
            this.label_companyName.AutoSize = true;
            this.label_companyName.Location = new System.Drawing.Point(943, 19);
            this.label_companyName.Name = "label_companyName";
            this.label_companyName.Size = new System.Drawing.Size(135, 32);
            this.label_companyName.TabIndex = 13;
            this.label_companyName.Text = "Company";
            // 
            // label_stockName
            // 
            this.label_stockName.AutoSize = true;
            this.label_stockName.Location = new System.Drawing.Point(30, 196);
            this.label_stockName.Name = "label_stockName";
            this.label_stockName.Size = new System.Drawing.Size(0, 32);
            this.label_stockName.TabIndex = 14;
            // 
            // button_update
            // 
            this.button_update.BackColor = System.Drawing.Color.MediumPurple;
            this.button_update.Location = new System.Drawing.Point(625, 116);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(229, 70);
            this.button_update.TabIndex = 15;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = false;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // comboBox_pattern
            // 
            this.comboBox_pattern.BackColor = System.Drawing.Color.Thistle;
            this.comboBox_pattern.FormattingEnabled = true;
            this.comboBox_pattern.Location = new System.Drawing.Point(1336, 102);
            this.comboBox_pattern.Name = "comboBox_pattern";
            this.comboBox_pattern.Size = new System.Drawing.Size(239, 39);
            this.comboBox_pattern.TabIndex = 16;
            this.comboBox_pattern.Text = "All";
            this.comboBox_pattern.SelectedIndexChanged += new System.EventHandler(this.comboBox_pattern_SelectedIndexChanged);
            // 
            // label_pattern
            // 
            this.label_pattern.AutoSize = true;
            this.label_pattern.Location = new System.Drawing.Point(1330, 67);
            this.label_pattern.Name = "label_pattern";
            this.label_pattern.Size = new System.Drawing.Size(106, 32);
            this.label_pattern.TabIndex = 17;
            this.label_pattern.Text = "Pattern";
            // 
            // numericUpDown_leeway
            // 
            this.numericUpDown_leeway.BackColor = System.Drawing.Color.MistyRose;
            this.numericUpDown_leeway.Location = new System.Drawing.Point(1698, 104);
            this.numericUpDown_leeway.Name = "numericUpDown_leeway";
            this.numericUpDown_leeway.Size = new System.Drawing.Size(168, 38);
            this.numericUpDown_leeway.TabIndex = 20;
            this.numericUpDown_leeway.ValueChanged += new System.EventHandler(this.numericUpDown_leeway_ValueChanged);
            // 
            // label_leeway
            // 
            this.label_leeway.AutoSize = true;
            this.label_leeway.Location = new System.Drawing.Point(1692, 67);
            this.label_leeway.Name = "label_leeway";
            this.label_leeway.Size = new System.Drawing.Size(112, 32);
            this.label_leeway.TabIndex = 21;
            this.label_leeway.Text = "Leeway";
            // 
            // button_CalculateBeauty
            // 
            this.button_CalculateBeauty.BackColor = System.Drawing.Color.Honeydew;
            this.button_CalculateBeauty.Location = new System.Drawing.Point(2028, 102);
            this.button_CalculateBeauty.Name = "button_CalculateBeauty";
            this.button_CalculateBeauty.Size = new System.Drawing.Size(367, 81);
            this.button_CalculateBeauty.TabIndex = 22;
            this.button_CalculateBeauty.Text = "Calculate Beauty at Price";
            this.button_CalculateBeauty.UseVisualStyleBackColor = false;
            this.button_CalculateBeauty.Click += new System.EventHandler(this.button_CalculateBeauty_Click);
            // 
            // textBox_PriceInput
            // 
            this.textBox_PriceInput.Location = new System.Drawing.Point(2268, 54);
            this.textBox_PriceInput.Name = "textBox_PriceInput";
            this.textBox_PriceInput.Size = new System.Drawing.Size(110, 38);
            this.textBox_PriceInput.TabIndex = 23;
            this.textBox_PriceInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_PriceInput
            // 
            this.label_PriceInput.AutoSize = true;
            this.label_PriceInput.BackColor = System.Drawing.Color.Lavender;
            this.label_PriceInput.Location = new System.Drawing.Point(2032, 60);
            this.label_PriceInput.Name = "label_PriceInput";
            this.label_PriceInput.Size = new System.Drawing.Size(230, 32);
            this.label_PriceInput.TabIndex = 24;
            this.label_PriceInput.Text = "Enter Price Here:";
            // 
            // Form_Stock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(2968, 1512);
            this.Controls.Add(this.label_PriceInput);
            this.Controls.Add(this.textBox_PriceInput);
            this.Controls.Add(this.button_CalculateBeauty);
            this.Controls.Add(this.label_leeway);
            this.Controls.Add(this.numericUpDown_leeway);
            this.Controls.Add(this.label_pattern);
            this.Controls.Add(this.comboBox_pattern);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.label_stockName);
            this.Controls.Add(this.label_companyName);
            this.Controls.Add(this.label_frequency);
            this.Controls.Add(this.comboBox_filters);
            this.Controls.Add(this.comboBox_companyName);
            this.Controls.Add(this.chart_stockDisplay);
            this.Controls.Add(this.label_endDate);
            this.Controls.Add(this.label_startDate);
            this.Controls.Add(this.dateTimePicker_endDate);
            this.Controls.Add(this.dateTimePicker_startDate);
            this.Controls.Add(this.button_load);
            this.Name = "Form_Stock";
            this.Text = "Input Form to Load Stock";
            ((System.ComponentModel.ISupportInitialize)(this.chart_stockDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher_load)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_leeway)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void chart_stockDisplay_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.OpenFileDialog openFileDialog_load;
        private System.Windows.Forms.DateTimePicker dateTimePicker_startDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_endDate;
        private System.Windows.Forms.Label label_startDate;
        private System.Windows.Forms.Label label_endDate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_stockDisplay;
        private System.IO.FileSystemWatcher fileSystemWatcher_load;
        private System.Windows.Forms.ComboBox comboBox_companyName;
        private System.Windows.Forms.ComboBox comboBox_filters;
        private System.Windows.Forms.Label label_companyName;
        private System.Windows.Forms.Label label_frequency;
        private System.Windows.Forms.Label label_stockName;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Label label_pattern;
        private System.Windows.Forms.ComboBox comboBox_pattern;
        private System.Windows.Forms.Label label_leeway;
        private System.Windows.Forms.NumericUpDown numericUpDown_leeway;
        private System.Windows.Forms.TextBox textBox_PriceInput;
        private System.Windows.Forms.Button button_CalculateBeauty;
        private System.Windows.Forms.Label label_PriceInput;
    }
}