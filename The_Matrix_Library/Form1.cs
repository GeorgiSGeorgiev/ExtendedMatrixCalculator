using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Matrix_Operations;

namespace Extended_Matrix_Calculator
{
    public partial class Form1 : Form
    {
        The_Menu Menu1;

        Panel Choose_Swap_RC_Panel;
        NumericUpDown First_Row_or_Column;
        NumericUpDown Second_Row_or_Column;
        Label Double_Arrow;
        Button Menu_Button_OK;

        Choice_screen Choice_screen1;
        Matrix_screen Matrix_screen1 = new Matrix_screen();
        Matrix_screen Matrix_screen2 = new Matrix_screen();
        Result_Screen Result_screen1;
        Timer Menu_Timer;
        Timer Choice_Screen_Timer;
        Timer Matrix_Timer;
        Timer Result_Timer;
        Button The_Hamburger_Button;

        public int current_step = 0;

        Boolean M1_Timer_active = false;

        public Form1()
        {
            InitializeComponent();
            this.current_step++;
            this.Size = new Size(880, 750);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(610, 785);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Text = "Extended Matrix Calculator";
            //this.Click += Form1_Click;

            this.The_Hamburger_Button = new Button();
            this.The_Hamburger_Button.BringToFront();
            this.The_Hamburger_Button.Size = new Size(42, 42);
            this.The_Hamburger_Button.Location = new Point(1, 2);
            this.The_Hamburger_Button.FlatStyle = FlatStyle.Popup;
            this.The_Hamburger_Button.BackColor = Color.FromArgb(100, 255, 255, 255); // border color
            this.The_Hamburger_Button.BackgroundImage = Image.FromFile($@"{Environment.CurrentDirectory}\Pics\menu.png");
            this.The_Hamburger_Button.ImageAlign = ContentAlignment.TopCenter;
            this.The_Hamburger_Button.BackgroundImageLayout = ImageLayout.Stretch;
            this.The_Hamburger_Button.Click += Menu_Click;
            this.Controls.Add(The_Hamburger_Button);

            Menu1 = new The_Menu();
            this.Controls.Add(Menu1);

            Choose_Swap_RC_Panel = new Panel();
            Choose_Swap_RC_Panel.Location = new Point(210, 129);
            Choose_Swap_RC_Panel.Size = new Size(290, 71);
            Choose_Swap_RC_Panel.BringToFront();
            Choose_Swap_RC_Panel.BackColor = Color.Black;
            Choose_Swap_RC_Panel.BorderStyle = BorderStyle.FixedSingle;

            First_Row_or_Column = new NumericUpDown();
            First_Row_or_Column.Text = "";
            First_Row_or_Column.Font = new Font(First_Row_or_Column.Font.FontFamily, 19);
            First_Row_or_Column.TextAlign = HorizontalAlignment.Center;
            First_Row_or_Column.Minimum = Int32.MinValue;
            First_Row_or_Column.Maximum = Int32.MaxValue;
            First_Row_or_Column.Size = new Size(80, 70);
            First_Row_or_Column.Location = new Point(9, 16);
            Choose_Swap_RC_Panel.Controls.Add(First_Row_or_Column);

            Second_Row_or_Column = new NumericUpDown();
            Second_Row_or_Column.Text = "";
            Second_Row_or_Column.Font = new Font(Second_Row_or_Column.Font.FontFamily, 19);
            Second_Row_or_Column.TextAlign = HorizontalAlignment.Center;
            Second_Row_or_Column.Minimum = Int32.MinValue;
            Second_Row_or_Column.Maximum = Int32.MaxValue;
            Second_Row_or_Column.Size = new Size(80, 70);
            Second_Row_or_Column.Location = new Point(126, 16);
            Choose_Swap_RC_Panel.Controls.Add(Second_Row_or_Column);

            Double_Arrow = new Label();
            Double_Arrow.ForeColor = Color.LawnGreen;
            Double_Arrow.Size = new Size(35, 36);
            Double_Arrow.Location = new Point(92, 11);
            Double_Arrow.Font = new Font(Double_Arrow.Font.FontFamily, 25);
            Double_Arrow.TextAlign = ContentAlignment.MiddleCenter;
            Double_Arrow.Text = char.ConvertFromUtf32(0x21D4);
            Choose_Swap_RC_Panel.Controls.Add(Double_Arrow);

            Menu_Button_OK = new Button();
            Menu_Button_OK.FlatStyle = FlatStyle.Popup;
            Menu_Button_OK.Text = "OK";
            Menu_Button_OK.TextAlign = ContentAlignment.MiddleCenter;
            Menu_Button_OK.Font = new Font(Menu_Button_OK.Font.FontFamily, 15);
            Menu_Button_OK.ForeColor = Color.LawnGreen;
            Menu_Button_OK.BackColor = Color.FromArgb(15, 170, 255, 255);
            Menu_Button_OK.Size = new Size(60, 36);
            Menu_Button_OK.Location = new Point(215, 16);
            Menu_Button_OK.Click += new EventHandler(Menu_Button_OK_Click);
            Choose_Swap_RC_Panel.Controls.Add(Menu_Button_OK);
            this.Controls.Add(Choose_Swap_RC_Panel);
            Choose_Swap_RC_Panel.Enabled = false;
            Choose_Swap_RC_Panel.Visible = false;

            Menu_Timer = new Timer();
            Menu_Timer.Tick += Menu_Timer_Tick;

            Choice_screen1 = new Choice_screen();
            Choice_screen1.Add_Number_Of_Rows.Focus();
            Choice_Screen_Timer = new Timer();
            Choice_Screen_Timer.Tick += Choice_Screen_Timer_Tick;
            Choice_Screen_Timer.Start();
            this.Controls.Add(Choice_screen1);
        }

