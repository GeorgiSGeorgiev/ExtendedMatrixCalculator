using System;
using System.Drawing;
using System.Windows.Forms;

namespace Extended_Matrix_Calculator
{
    public class Choice_screen: Panel
    {
        public int Next_Click_IDentificator = 0;
        public int Back_Click_IDentificator = 0;
        public int Number_of_rows = 0;
        public int Number_of_columns = 0;
        public decimal Number_of_columns_Memory = 0;
        public decimal Number_of_rows_Memory = 0;
        //private decimal rows_temporary = 0;
        private decimal columns_temporary = 0; // temp ulozeni hodnoty (v pripadu unchecknuti se hodnota vrati), jestli byla vybrana moznost LSM
        public Boolean LSM_active = false; // ulozi pro budoucnost to, ze LSM je aktivni
        public int LSM_click_IDentificator = 0;
        //public Boolean Menu_is_active = false;

        public Label Title;
        public NumericUpDown Add_Number_Of_Rows;
        public NumericUpDown Add_Number_Of_Columns;
        public Label Rows_Label;
        public Label Columns_Label;
        public Button Next;
        public Button Back;
        public CheckBox LSM_CBox;


        //********************************** Constructors *********************************\\

        public Choice_screen()
        {
            this.Dock = DockStyle.Fill; //Panel se meni spolecne s formou (spolecne s rodicem)
            this.BackColor = Color.Black;
            this.AutoScroll = true;
            this.AllowDrop = false;
            //this.Click += Close_Menu;

            Title = new Label();
            Title.Anchor = AnchorStyles.Top;
            Title.ForeColor = Color.LawnGreen;
            Title.Font = new Font("Comic Sans MS", 30, FontStyle.Bold);
            Title.TextAlign = ContentAlignment.MiddleCenter;
            Title.Size = new Size(370, 130);
            Title.Location = new Point(this.ClientSize.Width / 2 - Title.Width / 2, 35);
            Title.Text ="Extended" + System.Environment.NewLine + "Matrix Calculator";

            Rows_Label = new Label();
            Columns_Label = new Label();

            //Rows_Label.BackColor = Color.Red;
            Rows_Label.Anchor = AnchorStyles.None;
            Rows_Label.Font = new Font(Rows_Label.Font.FontFamily, 16);
            Rows_Label.ForeColor = Color.White;
            Rows_Label.Location = new Point(this.ClientSize.Width / 2 - 195,
                                            this.ClientSize.Height / 2 - 19);
            Rows_Label.Size = new Size(180, 26);
            Rows_Label.Text = Writings.Number_of_rows;

            Columns_Label.Anchor = AnchorStyles.None;
            Columns_Label.Font = new Font(Columns_Label.Font.FontFamily, 16);
            Columns_Label.ForeColor = Color.White;
            Columns_Label.Location = new Point(this.ClientSize.Width / 2 + 40,
                                               this.ClientSize.Height / 2 - 19);
            Columns_Label.Size = new Size(180, 26);
            Columns_Label.Text = Writings.Number_of_columns;



            Add_Number_Of_Rows = new NumericUpDown();
            Add_Number_Of_Columns = new NumericUpDown();

            Add_Number_Of_Rows.Anchor = AnchorStyles.None;
            Add_Number_Of_Rows.Text = "";
            Add_Number_Of_Rows.Font = new Font(Add_Number_Of_Rows.Font.FontFamily, 19);
            Add_Number_Of_Rows.TextAlign = HorizontalAlignment.Center;
            Add_Number_Of_Rows.Minimum = 0;
            Add_Number_Of_Rows.Maximum = 401;
            Add_Number_Of_Rows.Location = new Point(this.ClientSize.Width / 2 - 195,
                                                    this.ClientSize.Height / 2 + 25);
            Add_Number_Of_Rows.Size = new Size(150, 150); //width, height

            Add_Number_Of_Columns.Anchor = AnchorStyles.None;
            Add_Number_Of_Columns.Text = "";
            Add_Number_Of_Columns.Font = new Font(Add_Number_Of_Columns.Font.FontFamily, 19);
            Add_Number_Of_Columns.TextAlign = HorizontalAlignment.Center;
            Add_Number_Of_Columns.Minimum = 0;
            Add_Number_Of_Columns.Maximum = 401;
            Add_Number_Of_Columns.Location = new Point(this.ClientSize.Width / 2 + 40,
                                                       this.ClientSize.Height / 2 + 25);
            Add_Number_Of_Columns.Size = new Size(150, 150);


            Label The_X = new Label();
            The_X.Anchor = AnchorStyles.None;
            The_X.Font = new Font(Rows_Label.Font.FontFamily, 14);
            The_X.ForeColor = Color.Gray;
            The_X.Size = new Size(30, 30);
            The_X.Location = new Point(this.ClientSize.Width / 2 - The_X.Size.Width / 2,
                                       this.ClientSize.Height / 2 + 35);
            The_X.TextAlign = ContentAlignment.MiddleCenter;
            The_X.Text = "X";


            Next = new Button();
            Next.Anchor = AnchorStyles.None;
            Next.FlatStyle = FlatStyle.Popup;
            Next.Font = new Font(Next.Font.FontFamily, 15);
            Next.ForeColor = Color.LawnGreen;
            Next.BackColor = Color.FromArgb(15, 170, 255, 255);
            Next.Click += new EventHandler(Button_Next_Click);
            Next.Size = new Size(90, 50);
            Next.Location = new Point(this.ClientSize.Width / 2 - Next.Size.Width / 2,
                                      this.ClientSize.Height / 2 + 120);
            Next.Text = Writings.Next;
            //Next.Text = "Další";

            Back = new Button();
            Back.Anchor = AnchorStyles.None;
            Back.FlatStyle = FlatStyle.Popup;
            Back.Font = new Font(Back.Font.FontFamily, 15);
            Back.ForeColor = Color.LawnGreen;
            Back.BackColor = Color.FromArgb(15, 170, 255, 255);
            Back.Click += new EventHandler(Button_Back_Click);
            Back.Size = new Size(90, 50);
            Back.Location = new Point(this.ClientSize.Width / 2 - Next.Size.Width / 2 - 70,
                                      this.ClientSize.Height / 2 + 120);
            Back.Text = Writings.Back;
            Back.Enabled = false;
            Back.Visible = false;


            Add_Number_Of_Rows.KeyDown += new KeyEventHandler(Boxes_KeyDown);
            Add_Number_Of_Columns.KeyDown += new KeyEventHandler(Boxes_KeyDown);

            LSM_CBox = new CheckBox();
            LSM_CBox.Anchor = AnchorStyles.None;
            LSM_CBox.FlatStyle = FlatStyle.Popup;
            LSM_CBox.Size = new Size(300, 160);
            //LSM_CBox.Location = new Point(100, -230);
            LSM_CBox.Location = new Point(this.ClientSize.Width / 2 - Next.Size.Width / 2 - 50, 242);
            LSM_CBox.ForeColor = Color.White;
            LSM_CBox.Font = new Font(LSM_CBox.Font.FontFamily, 10);
            LSM_CBox.Text = Writings.Least_Squares_Method;
            LSM_CBox.CheckedChanged += LSM_CBox_CheckChanged;

            this.Controls.Add(Title);
            this.Controls.Add(Rows_Label);
            this.Controls.Add(Columns_Label);
            this.Controls.Add(Add_Number_Of_Rows);
            this.Controls.Add(Add_Number_Of_Columns);
            this.Controls.Add(The_X);
            this.Controls.Add(Next);
            this.Controls.Add(Back);
            this.Controls.Add(LSM_CBox);
        }


