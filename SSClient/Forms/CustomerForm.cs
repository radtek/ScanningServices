using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using static ScanningServicesAdmin.Data.GlovalVariables;

namespace ScanningServicesAdmin.Forms
{
    public partial class CustomerForm : Form
    {

        //public string BaseURL = "http://localhost:47063" + "/api/";

        public CustomerForm()
        {
            InitializeComponent();
            newCustomersList.Clear();
            if (Data.GlovalVariables.transactionType == "Update")
            {
                CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            }
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }
    
        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (Data.GlovalVariables.transactionType == "New")
            {
                CustomerNameTextBox.Text = "";
            }
            else
            {
                CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyHutton_Click(object sender, EventArgs e)
        {
            Save("Save");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save("SaveAndExit");
        }

        private void Save(string action)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string customerJS = "";
            string returnMessage = "";
            Customer customer = new Customer();
            ResultCustomers resultCustomers = new ResultCustomers();
            customer.CustomerName = CustomerNameTextBox.Text;

            switch (Data.GlovalVariables.transactionType)
            {
                case "New":
                    
                    customerJS = JsonConvert.SerializeObject(customer, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "Customers/NewCustomer";
                    bodyString = "'" + customerJS + "'";

                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    //ResultCustomers resultCustomers = new ResultCustomers();
                    using (HttpContent content = response_for_new.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Reformating the result string
                        //returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                        //returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                        resultCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);
                    }

                    if (response_for_new.IsSuccessStatusCode)
                    {
                        // Set the value of the new customer to a gloval variable
                        if (resultCustomers.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:"  +"\r\n" + resultCustomers.Message.Replace(". ", "\r\n"), "New Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.newCustomersList.Add(CustomerNameTextBox.Text);
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                CustomerNameTextBox.Text = "";
                                CustomerNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultCustomers.Message.Replace(". ", "\r\n") +  resultCustomers.Message, "New Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case "Update":
                    customer.CustomerID = Data.GlovalVariables.currentCustomerID;
                    customerJS = JsonConvert.SerializeObject(customer, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "Customers/UpdateCustomer";
                    bodyString = "'" + customerJS + "'";

                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;
                                        
                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Reformating the result string
                        //returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                        //returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                        resultCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        // Set the value of the new customer to a gloval variable
                        if (resultCustomers.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultCustomers.Message.Replace(". ","\r\n"), "Update Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.currentCustomerName = CustomerNameTextBox.Text;
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                CustomerNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:"  + "\r\n" + resultCustomers.Message.Replace(". ", "\r\n"), "Update Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

            }

        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