        //********************************** Help Functions *********************************\\

        private void Result_Screen_Intro_Single(int rows, int columns, double[,] the_matrix, int Chosen_Mode)
        {
            this.current_step++;
            this.Menu1.Refresh_Accuracy();
            Result_screen1 = new Result_Screen(rows, columns, the_matrix, Chosen_Mode, this.Menu1.Accuracy_number, null);
            this.Controls.Add(Result_screen1);
            this.Result_Timer = new Timer();
            Result_Timer.Tick += Result_Timer_Tick;
            Result_Timer.Start();
            Menu1.Disable_all_No_Changes();
        }

        private void Result_Screen_LSM(int rows, int columns, double[,] the_matrix, TextBox[] text_info)
        {
            this.current_step++;
            this.Menu1.Refresh_Accuracy();
            Result_screen1 = new Result_Screen(rows, columns, the_matrix, 15, this.Menu1.Accuracy_number, text_info);
            this.Controls.Add(Result_screen1);
            this.Result_Timer = new Timer();
            Result_Timer.Tick += Result_Timer_Tick;
            Result_Timer.Start();
            Menu1.Disable_all_No_Changes();
        }

        private void Result_Screen_Intro_Double(int rows_1, int columns_1, double[,] matrix_1, int rows_2, int columns_2, double[,] matrix_2, int Chosen_Mode)
        {
            this.current_step++;
            this.Menu1.Refresh_Accuracy();
            Result_screen1 = new Result_Screen(rows_1, columns_1, matrix_1, rows_2, columns_2, matrix_2, Chosen_Mode, this.Menu1.Accuracy_number);
            this.Controls.Add(Result_screen1);
            this.Result_Timer = new Timer();
            Result_Timer.Tick += Result_Timer_Tick;
            Result_Timer.Start();
            Menu1.Disable_all_No_Changes();
        }

        private void All_Labels_and_Buttons_Text_Refresh()
        {
            if (Choice_screen1 != null)
            {
                this.Choice_screen1.LSM_CBox.Text = Writings.Least_Squares_Method;
                if (!Choice_screen1.LSM_active)
                {
                    this.Choice_screen1.Rows_Label.Text = Writings.Number_of_rows;
                    this.Choice_screen1.Columns_Label.Text = Writings.Number_of_columns;
                }
                else
                {
                    this.Choice_screen1.Rows_Label.Text = Writings.Samples_count;
                    this.Choice_screen1.Columns_Label.Text = Writings.Data_types;
                }

                this.Choice_screen1.Next.Text = Writings.Next;
                this.Choice_screen1.Back.Text = Writings.Back;
                if (!this.Choice_screen1.LSM_CBox.Enabled && current_step > 2)
                {
                    this.Choice_screen1.Title.Text = Writings.Second_Matrix_Size;
                }
                else
                {
                    this.Choice_screen1.Title.Text = "Extended" + System.Environment.NewLine + "Matrix Calculator";
                }

            }

            if (this.Menu1 != null)
            {
                this.Menu1.Refresh_Text();
            }

            if (current_step >= 2)
            {
                this.Matrix_screen1.Refresh_Text();
            }

            if (current_step >= 4)
            {
                this.Matrix_screen2.Refresh_Text();
            }
        }