        //********************************** Help Functions *********************************\\

        public void Matrix_2_Choice_Screen_Back()
        {
            this.Add_Number_Of_Rows.Enabled = true;
            this.Add_Number_Of_Columns.Enabled = true;
            this.Back.Enabled = false;
            this.Back.Visible = false;
            this.LSM_CBox.Visible = true;
            this.LSM_CBox.Enabled = true;
            this.Next.Left -= 70;
            this.Add_Number_Of_Rows.Value = this.Number_of_rows_Memory;
            this.Add_Number_Of_Columns.Value = this.Number_of_columns_Memory;
            this.Visible = false;
            this.Enabled = false;
            this.Title.Text = "Extended" + System.Environment.NewLine + "Matrix Calculator"; ;
        }

        public void Matrix_2_Choice_Screen(int Matrix_1_Rows, int Matrix_1_Columns, int Chosen_Mode)
        {
            this.Enabled = true;
            this.Visible = true;
            this.Add_Number_Of_Rows.Enabled = false;
            if (Chosen_Mode == 2 || Chosen_Mode == 3) // Sum or Subtract
            {
                this.Add_Number_Of_Rows.Value = Matrix_1_Rows;
                this.Add_Number_Of_Columns.Enabled = false;
                this.Add_Number_Of_Columns.Value = Matrix_1_Columns;
            }
            else // Multiply
            {
                this.Add_Number_Of_Rows.Value = Matrix_1_Columns;
                this.Add_Number_Of_Columns.Value = 0;
                this.Add_Number_Of_Columns.Text = "";
            }
            this.Title.Text = Writings.Second_Matrix_Size;
            this.LSM_CBox.Enabled = false;
            this.LSM_CBox.Visible = false;

            this.Back.Enabled = true;
            this.Back.Visible = true;
            this.Next.Left += 70;
        }

