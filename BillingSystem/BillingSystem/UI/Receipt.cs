using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;

namespace BillingSystem.UI
{
    public partial class Receipt : Form
    {
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public Receipt()
        {
            InitializeComponent();

            
        }

        private void Receipt_Load(object sender, EventArgs e)
        {

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label14.Visible = false;

            error();

            label11.Visible = false;
            label17.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label10.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            btnGenerate.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label24.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            textBox5.Visible = false;
            textBox4.Visible = false;
            crystalReportViewer1.Visible = false;
            crystalReportViewer2.Visible = false;

            label13.Text = DateTime.Now.ToLongDateString();

            label1.Text = frmPurchaseAndSales.subtotal;
            label2.Text = frmPurchaseAndSales.discount;
            label3.Text = frmPurchaseAndSales.cgst;
            label4.Text = frmPurchaseAndSales.igst;
            label5.Text = frmPurchaseAndSales.sgst;
            label6.Text = frmPurchaseAndSales.cgstamount;
            label7.Text = frmPurchaseAndSales.igstamount;
            label8.Text = frmPurchaseAndSales.sgstamount;
            label9.Text = frmPurchaseAndSales.grandtotal;
            label14.Text = frmPurchaseAndSales.discountamount;
            label17.Text = frmPurchaseAndSales.pvandsr;

            if (label17.Text == "Payment Voucher")
            {
                label11.Visible = false;
                label17.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label21.Visible = false;
                label24.Visible = false;
                label22.Visible = false;
                label23.Visible = false;
                textBox5.Visible = false;
                textBox4.Visible = false;
                label10.Visible = true;
                label15.Visible = true;
                label16.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                btnGenerate.Visible = true;
                crystalReportViewer1.Visible = true;
                crystalReportViewer2.Visible = false;
            }
            else if (label17.Text == "Sales Receipt")
            {
                label11.Visible = false;
                label17.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label10.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                label21.Visible = false;
                label24.Visible = false;
                label23.Visible = true;
                label22.Visible = true;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox5.Visible = true;
                textBox4.Visible = true;
                btnGenerate.Visible = true;
                crystalReportViewer1.Visible = false;
                crystalReportViewer2.Visible = true;
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"E:\Important\Project Documentation\Inventory Management Documentation\BillingSystem\BillingSystem\UI\XMLFile1.xml");
            doc.DocumentElement.RemoveAll();
            doc.Save(@"E:\Important\Project Documentation\Inventory Management Documentation\BillingSystem\BillingSystem\UI\XMLFile1.xml");

            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
           if(label17.Text == "Payment Voucher")
           {
               label12.Text = label18.Text + label19.Text + label20.Text;
               label11.Text = NumberToWords(Convert.ToInt32(textBox1.Text)) + " Only";

               //Page Header Section

               CrystalReport1 cr = new CrystalReport1();
               TextObject VoucherNo = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["Text5"];
               VoucherNo.Text = label12.Text;
               crystalReportViewer1.ReportSource = cr;

               TextObject Date = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["Text6"];
               Date.Text = label13.Text;
               crystalReportViewer1.ReportSource = cr;

               //Details Section

               TextObject sumofrupees = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text9"];
               sumofrupees.Text = textBox1.Text;
               crystalReportViewer1.ReportSource = cr;

               TextObject inwords = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text11"];
               inwords.Text = label11.Text;
               crystalReportViewer1.ReportSource = cr;

               TextObject paidto = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text13"];
               paidto.Text = textBox2.Text;
               crystalReportViewer1.ReportSource = cr;

               TextObject onbehalfof = (TextObject)cr.ReportDefinition.Sections["Section3"].ReportObjects["Text16"];
               onbehalfof.Text = textBox3.Text;
               crystalReportViewer1.ReportSource = cr;
           }
           else if(label17.Text == "Sales Receipt")
           {
               label12.Text = label21.Text + label24.Text;

               ReportDocument cryRpt = new ReportDocument();
               cryRpt.Load(@"E:\Important\Project Documentation\Inventory Management Documentation\BillingSystem\BillingSystem\UI\CrystalReport2.rpt");
               crystalReportViewer2.ReportSource = cryRpt;
               crystalReportViewer2.Refresh();

               CrystalReport2 cr = new CrystalReport2();

               //Report Header Section

               TextObject billno = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text22"];
               billno.Text = label12.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject date = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text23"];
               date.Text = label13.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject name = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text9"];
               name.Text = textBox5.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject address = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text6"];
               address.Text = textBox4.Text;
               crystalReportViewer2.ReportSource = cr;

               //Report Footer Section

               TextObject subtotal = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text10"];
               subtotal.Text = label1.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject discount = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text31"];
               discount.Text = label2.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject discountamount = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text12"];
               discountamount.Text = label14.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject cgstamount = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text13"];
               cgstamount.Text = label6.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject sgstamount = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text14"];
               sgstamount.Text = label8.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject igstamount = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text15"];
               igstamount.Text = label7.Text;
               crystalReportViewer2.ReportSource = cr;

               TextObject grandtotal = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text21"];
               grandtotal.Text = label9.Text;
               crystalReportViewer2.ReportSource = cr;

               clear();
           }

           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        


        private void label9_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label17_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label18.Text = textBox1.Text.Length.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label19.Text = textBox2.Text.Length.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label20.Text = textBox3.Text.Length.ToString();
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label21.Text = textBox5.Text.Length.ToString();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label24.Text = textBox4.Text.Length.ToString();
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                errorProvider1.SetError(textBox1, "Please Enter the Sum of Amount");
            }
            else
            {
                errorProvider1.SetError(textBox1, "");
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text == string.Empty)
            {
                errorProvider1.SetError(textBox2, "Please Enter the name of the vendor");
            }
            else
            {
                errorProvider1.SetError(textBox2, "");
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                errorProvider1.SetError(textBox3, "Please Enter the On behalf of");
            }
            else
            {
                errorProvider1.SetError(textBox3, "");
            }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text == string.Empty)
            {
                errorProvider1.SetError(textBox5, "Please Enter the Name of the Customer");
            }
            else
            {
                errorProvider1.SetError(textBox5, "");
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text == string.Empty)
            {
                errorProvider1.SetError(textBox4, "Please Enter the Delievery Address of the Customer");
            }
            else
            {
                errorProvider1.SetError(textBox4, "");
            }
        }

        private void error()
        {
            errorProvider1.SetError(textBox1, "Please Enter the Sum of Amount");
            errorProvider1.SetError(textBox2, "Please Enter the name of the vendor");
            errorProvider1.SetError(textBox3, "Please Enter the On behalf of");
            errorProvider1.SetError(textBox5, "Please Enter the Name of the Customer");
            errorProvider1.SetError(textBox4, "Please Enter the Delievery Address of the Customer");
        }

        private void repeaterrorprovider()
        {
            errorProvider1.SetError(textBox1, "");
            errorProvider1.SetError(textBox2, "");
            errorProvider1.SetError(textBox3, "");
            errorProvider1.SetError(textBox5, "");
            errorProvider1.SetError(textBox4, "");
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            repeaterrorprovider();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