        //********************************* Event Handlers *********************************\\

        /*private void Form1_Click(object sender, EventArgs e)
        {
            if (Menu_is_opened)
            {
                this.The_Hamburger_Button.FlatStyle = FlatStyle.Popup;
                this.Menu1.open_close(Menu_is_opened);
                this.Menu_is_opened = false;
                Choose_Swap_RC.Enabled = false;
                Choose_Swap_RC.Visible = false;
                this.Menu_Timer.Stop();
            }
        }*/

        private void Menu_Click(object sender, EventArgs e)
        {
            if (!this.Menu1.Menu_is_opened/*!Menu_is_opened*/)
            {
                this.The_Hamburger_Button.FlatStyle = FlatStyle.Flat;
                this.Menu1.Open_Close();
                this.Menu_Timer.Start();
                //MessageBox.Show("Opened");
            }
            else // menu je otevrene, je zapotrebi vynulovat vsechny jeho hodnoty a zavrit ho 
            {
                this.Menu_Timer.Stop();
                this.Menu1.Refresh_Accuracy();
                this.The_Hamburger_Button.FlatStyle = FlatStyle.Popup;
                this.Menu1.Open_Close();
                //MessageBox.Show("Closed");
                if (this.Menu1.Chosen_Mode == 0 || this.Menu1.Chosen_Mode == 1)
                {
                    int i = 9000;
                    if (Menu1.Memory == 0) { i = 0; }
                    else { i = 1; }
                    this.Menu1.Enable_ModeChoose_Button(i); // pozor, je zapotrebi vratit do puvodnem stavu i Memory, jinak muze nastat Memory==Chosen_Mode 
                }
                Choose_Swap_RC_Panel.Enabled = false; // zavrit dodatecny row_column panel
                Choose_Swap_RC_Panel.Visible = false;
                if (Matrix_screen1 != null)
                {
                    if (Matrix_screen1.Matrix_Number != 0)
                    {
                        Matrix_screen1.Change_Decimal_Places(this.Menu1.Accuracy_number);
                    }
                }
                if (Matrix_screen2 != null)
                {
                    if (Matrix_screen2.Matrix_Number != 0)
                    {
                        Matrix_screen2.Change_Decimal_Places(this.Menu1.Accuracy_number);
                    }
                }
            }
        }

