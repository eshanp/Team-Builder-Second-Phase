using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Assignment5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Declaring global variables
        decimal CalculateEvent, CalculateLocation, CalculateMeal, TotalFinalPrice;
        int days, Randomnumber, TempCount = 0,TempCount2=0;
        int[,] result = new int[10, 10];
        //Storing the date and time format in constant.
        private const string DATEFORMAT = "dd-MM-yyyy", TIMEFORMAT = "hh-mm-ss";
        int[] NoOfDays = { 2, 3, 4, 6, 2, 5, 3, 2, 6, 5 };
        string[] EventName = {"Murder Mystery Weekend","CSI Weekend","The Great Outdoors",
                               "The Chase","Digital Refresh","Action Photography","Team Ryder Cup","Abseiling",
                               "War Games","Find Wally"},
                 LocationName = { "Cork", "Dublin", "Galway", "Belmullet", "Belfast" };
        decimal[] RegistrationFees = { 600m, 1000m, 1500m, 1800m, 599m, 999m, 619m, 499m, 1999m, 799m },
                  LocationFee = { 250m, 165m, 225m, 305m, 95m };

        decimal[,] MealPrice =
        {
            {99m, 75m, 24m, 0m },
            {149m, 113m, 36m, 0m },
            {198m, 150m, 48m, 0m },
            {297m, 225m, 72m, 0m },
            {99m, 75m, 25m, 0m },
            {248m, 188m, 60m, 0m },
            {149m, 113m, 36m, 0m },
            {99m, 75m, 24m, 0m },
            {297m, 225m, 72m, 0m },
            {248m, 188m, 60m, 0m }
        };
        int[,] LocationAvail =
        {
            {35, 67, 12, 77, 32 },
            {28, 3, 34, 12, 7 },
            {23, 2, 6, 4, 3 },
            {12, 6, 9, 4, 6 },
            {2, 0, 7, 5, 8 },
            {1, 8, 4, 7, 4 },
            {16, 24, 40, 4, 12 },
            {3, 7, 45, 3, 3 },
            {45, 12, 56, 12, 23 },
            {0, 0, 3, 0, 0 }
        };
        List<int> TransactionList = new List<int>();
        List<int> TotalSeatList = new List<int>();
        List<string> EventNameList = new List<string>();
        List<string> LocationNameList = new List<string>();
        List<string> MealPlanNameList = new List<string>();
        List<decimal> TotalAmountList = new List<decimal>();
        //Radio buttons selection when clicked, changing according to the meal plan selected.
        private void FullMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateMeal = MealPrice[(EventSelectionListView.SelectedItems[0].Index), 0];
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            MealPlanDisplayLabel.Text = FullMealRadioButton.Text;
        }

        private void HalfMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateMeal = MealPrice[(EventSelectionListView.SelectedItems[0].Index), 1];
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            MealPlanDisplayLabel.Text = HalfMealRadioButton.Text;
        }

        private void BreakfastMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateMeal = MealPrice[(EventSelectionListView.SelectedItems[0].Index), 2];
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            MealPlanDisplayLabel.Text = BreakfastMealRadioButton.Text;
        }

        private void NoMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateMeal = MealPrice[(EventSelectionListView.SelectedItems[0].Index), 3];
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            MealPlanDisplayLabel.Text = NoMealRadioButton.Text;
        }
        //Stores the total available places for all the events when requested or button is clicked.
        private void ManagementReportButton_Click(object sender, EventArgs e)
        {
            ReadAndStoreData();
            StreamWriter Report;
            if (File.Exists("ManagementReport"))
                ManagementWrite();
            else
            {
                Report = File.CreateText("ManagementReport");
                Report.Close();
                ManagementWrite();
            }
            MessageBox.Show("Management Report file has been created and stored", "Management Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ManagementWrite()
        {
            StreamWriter Report;
            File.WriteAllText("ManagementReport", string.Empty);
            Report = File.AppendText("ManagementReport");
            Report.WriteLine("PLACES AVAIL" + "\t\t\t\t" + LocationName[0] + "\t\t" + LocationName[1] + "\t\t" + LocationName[2] + "\t\t" + LocationName[3] + "\t\t" + LocationName[4]);
            Report.WriteLine(EventName[0] + "\t\t" + result[0, 0] + "\t\t\t" + result[0, 1] + "\t\t\t" + result[0, 2] + "\t\t\t\t" + result[0, 3] + "\t\t\t" + result[0, 4]);
            Report.WriteLine(EventName[1] + "\t\t\t\t\t" + result[1, 0] + "\t\t\t" + result[1, 1] + "\t\t\t" + result[1, 2] + "\t\t\t\t" + result[1, 3] + "\t\t\t" + result[1, 4]);
            Report.WriteLine(EventName[2] + "\t\t\t" + result[2, 0] + "\t\t\t" + result[2, 1] + "\t\t\t" + result[2, 2] + "\t\t\t\t" + result[2, 3] + "\t\t\t" + result[2, 4]);
            Report.WriteLine(EventName[3] + "\t\t\t\t\t" + result[3, 0] + "\t\t\t" + result[3, 1] + "\t\t\t" + result[3, 2]+ "\t\t\t\t" + result[3, 3] + "\t\t\t" + result[3, 4]);
            Report.WriteLine(EventName[4] + "\t\t\t\t" + result[4, 0] + "\t\t\t" + result[4, 1] + "\t\t\t" + result[4, 2] + "\t\t\t\t" + result[4, 3] + "\t\t\t" + result[4, 4]);
            Report.WriteLine(EventName[5] + "\t\t\t" + result[5, 0] + "\t\t\t" + result[5, 1] + "\t\t\t" + result[5, 2] + "\t\t\t\t" + result[5, 3] + "\t\t\t" + result[5, 4]);
            Report.WriteLine(EventName[6] + "\t\t\t\t" + result[6, 0] + "\t\t\t" + result[6, 1] + "\t\t\t" + result[6, 2] + "\t\t\t\t" + result[6, 3] + "\t\t\t" + result[6, 4]);
            Report.WriteLine(EventName[7] + "\t\t\t\t\t" + result[7, 0] + "\t\t\t" + result[7, 1] + "\t\t\t" + result[7, 2] + "\t\t\t\t" + result[7, 3] + "\t\t\t" + result[7, 4]);
            Report.WriteLine(EventName[8] + "\t\t\t\t\t" + result[8, 0] + "\t\t\t" + result[8, 1] + "\t\t\t" + result[8, 2] + "\t\t\t\t" + result[8, 3] + "\t\t\t" + result[8, 4]);
            Report.WriteLine(EventName[9] + "\t\t\t\t\t" + result[9, 0] + "\t\t\t" + result[9, 1] + "\t\t\t" + result[9, 2] + "\t\t\t\t" + result[9, 3] + "\t\t\t" + result[9, 4]);
            Report.WriteLine("\n\nDATE:" + DateTime.Now.ToString(DATEFORMAT) + "\tTIME:" + DateTime.Now.ToString(TIMEFORMAT));
            Report.Close();
        }
        //Writes and stores the total seats available for events for further bookings.
        private void WriteAndStoreData()
        {
            StreamWriter WriteData;
            //Clears the file to store a whole new data.
            using (var fs = new FileStream("AvailPlaces", FileMode.Truncate))
            {
                fs.Close();
            }
            WriteData = File.AppendText("AvailPlaces");

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    WriteData.WriteLine(result[i, j].ToString());

                }
            }
            WriteData.Close();

        }
        //Reads the data from the file for seats availibility and stores it ina global array for further use.
        private void ReadAndStoreData()
        {
            StreamReader ReadData;
            ReadData = File.OpenText("AvailPlaces");

            for (int i = 0; i < 10; i++)
            {

                for (int j = 0; j < 5; j++)
                {
                    string temp = ReadData.ReadLine();
                    result[i, j] = int.Parse(temp);
                }
            }
            ReadData.Close();
        }
        //Sets the application to default for next transaction. 
        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (TempCount == 1)
                TempCount = 0;
            CompletedOrderDetailsGroupBox.Visible = false;
            DisplayGroupBox.Visible = false;
            EventSelectionListView.Items[0].Selected = false;
            TransactionList.Clear();
            EventNameList.Clear();
            LocationNameList.Clear();
            MealPlanNameList.Clear();
            TotalAmountList.Clear();
            TotalSeatList.Clear();
            CalculateEvent = 0;
            CalculateLocation = 0;
            CalculateMeal = 0;
            LocationSelectionListBox.SelectedIndex = -1;
            LocationSelectionListBox.Enabled = false;
            EventDisplayLabel.Text = string.Empty;
            SeatsRequiredTextBox.Text = 1.ToString();
            FullMealRadioButton.Checked = false;
            HalfMealRadioButton.Checked = false;
            BreakfastMealRadioButton.Checked = false;
            NoMealRadioButton.Checked = false;
            LocationDisplayLabel.Text = string.Empty;
            MealPlanDisplayLabel.Text = string.Empty;
            BookingAmountDisplayLabel.Text = "";

        }
        //this meathod adds the details selected by the user into the list. Makes the application ready for another booking or completion of the order.
        private void AddBookingButton_Click(object sender, EventArgs e)
        {
            if (File.Exists("AvailPlaces") && TempCount == 0)
            {
                ReadAndStoreData();
                //Changes the value to 1 to stop it from reading the old values from the file until the application closes or writes data.
                TempCount = 1;
            }
            if (EventSelectionListView.SelectedItems.Count > 0 && LocationSelectionListBox.SelectedIndex != -1)
            {
                try
                {
                    if (int.Parse(SeatsRequiredTextBox.Text) <= result[EventSelectionListView.SelectedItems[0].Index, LocationSelectionListBox.SelectedIndex])
                    {   //Adding the details in the list.
                        int Tempy = int.Parse(SeatsRequiredTextBox.Text);
                        int Tempx = result[EventSelectionListView.SelectedItems[0].Index, LocationSelectionListBox.SelectedIndex];
                        result[EventSelectionListView.SelectedItems[0].Index, LocationSelectionListBox.SelectedIndex] = Tempx - Tempy;
                        var Random = new Random();
                        Randomnumber = Random.Next(9999, 99999);
                        TransactionList.Add(Randomnumber);
                        TotalSeatList.Add(int.Parse(SeatsRequiredTextBox.Text));
                        EventNameList.Add(EventDisplayLabel.Text);
                        LocationNameList.Add(LocationDisplayLabel.Text);
                        MealPlanNameList.Add(MealPlanDisplayLabel.Text);
                        TotalAmountList.Add(decimal.Parse(BookingAmountDisplayLabel.Text));
                        MessageBox.Show("Event:" + EventDisplayLabel.Text + "\nLocation:" + LocationDisplayLabel.Text + "\nMeal Plan:" + MealPlanDisplayLabel.Text + "\nTotal Amount:" + BookingAmountDisplayLabel.Text, "Booking has been added to list.Please select complete order to complete the transaction.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CompleteOrderButton.Enabled = true;
                        CompletedOrderDetailsGroupBox.Visible = false;
                        DisplayGroupBox.Visible = false;
                        EventSelectionListView.Items[0].Selected = false;
                        LocationSelectionListBox.SelectedIndex = -1;
                        LocationSelectionListBox.Enabled = false;
                        EventDisplayLabel.Text = string.Empty;
                        SeatsRequiredTextBox.Text = 1.ToString();
                        FullMealRadioButton.Checked = false;
                        HalfMealRadioButton.Checked = false;
                        BreakfastMealRadioButton.Checked = false;
                        NoMealRadioButton.Checked = false;
                        LocationDisplayLabel.Text = string.Empty;
                        MealPlanDisplayLabel.Text = string.Empty;
                        BookingAmountDisplayLabel.Text = "";
                  
                    }
                        else
                    {
                        MessageBox.Show("Sorry, we are unable to add your booking. Only these many seats are available: " + result[EventSelectionListView.SelectedItems[0].Index, LocationSelectionListBox.SelectedIndex].ToString(), "Seats Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SeatsRequiredTextBox.Text = result[EventSelectionListView.SelectedItems[0].Index, LocationSelectionListBox.SelectedIndex].ToString();
                        SeatsRequiredTextBox.SelectAll();
                        SeatsRequiredTextBox.Focus();
                    }

                }
                catch
                {
                    MessageBox.Show("Enter number of seats required", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please Select Event/Location", "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //exits the application after writing the transaction details for that particular date
        private void ExitButton_Click(object sender, EventArgs e)
        {
            FileDetailsWrite();
            Application.Exit();
        }
        //Writes the data into the file.
        private void FileDetailsWrite()
        {
            StreamWriter WriteData;

            string MyFileName = String.Format(DateTime.Now.ToString(DATEFORMAT), "MyFileName");
            if (File.Exists(MyFileName))
            {   //checks if the file exists for that day and writes the data
                if (MyFileName != DateTime.Now.ToString(DATEFORMAT))
                {
                    string NewFile = DateTime.Now.ToString(DATEFORMAT);
                    WriteData = File.CreateText(NewFile);
                    WriteData.Close();
                    WriteData = File.AppendText(NewFile);
                    for (int i = 0; i < EventNameList.Count; i++)
                    {
                        //both the if and else according to the condition writes all the multiple bookings made into file 
                        WriteData.WriteLine(TransactionList[i].ToString());
                        WriteData.WriteLine(TotalSeatList[i].ToString());
                        WriteData.WriteLine(EventNameList[i]);
                        WriteData.WriteLine(LocationNameList[i]);
                        WriteData.WriteLine(MealPlanNameList[i]);
                        WriteData.WriteLine(TotalAmountList[i].ToString());
                        WriteData.WriteLine("                    ");
                    }
                    WriteData.Close();
                }
                else
                {
                    WriteData = File.AppendText(MyFileName);
                    for (int i = 0; i < EventNameList.Count; i++)
                    {
                        WriteData.WriteLine(TransactionList[i].ToString());
                        WriteData.WriteLine(TotalSeatList[i].ToString());
                        WriteData.WriteLine(EventNameList[i]);
                        WriteData.WriteLine(LocationNameList[i]);
                        WriteData.WriteLine(MealPlanNameList[i]);
                        WriteData.WriteLine(TotalAmountList[i].ToString());
                        WriteData.WriteLine("                    ");
                    }
                    WriteData.Close();
                }
            }
            else
            {
                WriteData = File.CreateText(MyFileName);
                WriteData.Close();
                WriteData = File.AppendText(MyFileName);

                for (int i = 0; i < EventNameList.Count; i++)
                {
                    WriteData.WriteLine(TransactionList[i].ToString());
                    WriteData.WriteLine(TotalSeatList[i].ToString());
                    WriteData.WriteLine(EventNameList[i]);
                    WriteData.WriteLine(LocationNameList[i]);
                    WriteData.WriteLine(MealPlanNameList[i]);
                    WriteData.WriteLine(TotalAmountList[i].ToString());
                    WriteData.WriteLine("                    ");
                }
                WriteData.Close();
            }
        }
        //Changes the total price of that particular booking according to the number of seats. 
        private void SeatsRequiredTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SeatsRequiredTextBox.Text == null || SeatsRequiredTextBox.Text != null)
                {
                    int.Parse(SeatsRequiredTextBox.Text);

                    BookingAmountDisplayLabel.Text = ((CalculateEvent + CalculateLocation + CalculateMeal) * int.Parse(SeatsRequiredTextBox.Text)).ToString();
                }
            }
            catch
            {
                MessageBox.Show("Enter a Valid Number");
            }
        }
        //Writes all the details present in the list into the file by calling the method.
        private void CompleteOrderButton_Click(object sender, EventArgs e)
        {
            //Stores data for places availibility
            WriteAndStoreData();
            TempCount = 0;
            DisplayListView.Items.Clear();

            for (int i = 0; i < LocationNameList.Count; i++)
            {
                //Displays all the items for that transaction in a list view.
                ListViewItem lst1 = new ListViewItem(Randomnumber.ToString());
                lst1.SubItems.Add(TotalSeatList[i].ToString());
                lst1.SubItems.Add(EventNameList[i]);
                lst1.SubItems.Add(LocationNameList[i]);
                lst1.SubItems.Add(MealPlanNameList[i]);
                lst1.SubItems.Add(TotalAmountList[i].ToString());
                DisplayListView.Items.Add(lst1);
                TotalFinalPrice += TotalAmountList[i];

            }
            TotalPriceDisplayLabel.Text = TotalFinalPrice.ToString();
            TotalFinalPrice = 0;
            //stores details in the file
            FileDetailsWrite();
            //Visibility is handled
            CompletedOrderDetailsGroupBox.Visible = true;
            CompleteOrderButton.Enabled = false;
            TransactionList.Clear();
            EventNameList.Clear();
            LocationNameList.Clear();
            MealPlanNameList.Clear();
            TotalAmountList.Clear();
            TotalSeatList.Clear();
        }
        //Changes the display view on dynamic selection on mouse clicks for both the meathods. 
        private void EventSelectionListView_MouseClick(object sender, MouseEventArgs e)
        {
            
            ClearButton.Enabled = true;
            CompletedOrderDetailsGroupBox.Visible = false;
            DisplayGroupBox.Visible = true;
            CalculateEvent = RegistrationFees[EventSelectionListView.SelectedItems[0].Index];
            days = NoOfDays[(EventSelectionListView.SelectedItems[0].Index)];
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            EventDisplayLabel.Text = (EventName[EventSelectionListView.SelectedItems[0].Index]).ToString();
            LocationSelectionListBox.Enabled = true;
        }

        private void LocationSelectionListBox_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateLocation = (LocationFee[LocationSelectionListBox.SelectedIndex]) * decimal.Parse(days.ToString());
            BookingAmountDisplayLabel.Text = (CalculateEvent + CalculateLocation + CalculateMeal).ToString();
            LocationDisplayLabel.Text = (LocationName[LocationSelectionListBox.SelectedIndex]).ToString();
            MealPlanGroupBox.Enabled = true;
        }
    }
}
