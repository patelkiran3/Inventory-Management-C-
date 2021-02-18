using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BillingSystem.DAL;
using BillingSystem.BLL;
using System.Transactions;
using System.Xml;

namespace BillingSystem.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public static string subtotal;
        public static string discount;
        public static string cgst;
        public static string igst;
        public static string sgst;
        public static string cgstamount;
        public static string igstamount;
        public static string sgstamount;
        public static string grandtotal;
        public static string discountamount;
        public static string pvandsr;

        public frmPurchaseAndSales()
        {
            InitializeComponent();

            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            error();
        }

        DataTable table = new DataTable("Receipt");

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();
        DataTable transactionDT = new DataTable();

        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            label17.Visible = false;
            dataGridView1.Visible = false;

            //Get the transactionType value from user dashboard.
            string type = UserDashboard.transactionType;
            
            //Set the value on lblTop
            lblTop.Text = type;
            
            

            //Specify Columns for Transaction DataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Qty");
            transactionDT.Columns.Add("Total");

            table.Columns.Add("ProductName", typeof(String));
            table.Columns.Add("Rate", typeof(decimal));
            table.Columns.Add("Qty", typeof(int));
            table.Columns.Add("Total", typeof(String));

            dataGridView1.DataSource = table;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keyword from TextBox
            string keywords = txtSearch.Text;

            if (keywords == "")
            {
                //Clear all the TextBoxs
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text= "";
                txtAddress.Text = "";
                return;
            }

            //Write the code to get the details and set the value on textboxes
            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keywords);

            //Set the value from DeaCustBLL to TextBoxs
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;


        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywords from productsSearch textBox
            string keywords = txtSearchProduct.Text;

            if (keywords == "")
            {
                txtProductName.Text="";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }

            //Search the product and display on repective textboxs
            productsBLL p = pDAL.GetProductsForTransaction(keywords);

            //Set the values on textboxs based on p object
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region Throw Error Message

            if (txtSearchProduct.Text == "" && txtQty.Text == "")
                MessageBox.Show(" Please Enter Product Name Or Id And Enter Qty.... !! ");

            else if (txtSearchProduct.Text == "")
                MessageBox.Show(" Please Enter Product Id Or Product Name ");

            else if (txtQty.Text == "")
                MessageBox.Show(" Please Enter Qty .... !! ");

            #endregion

            else
            {
                //Get Product Name, Rate, Quantity customer wants to buy
                string productName = txtProductName.Text;
                decimal Rate = decimal.Parse(txtRate.Text);
                decimal Qty = decimal.Parse(txtQty.Text);

                decimal rtotal = Rate * Qty;
                label17.Text = rtotal.ToString();

                table.Rows.Add(txtProductName.Text, txtRate.Text, txtQty.Text, label17.Text);
                dataGridView1.DataSource = table;

                decimal Total = Rate * Qty; //Total = Rate x Qty

                //Display the SubTotal in TextBox
                //Get the Total Value from TextBox
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                subTotal = subTotal + Total;

                //Check whether the product is selected or not
                if (productName == "")
                {
                    MessageBox.Show("Select the Product First. Try Again.");
                }
                else
                {
                    transactionDT.Rows.Add(productName, Rate, Qty, Total);

                    //Display in Data Grid View
                    dgvAddedProducts.DataSource = transactionDT;

                    //Display SubTotal in TextBox
                    txtSubTotal.Text = subTotal.ToString();

                    //Clear the TextBoxs
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "";
                    txtRate.Text = "";
                    txtQty.Text = "";

                }

                if (txtQty.Text == "" || txtQty.Text == "0.00" || txtQty.Text == "0")
                {
                    errorProvider1.SetError(txtSearchProduct, "Please Enter the PRODUCT ID");
                    errorProvider1.SetError(txtQty, "Please Enter the Quantity");
                }
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //Get the value from Discount TextBox
            string value = txtDiscount.Text;
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            //Check if the Grandtotal has value or not if doesn`t have value then calculate the discount first
            string check = txtGrandTotal.Text;

            if (check == "")
            {
                MessageBox.Show("Calculate the Discount and Set the GrandTotal First.");
            } 
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            txtReturnAmount.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                
                //Get the Paid Amount and GrandTotal
                decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
                decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

                
                
                decimal returnAmount = paidAmount - grandTotal;

                //Display the Return Amount
                
                
                txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get the values from purchase sales from first
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //Get the id of Dealer and Customer
            //Lets get the name of the dealer and customer first
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.ig = decimal.Parse(txtig.Text);
            transaction.cg = decimal.Parse(txtcg.Text);
            transaction.sg = decimal.Parse(txtsg.Text);
            transaction.igamount = decimal.Parse(textigstamount.Text);
            transaction.cgamount = decimal.Parse(textcgstamount.Text);
            transaction.sgamount = decimal.Parse(textsgstamount.Text);
            transaction.discount = decimal.Parse(txtDiscount.Text);
            transaction.discountamount = decimal.Parse(textdiscoutamount.Text);

            //Get the username of logged in user
            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            //Create a Boolean Variable and set its value to false
            bool success = false;

            //Actual code to Insert Transaction and transactionDetails
            using (TransactionScope scope = new TransactionScope())
            {
                int tranactionID = -1;

                //Create a Boolean Value and Insert Transaction
                bool w = tDAL.Insert_Transaction(transaction, out tranactionID);

                //Use for loop to insert transaction details
                for (int i = 0; i < transactionDT.Rows.Count; i++)
                {
                    //Get all the details of the product
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();

                    //Get the product name and convert it to ID
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //Increase or Decrease Product Quantity Based on Purchase or Sales
                    string transactionType = lblTop.Text;

                    //Check Whetehr we are on purchase or sales
                    bool x = false;
                    if (transactionType == "Purchase")
                    {
                        //Increase the Product
                        x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    else if (transactionType == "Sales")
                    {
                        //Decrease Product Quantity
                        x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    //Insert TransactionDetails inside our database
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }

                if (success == true)
                {
                    //Transaction Complete
                    scope.Complete();

                    label6.Text = txtSubTotal.Text;
                    label7.Text = txtDiscount.Text;
                    label8.Text = txtcg.Text;
                    label9.Text = txtig.Text;
                    label10.Text = txtsg.Text;
                    label11.Text = textcgstamount.Text;
                    label12.Text = textigstamount.Text;
                    label13.Text = textsgstamount.Text;
                    label14.Text = txtGrandTotal.Text;
                    label15.Text = textdiscoutamount.Text;

                    MessageBox.Show("Transaction Completed Successfully.");
                    comboBox1.Visible = true;


                    //Clear the data Grid View and all the TextBxs
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "";
                    txtRate.Text = "";
                    txtQty.Text = "";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtig.Text = "0";
                    txtcg.Text = "0";
                    txtsg.Text = "0";
                    textigstamount.Text = "0";
                    textsgstamount.Text = "0";
                    textcgstamount.Text = "0";
                    textdiscoutamount.Text = "0";
                    txtGrandTotal.Text = "0";
                    txtPaidAmount.Text = "";
                }
                else
                {
                    MessageBox.Show("Transaction Failed.");
                }
            }
        }

        private void btnVAT_Click(object sender, EventArgs e)
        {
            // IGST AMOUNT CALCULATION
            double totalamount, taxamount, subtotal, percentage;
            subtotal = Convert.ToDouble(txtSubTotal.Text);
            percentage = Convert.ToDouble(txtig.Text);
            totalamount = (subtotal * 100) / (100 + percentage);
            taxamount = totalamount * (percentage / 100);
            textigstamount.Text = Convert.ToString(Math.Round(taxamount, 2));
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {

            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            decimal discount = decimal.Parse(txtDiscount.Text);

            //Calculate the GrandTotal based on Discount
            decimal grandTotal = ((100 - discount) / 100) * subTotal;

            //Display the GrandTotal in TextBox
            txtGrandTotal.Text = grandTotal.ToString();

            // DISCOUNT CALCULATION
            double subtotal = Convert.ToDouble(txtSubTotal.Text.Trim());
            double Discount = Convert.ToDouble(txtDiscount.Text.Trim());

            double discountprice = (subtotal * Discount) / 100;
            textdiscoutamount.Text = discountprice.ToString();

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // SGST AMOUNT CALCULATION
            double totalamount, taxamount, subtotal, percentage;
            subtotal = Convert.ToDouble(txtSubTotal.Text);
            percentage = Convert.ToDouble(txtsg.Text);
            totalamount = (subtotal * 100) / (100 + percentage);
            taxamount = totalamount * (percentage / 100);
            textsgstamount.Text = Convert.ToString(Math.Round(taxamount, 2));
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            // CGST AMOUNT CALCULATION
            double totalamount, taxamount, subtotal, percentage;
            subtotal = Convert.ToDouble(txtSubTotal.Text);
            percentage = Convert.ToDouble(txtcg.Text);
            totalamount = (subtotal * 100) / (100 + percentage);
            taxamount = totalamount * (percentage / 100);
            textcgstamount.Text = Convert.ToString(Math.Round(taxamount, 2));
        }

        private void txtSubTotal_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            decimal igstamount = decimal.Parse(textigstamount.Text);
            decimal cgstamount = decimal.Parse(textcgstamount.Text);
            decimal sgstamount = decimal.Parse(textsgstamount.Text);
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);

            decimal gt = grandTotal + igstamount + cgstamount + sgstamount;


            txtGrandTotal.Text = gt.ToString();
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSearch_Validating(object sender, CancelEventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                errorProvider1.SetError(txtSearch, "Please Enter the suitable word");
            }
            else
            {
                errorProvider1.SetError(txtSearch, "");
            }
        }

        private void txtSearchProduct_Validating(object sender, CancelEventArgs e)
        {
            if (txtSearchProduct.Text == string.Empty)
            {
                errorProvider1.SetError(txtSearchProduct, "Please Enter the PRODUCT ID");
            }
            else
            {
                errorProvider1.SetError(txtSearchProduct, "");
            }
        }

        private void txtSubTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtcg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtig_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtGrandTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPaidAmount_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtPaidAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtReturnAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtQty_Validating(object sender, CancelEventArgs e)
        {
            if (txtQty.Text == string.Empty)
            {
                errorProvider1.SetError(txtQty, "Please Enter the Quantity");
            }
            else
            {
                errorProvider1.SetError(txtQty, "");
            }
        }


        private void error()
        {
            errorProvider1.SetError(txtSearch, "Please Enter the suitable word"); 
            errorProvider1.SetError(txtSearchProduct, "Please Enter the PRODUCT ID");
            errorProvider1.SetError(txtQty, "Please Enter the Quantity");
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            
        }

        private void txtReceipt_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtInventory_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pvandsr = comboBox1.Text;

            if (comboBox1.SelectedItem == "Payment Voucher")
            {

                Receipt r = new Receipt();
                r.Show();
                this.Hide();
            }

            else if (comboBox1.SelectedItem == "Sales Receipt")
            {
                table.WriteXml(@"E:\Important\Project Documentation\Inventory Management Documentation\BillingSystem\BillingSystem\UI\XMLFile1.xml", XmlWriteMode.WriteSchema);
                MessageBox.Show("Data Exported");


                subtotal = label6.Text;
                discount = label7.Text;
                cgst = label8.Text;
                igst = label9.Text;
                sgst = label10.Text;
                cgstamount = label11.Text;
                igstamount = label12.Text;
                sgstamount = label13.Text;
                grandtotal = label14.Text;
                discountamount = label15.Text;

                

                Receipt r = new Receipt();
                r.Show();
                this.Hide();
            }
        }

        private void pnlCalculation_Paint(object sender, PaintEventArgs e)
        {
        }

        private void txtReceipt_Click_1(object sender, EventArgs e)
        {
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label18_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void pnlDeaCust_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDeaCustTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblEmail_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblContact_Click(object sender, EventArgs e)
        {

        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblAddress_Click(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblBillDate_Click(object sender, EventArgs e)
        {

        }

        private void dtpBillDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pnlProductDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblProductTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblProductSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblProductName_Click(object sender, EventArgs e)
        {

        }

        private void lblInventory_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void lblRate_Click(object sender, EventArgs e)
        {

        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlDataGridView_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDGVTitle_Click(object sender, EventArgs e)
        {

        }

        private void dgvAddedProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblcalculationTitle_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void lblSubTotal_Click(object sender, EventArgs e)
        {

        }

        private void lblDiscount_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textdiscoutamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtcg_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtsg_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblVat_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textcgstamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textsgstamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textigstamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void lblGrandTotal_Click(object sender, EventArgs e)
        {

        }

        private void txtGrandTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