        private void Menu_Button_OK_Click(object sender, EventArgs e)
        {
            if (this.First_Row_or_Column.Text != "" && this.First_Row_or_Column.Value != this.Second_Row_or_Column.Value
                && this.Second_Row_or_Column.Text != "")
            {
                this.Menu1.first = Decimal.ToInt32(First_Row_or_Column.Value);
                this.Menu1.second = Decimal.ToInt32(Second_Row_or_Column.Value);
                if (current_step == 2 && this.Menu1.Chosen_Mode == 0 && Menu1.first < Matrix_screen1.Rows && Menu1.second < Matrix_screen1.Rows ||
                    current_step == 4 && this.Menu1.Chosen_Mode == 0 && Menu1.first < Matrix_screen2.Rows && Menu1.second < Matrix_screen2.Rows)
                {
                    if (current_step == 2)
                    {
                        Matrix_screen1.Remember_the_matrix();
                        Matrix_screen1.the_matrix = Matrix_Library.Swap_Rows(Matrix_screen1.Rows, Matrix_screen1.Columns, Matrix_screen1.the_matrix, Menu1.first, Menu1.second);
                        Matrix_screen1.Refresh_the_matrix();
                    }
                    else // step 4
                    {
                        this.Matrix_screen2.Remember_the_matrix();
                        Matrix_screen2.the_matrix = Matrix_Library.Swap_Rows(Matrix_screen2.Rows, Matrix_screen2.Columns, Matrix_screen2.the_matrix, Menu1.first, Menu1.second);
                        this.Matrix_screen2.Refresh_the_matrix();
                    }
                    this.Menu1.Enable_ModeChoose_Button(0);
                    this.Choose_Swap_RC_Panel.Enabled = false;
                    this.Choose_Swap_RC_Panel.Visible = false;
                }
                else if (current_step == 2 && this.Menu1.Chosen_Mode == 1 && Menu1.first < Matrix_screen1.Columns && Menu1.second < Matrix_screen1.Columns || 
                         current_step == 4 && this.Menu1.Chosen_Mode == 1 && Menu1.first < Matrix_screen2.Columns && Menu1.second < Matrix_screen2.Columns) // swap columns
                {
                    if (current_step == 2)
                    {
                        Matrix_screen1.Remember_the_matrix();
                        Matrix_screen1.the_matrix = Matrix_Library.Swap_Columns(Matrix_screen1.Rows, Matrix_screen1.Columns, Matrix_screen1.the_matrix, Menu1.first, Menu1.second);
                        Matrix_screen1.Refresh_the_matrix();
                    }
                    else // step 4 == Matrix_2
                    {
                        this.Matrix_screen2.Remember_the_matrix();
                        Matrix_Library.Swap_Columns(Matrix_screen2.Rows, Matrix_screen2.Columns, Matrix_screen2.the_matrix, Menu1.first, Menu1.second);
                        this.Matrix_screen2.Refresh_the_matrix();
                    }
                    this.Menu1.Enable_ModeChoose_Button(1);
                    Choose_Swap_RC_Panel.Enabled = false;
                    Choose_Swap_RC_Panel.Visible = false;
                }
                else
                {
                    MessageBox.Show(Writings.Unacceptable_Value);
                }
            }
            else if (this.First_Row_or_Column.Text == "" && this.Second_Row_or_Column.Text == "") // neudela nic krome zavreni okinka
            {
                int index = 0;
                if (this.Menu1.Chosen_Mode == 0) // ktere tlacitko se musi vratit do normalniho stavu
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }
                this.Menu1.Enable_ModeChoose_Button(index);
                Choose_Swap_RC_Panel.Enabled = false;
                Choose_Swap_RC_Panel.Visible = false;
            }
            else if (this.First_Row_or_Column.Value == this.Second_Row_or_Column.Value)
            {
                MessageBox.Show(Writings.Same_Value);
            }
            else
            {
                MessageBox.Show(Writings.Unacceptable_Value);
            }
        }

