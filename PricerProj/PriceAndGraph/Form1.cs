using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceAndGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numOfPaths = 100000;

                double spot = Double.Parse(textBox1.Text);
                double Strike = Double.Parse(textBox2.Text);
                double expiry = Double.Parse(textBox3.Text);
                double interest = Double.Parse(textBox4.Text);
                double vol = Double.Parse(textBox5.Text);

                double mean = Math.Pow((1.0 + interest), (1.0 / 252)) - 1.0;
                double stdDev = vol / Math.Sqrt(252);

                PricerProj.MCGenerator monty = new PricerProj.MCGenerator(mean, stdDev, 1.0);

                List<double> results = monty.generatePaths(spot, numOfPaths, expiry);

                double average = results.Average();

                textBox7.Text = Math.Max(average - Strike, 0.0).ToString();
            }
            catch
            {
                MessageBox.Show("Please check all inputs are entered");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int numOfPaths = Convert.ToInt32(textBox6.Text);

                double spot = Double.Parse(textBox1.Text);
                double Strike = Double.Parse(textBox2.Text);
                double expiry = Double.Parse(textBox3.Text);
                double interest = Double.Parse(textBox4.Text);
                double vol = Double.Parse(textBox5.Text);

                double mean = Math.Pow((1.0 + interest), (1.0 / 252)) - 1.0;
                double stdDev = vol / Math.Sqrt(252);

                PricerProj.MCGenerator monty = new PricerProj.MCGenerator(mean, stdDev, 1.0);

                double[][] results = monty.generatePathsHist(spot, numOfPaths, expiry);

                plotData(results);
            }
            catch
            {
                MessageBox.Show("Please check all inputs are entered");
            }
        }

        private void plotData(double[][] results)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisY.Minimum = Double.Parse(textBox1.Text) - 2;
            chart1.ChartAreas[0].AxisY.Maximum = Double.Parse(textBox1.Text) + 2;

            var indices = Enumerable.Range(0, results[0].GetLength(0) - 1);
            int count = 0;
            foreach (double[] line in results)
            {
                var name = "Series" + count++;

                chart1.Series.Add(name);
                chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                foreach (int i in indices)
                {
                    chart1.Series[name].Points.AddXY(i, line[i]);
                }
            }
        }
    }
}