        //********************************** Event Handlers *********************************\\

        private void Button_Next_Click(object sender, EventArgs e) //Mouse Click Handler
        {
            if ( Add_Number_Of_Rows.Text != "" && Add_Number_Of_Rows.Value != 0
                 && Add_Number_Of_Columns.Text != "" && Add_Number_Of_Columns.Value != 0 )
            {
                this.Next_Click_IDentificator = 1; // data jsou validni => dalsi obrazovka
                Number_of_rows = Decimal.ToInt32(Add_Number_Of_Rows.Value);
                Number_of_columns = Decimal.ToInt32(Add_Number_Of_Columns.Value);
                columns_temporary = Number_of_columns; // zapamatuj pocet sloupcu v pripade navratu
                //MessageBox.Show(Convert.ToString(Number_of_columns));
            }
            else
            {
                MessageBox.Show(Writings.Unacceptable_Value);
            }
            //throw new NotImplementedException();
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            this.Back_Click_IDentificator = 1;
        }

        private void Boxes_KeyDown(object sender, KeyEventArgs e) //ENTER handler
        {
            bool num_of_rows_set = false;
            bool num_of_columns_set = false;
            if (e.KeyCode == Keys.Enter)
            {
                if (Add_Number_Of_Rows.Text != "" && Add_Number_Of_Rows.Value != 0)
                {
                    num_of_rows_set = true;
                    Add_Number_Of_Columns.Focus();
                }
                if (Add_Number_Of_Columns.Text != "" && Add_Number_Of_Columns.Value != 0)
                {
                    num_of_columns_set = true;
                    if (!num_of_rows_set)
                    {
                        Add_Number_Of_Rows.Focus();
                    }
                }
                if (num_of_rows_set && num_of_columns_set)
                {
                    Button_Next_Click(sender, e);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void LSM_CBox_CheckChanged(object sender, EventArgs e)
        {
            LSM_click_IDentificator = 1;
            if (LSM_CBox.Checked == true)
            {
                Rows_Label.Text = Writings.Samples_count;
                Columns_Label.Text = Writings.Data_types;
                Add_Number_Of_Rows.Focus();
                string temp_text = Add_Number_Of_Rows.Text;
                Add_Number_Of_Rows.Minimum = 2;
                if (temp_text == "")
                {
                    Add_Number_Of_Rows.Text = "";
                }
                columns_temporary = Add_Number_Of_Columns.Value;
                Add_Number_Of_Columns.Value = 2;
                Add_Number_Of_Columns.Enabled = false;
                this.LSM_active = true;
            }
            else
            {
                Rows_Label.Text = Writings.Number_of_rows;
                Columns_Label.Text = Writings.Number_of_columns;
                Add_Number_Of_Columns.Value = columns_temporary;
                Add_Number_Of_Rows.Focus();
                Add_Number_Of_Rows.Minimum = 0;
                if (columns_temporary == 0)
                {
                    Add_Number_Of_Columns.Text = "";
                }
                Add_Number_Of_Columns.Enabled = true;
                this.LSM_active = false;
            }
        }
    }
}