        private void Choice_Screen_Timer_Tick(object sender, EventArgs e)
        {
            if (this.Choice_screen1.LSM_click_IDentificator == 1 && Choice_screen1.LSM_active)
            {
                this.Choice_screen1.LSM_click_IDentificator = 0;
                this.Menu1.Choose_Mode(15);
                this.Menu1.Accuracy.Minimum = 5;
                this.Menu1.Long_Lasting_Memory = 15;
            }
            else if (this.Choice_screen1.LSM_click_IDentificator == 1 && !Choice_screen1.LSM_active)
            {
                this.Choice_screen1.LSM_click_IDentificator = 0;
                this.Menu1.Reset_Menu_Unselectable();
                this.Menu1.Accuracy.Minimum = 0;
                this.Menu1.Accuracy.Value = 2;
                this.Menu1.Long_Lasting_Memory = 9000;
            }

            if (Choice_screen1.Next_Click_IDentificator == 1) // Next click
            {
                Choice_Screen_Timer.Stop();
                this.Menu1.Refresh_Accuracy();
                this.current_step++;
                Choice_screen1.Next_Click_IDentificator = 0;
                Choice_screen1.Enabled = false;
                Choice_screen1.Visible = false;
                Controls.Remove(Choice_screen1);
                if (current_step == 2)
                {
                    Matrix_screen1 = null;
                    Matrix_screen1 = new Matrix_screen(Choice_screen1.Number_of_rows, Choice_screen1.Number_of_columns, Choice_screen1.LSM_active, 1, Menu1.Accuracy_number);
                    Controls.Add(Matrix_screen1);
                    Matrix_Timer = new Timer();
                    Matrix_Timer.Tick += Matrix_Timer_Tick;
                    Matrix_Timer.Start();
                    if (!this.Choice_screen1.LSM_active)
                    {
                        Menu1.Matrix1_New_Menu_Init();
                    }
                    else
                    {
                        Menu1.Activate_first_two_buttons();
                    }
                    this.Choice_screen1.Number_of_rows_Memory = this.Choice_screen1.Number_of_rows;
                    this.Choice_screen1.Number_of_columns_Memory = this.Choice_screen1.Number_of_columns;
                }
                else if (current_step == 4)
                {
                    int New_Matrix_ID = Matrix_screen1.Matrix_Number + 1;
                    this.Menu1.Activate_first_two_buttons();
                    //MessageBox.Show(Convert.ToString(Menu1.Long_Lasting_Memory) + Convert.ToString(Menu1.Chosen_Mode));
                    Matrix_screen2 = null;
                    Matrix_screen2 = new Matrix_screen(Choice_screen1.Number_of_rows, Choice_screen1.Number_of_columns, false, New_Matrix_ID, Menu1.Accuracy_number);
                    this.Controls.Add(Matrix_screen2);
                    this.Matrix_Timer.Start();
                    //Menu1.Result_Matrix2_Menu(); //IMPORTANT, DO NOT FORGET
                }
            }
            else if (Choice_screen1.Back_Click_IDentificator == 1)
            {
                Choice_Screen_Timer.Stop();
                this.current_step--;
                Matrix_Timer.Start();
                Choice_screen1.Back_Click_IDentificator = 0;
                this.Menu1.Refresh_Accuracy();
                Menu1.Menu_Back_to_matrix_1();
                this.Matrix_screen1.Visible = true;
                this.Matrix_screen1.Enabled = true;
                this.Matrix_screen1.Change_Decimal_Places(this.Menu1.Accuracy_number);
                this.Controls.Add(Matrix_screen1);
                Choice_screen1.Matrix_2_Choice_Screen_Back();
            }
        }

        private void Matrix_Timer_Tick(object sender, EventArgs e)
        {
            if (current_step == 2)
            {
                if (!M1_Timer_active)
                {
                    this.Matrix_screen1.Inside_Matrix_Mode_Timer.Start();
                    M1_Timer_active = true;
                }
                this.Matrix_screen1.Refresh_the_mode(this.Menu1.Chosen_Mode);
            }
            else
            {
                this.Matrix_screen1.Inside_Matrix_Mode_Timer.Stop();
                M1_Timer_active = false;
            }
            if (this.Matrix_screen2.Next_Click_IDentificator == 1)
            {
                this.Matrix_Timer.Stop();
                this.Matrix_screen2.Next_Click_IDentificator = 0;
                this.Menu1.Disable_all_No_Changes();
                this.Matrix_screen2.Remember_the_matrix();
                this.Matrix_screen2.Enabled = false;
                this.Matrix_screen2.Visible = false;
                this.Controls.Remove(Matrix_screen2);
                this.Result_Screen_Intro_Double(Matrix_screen1.Rows, Matrix_screen1.Columns, Matrix_screen1.the_matrix,
                                                Matrix_screen2.Rows, Matrix_screen2.Columns, Matrix_screen2.the_matrix, Menu1.Long_Lasting_Memory);
            }
            else if (this.Matrix_screen2.Back_Click_IDentificator == 1)
            {
                this.Matrix_Timer.Stop();
                this.Matrix_screen2.Back_Click_IDentificator = 0;
                this.Menu1.Disable_mods_No_Changes();
                this.current_step--;
                this.Controls.Remove(Matrix_screen2);
                this.Choice_Screen_Timer.Start();
                Matrix_screen2 = null;
                Matrix_screen2 = new Matrix_screen();
                Choice_screen1.Visible = true;
                Choice_screen1.Enabled = true;
                this.Controls.Add(Choice_screen1);
            }
            else if (Matrix_screen1.Next_Click_IDentificator == 1 && Menu1.Chosen_Mode > 1 && Menu1.Chosen_Mode != 9000 && Menu1.Chosen_Mode != 0 
                     && Menu1.Chosen_Mode != 1 || (Matrix_screen1.Next_Click_IDentificator == 1 && Choice_screen1.LSM_active))
            {
                if (Menu1.Chosen_Mode < 8 || this.Matrix_screen1.Rows == this.Matrix_screen1.Columns || Choice_screen1.LSM_active || Menu1.Chosen_Mode == 14)
                {
                    Writings.Irrational_numbers = false;
                    Writings.Result_not_accurate_shown = false;
                    if (this.Choice_screen1.LSM_active)
                    {
                        this.Menu1.Chosen_Mode = this.Menu1.Long_Lasting_Memory; // pri LSM_active, kvuli moznosti prohodit radky a sloupce
                    }
                    else
                    {
                        this.Menu1.Long_Lasting_Memory = Menu1.Chosen_Mode; // zapamatuji si mod 
                    }
                    this.Matrix_Timer.Stop();
                    this.Matrix_screen1.Next_Click_IDentificator = 0;
                    this.Matrix_screen1.Remember_the_matrix(); // matice zapamatovana
                    this.Matrix_screen1.Enabled = false;
                    this.Matrix_screen1.Visible = false;
                    this.Controls.Remove(Matrix_screen1);

                    if (Menu1.Chosen_Mode == 2 || Menu1.Chosen_Mode == 3 || Menu1.Chosen_Mode == 4) // otevreni prvni obrazovky za ucelem vyberu druhe matice
                    {
                        this.Menu1.Disable_mods_No_Changes();
                        this.current_step++;
                        this.Matrix_screen1.Enabled = false;
                        this.Controls.Remove(Matrix_screen1);
                        this.Choice_Screen_Timer.Start();
                        //this.Choice_screen1.Enabled = true;
                        //this.Choice_screen1.Visible = true;
                        this.Choice_screen1.Matrix_2_Choice_Screen(Matrix_screen1.Rows, Matrix_screen1.Columns, Menu1.Chosen_Mode);
                        this.Controls.Add(Choice_screen1);
                    }
                    else // Mode >= 5 || Mode == 14 || Mode == 15 ; Activate Result_Screen
                    {
                        // Pozor! Result_Screen_Intro zvysuje counter kroku o 1
                        if (Choice_screen1.LSM_active)
                        {
                            Result_Screen_LSM(Matrix_screen1.Rows, Matrix_screen1.Columns, Matrix_screen1.the_matrix, Matrix_screen1.Scale_info);
                        }
                        else
                        {
                            Result_Screen_Intro_Single(Matrix_screen1.Rows, Matrix_screen1.Columns, Matrix_screen1.the_matrix, Menu1.Chosen_Mode);
                        }
                    }
                }
                else if (Menu1.Chosen_Mode != 9000 && Menu1.Chosen_Mode >= 8 && this.Matrix_screen1.Rows != this.Matrix_screen1.Columns)
                {
                    this.Menu1.Reset_Menu_Selectable();
                    Matrix_screen1.Next_Click_IDentificator = 0;
                    MessageBox.Show(Writings.Sqare_Matrix_Needed);
                }
            }
            else if (Matrix_screen1.Next_Click_IDentificator == 1 && (Menu1.Chosen_Mode == 9000 || Menu1.Chosen_Mode == 0 || Menu1.Chosen_Mode == 1))
            {
                Matrix_screen1.Next_Click_IDentificator = 0;
                MessageBox.Show(Writings.Mode_not_chosen);
            }
            else if (Matrix_screen1.Back_Click_IDentificator == 1)
            {
                Matrix_Timer.Stop();
                Matrix_screen1.Back_Click_IDentificator = 0;
                this.current_step--;
                this.Controls.Remove(Matrix_screen1);
                this.Choice_Screen_Timer.Start();
                Matrix_screen1 = null;
                Matrix_screen1 = new Matrix_screen();
                Choice_screen1.Enabled = true;
                Choice_screen1.Visible = true;
                this.Controls.Add(Choice_screen1);
                if (this.Menu1.Long_Lasting_Memory != 15)
                {
                    this.Menu1.Reset_Menu_Unselectable();
                }
                else
                {
                    this.Menu1.Disable_mods_No_Changes();
                }
            }
        }

        private void Menu_Timer_Tick(object sender, EventArgs e) //active when menu is opened
        {
            // language change tracking
            if (Menu1.swap_rows)
            {
                Menu1.swap_rows = false;
                this.Choose_Swap_RC_Panel.Enabled = true;
                this.Choose_Swap_RC_Panel.Visible = true;
                this.Choose_Swap_RC_Panel.BringToFront();
            }
            else if (Menu1.swap_columns)
            {
                Menu1.swap_columns = false;
                this.Choose_Swap_RC_Panel.Enabled = true;
                this.Choose_Swap_RC_Panel.Visible = true;
                this.Choose_Swap_RC_Panel.BringToFront();
            }
            else if (Menu1.Chosen_Mode != 0 && Menu1.Chosen_Mode != 1)
            {
                this.Choose_Swap_RC_Panel.Enabled = false;
                this.Choose_Swap_RC_Panel.Visible = false;
            }

            if (Menu1.Language_Button_Clicked == 1)
            {
                this.Menu1.Language_Button_Clicked = 0;
                Writings.Change_Language(this.Menu1.Chosen_Language);
                this.All_Labels_and_Buttons_Text_Refresh();
            }
        }

        private void Result_Timer_Tick(object sender, EventArgs e)
        {
            if (this.Result_screen1.Close_IDentificator == 1)
            {
                this.Result_Timer.Stop();
                this.Result_screen1.Close_IDentificator = 0;
                this.Close();
            }
            else if (this.Result_screen1.Back_IDentificator == 1)
            {
                this.Result_Timer.Stop();
                current_step--;
                this.Result_screen1.Back_IDentificator = 0;
                this.Result_screen1.Enabled = false;
                this.Controls.Remove(Result_screen1);
                this.Result_screen1 = null;
                this.Matrix_Timer.Start();
                if (this.current_step == 4)
                {
                    this.Menu1.Activate_first_two_buttons();
                    this.Controls.Add(Matrix_screen2);
                    this.Matrix_screen2.Enabled = true;
                    this.Matrix_screen2.Visible = true;
                }
                else
                {
                    if (!this.Choice_screen1.LSM_active)
                    {
                        this.Menu1.Menu_Back_to_matrix_1();
                    }
                    else
                    {
                        this.Menu1.Activate_first_two_buttons();
                        //MessageBox.Show(Convert.ToString(Menu1.Memory));
                    }
                    this.Controls.Add(Matrix_screen1);
                    this.Matrix_screen1.Enabled = true;
                    this.Matrix_screen1.Visible = true;
                    this.The_Hamburger_Button.BringToFront();
                }
            }
            else if (this.Result_screen1.Open_IDentificator == 1)
            {
                this.Result_screen1.Open_IDentificator = 0;
                Process.Start(this.Result_screen1.Result_file_name);
            }
            else if (this.Result_screen1.Save_as_IDentificator == 1)
            {
                this.Result_screen1.Save_as_IDentificator = 0;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text File|*.txt";
                saveFileDialog1.Title = Writings.Save_the_Result;
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }
                    File.Copy(this.Result_screen1.Result_file_name, saveFileDialog1.FileName);
                }
            }
            else if (this.Result_screen1.Small_Open_IDentificator > -1)
            {
                this.Result_screen1.Small_Open_IDentificator = -1;
                Process.Start(this.Result_screen1.more_names[this.Result_screen1.Small_Buttons_Memory]);
            }
            else if (this.Result_screen1.Small_Save_as_IDentificator > -1 && this.Result_screen1.Small_Save_as_IDentificator != 100)
            {
                this.Result_screen1.Small_Save_as_IDentificator = -1;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text File|*.txt";
                saveFileDialog1.Title = Writings.Saving_Matrix + Result_screen1.Matrix_ID[this.Result_screen1.Small_Buttons_Memory].Text;

                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }
                    File.Copy(this.Result_screen1.Result_file_name, saveFileDialog1.FileName);
                }
            }
            else if (this.Result_screen1.The_Diagram != null && this.Result_screen1.The_Diagram.Error)
            {
                this.Result_screen1.Disable_LSM_Additional_Buttons();
            }

        }
    }
}
