using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Matrix_Operations;

namespace Extended_Matrix_Calculator
{
    class Matrix_screen : Panel
    {
        NumericUpDown[,] Field;
        Label Matrix_ID;
        Label[] Row_Labels;
        Label[] Column_labels;
        Button MS_Next;
        Button Back;
        Button Fill_With_Zeros;
        Button Clear_all;
        Button Open_from_a_file;
        public TextBox[] Scale_info;

        Pen pen1;
        Stream The_Stream = null;

        public Timer Inside_Matrix_Mode_Timer;

        int The_Menu_Chosen_Mode;
        int Bias_from_left_border = 30;
        int Horizontal_padding;
        int Vertical_padding;
        public double[,] the_matrix; // { get; set; }
        public int Matrix_Number;
        public int Rows;
        public int Columns;
        public int Next_Click_IDentificator;
        public int Back_Click_IDentificator;
        int last_horizontal, last_vertical; // last tiles position

        private Boolean drawn = false; //drawn urcuje kdy byla nakreslena cara
        private Boolean End_of_fields = false;


        //********************************** Constructors *********************************\\

        private void NumericUpDown_Init(int width, int height, int loc_X, int loc_Y, int font_size, ref NumericUpDown the_field, int accuracy)
        {
            the_field = new NumericUpDown();
            the_field.Text = "";
            the_field.DecimalPlaces = accuracy;
            the_field.Font = new Font(the_field.Font.FontFamily, font_size);
            the_field.TextAlign = HorizontalAlignment.Center;
            the_field.Minimum = Int32.MinValue;
            the_field.Maximum = Int32.MaxValue;
            the_field.Size = new Size(width, height);
            the_field.Margin = new Padding(10, 10, 10, 10);
            the_field.Location = new Point(loc_X, loc_Y);
            this.Controls.Add(the_field);
        }

        private void MS_Button_Init(Size The_Size, Point The_Location, int font_size, string text, ref Button The_Button)
        {
            The_Button = new Button();
            The_Button.FlatStyle = FlatStyle.Popup;
            The_Button.Text = text;
            The_Button.Font = new Font(The_Button.Font.FontFamily, font_size);
            The_Button.ForeColor = Color.LawnGreen;
            The_Button.BackColor = Color.FromArgb(15, 170, 255, 255);
            The_Button.Size = The_Size;
            The_Button.Location = The_Location;
            this.Controls.Add(The_Button);
        }

        private void MS_Label_Init(Size The_Size, Point The_Location, int font_size, string text, ContentAlignment The_Alignment, Color Text_colour, ref Label The_Label)
        {
            The_Label = new Label();
            The_Label.Location = The_Location;
            The_Label.Size = The_Size;
            The_Label.Font = new Font(The_Label.Font.FontFamily, font_size);
            The_Label.Text = text;
            The_Label.TextAlign = The_Alignment;
            The_Label.ForeColor = Text_colour;
            this.Controls.Add(The_Label);
        }

        private void Special_LSM_TextBox_Init(Size The_Size, Point The_Location, int font_size, string text, ref TextBox The_Box)
        {
            The_Box = new TextBox();
            The_Box.Location = The_Location;
            The_Box.Size = The_Size;
            The_Box.BorderStyle = BorderStyle.None;
            The_Box.Font = new Font(The_Box.Font.FontFamily, font_size);
            The_Box.Text = text;
            The_Box.TextAlign = HorizontalAlignment.Center;
            The_Box.ForeColor = Color.Yellow;
            The_Box.BackColor = Color.Black;
            The_Box.MaxLength = 8;
            this.Controls.Add(The_Box);
        }


        public Matrix_screen()
        {
            this.Matrix_Number = 0;
            this.Rows = 0;
            this.Columns = 0;
            this.Next_Click_IDentificator = 0;
            this.Back_Click_IDentificator = 0;
            this.last_horizontal = 0;
            this.last_vertical = 0;
        }

        public Matrix_screen(int number_of_rows, int number_of_columns, Boolean LSM_active, int ID, int accuracy)
        {
            this.pen1 = new Pen(Color.White, 3);
            this.Dock = DockStyle.Fill; //Panel se meni spolecne s formou (spolecne s rodicem)
            this.BackColor = Color.Black;
            this.AutoScroll = true;
            this.AllowDrop = false;

            this.Rows = number_of_rows;
            this.Columns = number_of_columns;
            this.Vertical_padding = 160;
            this.Horizontal_padding = 130;

            if (number_of_columns <= 2)
            {
                Horizontal_padding = 300;
            }

            if (LSM_active)
            {
                this.Scale_info = new TextBox[2];
                Size The_Size = new Size(85, 30);
                Point The_Location;
                for (int i = 0; i < 2; i++)
                {
                    The_Location = new Point(Horizontal_padding + i * 100, Vertical_padding - 10);
                    if (i == 0)
                    {
                        Special_LSM_TextBox_Init(The_Size, The_Location, 11, "X", ref Scale_info[i]);
                    }
                    else
                    {
                        Special_LSM_TextBox_Init(The_Size, The_Location, 11, "Y", ref Scale_info[i]);
                    }
                }
                this.The_Menu_Chosen_Mode = 15;
                this.Vertical_padding += 70;
            }
            else
            {
                this.The_Menu_Chosen_Mode = 9000;
            }

            this.Inside_Matrix_Mode_Timer = new Timer();
            this.Inside_Matrix_Mode_Timer.Tick += Inside_Matrix_Timer_Tick;
            this.Inside_Matrix_Mode_Timer.Start();

            Field = new NumericUpDown[number_of_rows, number_of_columns];

            for (int i = 0; i < number_of_rows; i++)
            {
                for (int j = 0; j < number_of_columns; j++)
                {
                    NumericUpDown_Init(85, 70, Horizontal_padding + j * 100, Vertical_padding + i * 45, 13, ref Field[i, j], accuracy);
                    Field[i, j].KeyDown += new KeyEventHandler(Enter_KeyDown);
                }
            }

            last_horizontal = Horizontal_padding + (number_of_columns - 1) * 100 / 2; // hledam stred, proto delim 2
            last_vertical = Vertical_padding + (number_of_rows - 1) * 45;


            Row_Labels = new Label[2 * number_of_rows];
            int counter = 0;
            string the_text = "";
            Size Label_Size = new Size();
            Point Label_Location = new Point();
            for (int i = 0; i < 2 * number_of_rows; i++)
            {
                Label_Size = new Size(30, 27);
                if (i < number_of_rows)
                {
                    Label_Location = new Point(Horizontal_padding - 40, Vertical_padding + i * 45);
                    the_text = Convert.ToString(counter);
                    counter++;
                }
                else
                {
                    if (i == number_of_rows)
                    {
                        counter = 0;
                    }
                    Label_Location = new Point( Horizontal_padding + number_of_columns * 100, Vertical_padding + (i - number_of_rows) * 45);
                    the_text = Convert.ToString(counter);
                    counter++;
                }
                MS_Label_Init(Label_Size, Label_Location, 9, the_text, ContentAlignment.MiddleCenter, Color.Gray, ref Row_Labels[i]);
            }

            Column_labels = new Label[2 * number_of_columns];
            counter = 0;
            for (int i = 0; i < 2 * number_of_columns; i++)
            {
                Label_Size = new Size(85, 27);
                if (i < number_of_columns)
                {
                    Label_Location = new Point(Horizontal_padding + i * 100, Vertical_padding - 38);
                    the_text = Convert.ToString(counter);
                    counter++;
                }
                else
                {
                    if (i == number_of_columns)
                    {
                        counter = 0;
                    }
                    Label_Location = new Point(Horizontal_padding + (i - number_of_columns) * 100, last_vertical + 35);
                    the_text = Convert.ToString(counter);
                    counter++;
                }
                MS_Label_Init(Label_Size, Label_Location, 9, the_text, ContentAlignment.MiddleCenter, Color.Gray, ref Column_labels[i]);
            }


            this.Matrix_Number = ID;
            MS_Label_Init(new Size(120, 26), new Point(Horizontal_padding - 2, 60), 13, Writings.Matrix_ID + this.Matrix_Number, ContentAlignment.MiddleLeft, Color.LawnGreen, ref Matrix_ID);

            MS_Button_Init(new Size(93, 26), new Point(Matrix_ID.Location.X + 2, Matrix_ID.Location.Y + 30), 8, Writings.Open_from_txt, ref Open_from_a_file);
            Open_from_a_file.Click += new EventHandler(Button_Open_from_a_file_Click);


            MS_Button_Init(new Size(85, 50), new Point(last_horizontal + 150, last_vertical + 75), 12, Writings.Next, ref MS_Next);
            MS_Next.Click += new EventHandler(Button_MS_Next_Click);

            MS_Button_Init(new Size(85, 50), new Point(last_horizontal + 50, last_vertical + 75), 12, Writings.Back, ref Back);
            Back.Click += new EventHandler(Button_Back_Click);

            MS_Button_Init(new Size(85, 50), new Point(last_horizontal - 50, last_vertical + 75), 12, Writings.Fill_0s, ref Fill_With_Zeros);
            Fill_With_Zeros.Click += new EventHandler(Button_Fill_With_Zeros_Click);

            MS_Button_Init(new Size(85, 50), new Point(last_horizontal - 150, last_vertical + 75), 12, Writings.Clear_all, ref Clear_all);
            Clear_all.Click += new EventHandler(Button_Clear_Click);
        }


        //********************************** Help Functions *********************************\\

        public void Refresh_the_mode(int mode)
        {
            this.The_Menu_Chosen_Mode = mode;
            //MessageBox.Show(Convert.ToString(mode));
        }

        public void Refresh_Text()
        {
            this.Matrix_ID.Text = Writings.Matrix_ID + Matrix_Number;
            this.MS_Next.Text = Writings.Next;
            this.Back.Text = Writings.Back;
            this.Fill_With_Zeros.Text = Writings.Fill_0s;
            this.Clear_all.Text = Writings.Clear_all;
            this.Open_from_a_file.Text = Writings.Open_from_txt;
        }

        public void Fill_With_Zeros_Method()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Field[i, j].Text == "")
                    {
                        Field[i, j].Value = 0;
                        Field[i, j].Text = "0";
                    }
                }
            }
        }

        private void Erase_Everything()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Field[i, j].Text != "" || Field[i,j].Value != 0)
                    {
                        Field[i, j].Value = 0;
                        Field[i, j].Text = "";
                    }
                }
            }
        }

        public void RREF_Mode_Fields() // Form1
        {
            for (int i = Rows; i < 2 * Rows; i++)
            {
                this.Field[i - Rows, Columns - 1].Left += Bias_from_left_border;
                this.Row_Labels[i].Left += Bias_from_left_border;
            }
            this.Column_labels[Columns - 1].Left += Bias_from_left_border;
            this.Column_labels[2 * Columns - 1].Left += Bias_from_left_border;

            this.Bias_from_left_border = this.Bias_from_left_border * (-1);
        }

        public void Remember_the_matrix() //ve skutecnosti zmeni format matice z Field[i,j].Value (decimal) na double
        {
            this.the_matrix = new double[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this.the_matrix[i, j] = (double)this.Field[i, j].Value;
                }
            }
        }

        public void Refresh_the_matrix()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this.Field[i, j].Value = (decimal)this.the_matrix[i, j];
                    this.Field[i, j].Text = Convert.ToString(this.Field[i, j].Value);
                }
            }
        }

        public void Change_Decimal_Places(int accuracy)
        {
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    this.Field[i, j].DecimalPlaces = accuracy;
                }
            }
        }

        //********************************* Event Handlers *********************************\\

        public void DrawLine(object sender, PaintEventArgs e)
        {
            Point A = new Point(this.Field[0, Columns - 1].Location.X - 22, this.Field[0, Columns - 1].Location.Y);
            Point B = new Point(this.Field[0, Columns - 1].Location.X - 22, this.Field[Rows - 1, Columns - 1].Location.Y + this.Field[Rows - 1, Columns - 1].Height);
            e.Graphics.DrawLine(pen1, A, B);
        }

        private void Inside_Matrix_Timer_Tick(object sender, EventArgs e)
        {
            if (The_Menu_Chosen_Mode == 7 && !drawn && this.Columns > 1)
            {
                drawn = true;
                RREF_Mode_Fields();
                this.Paint += DrawLine;
                this.Refresh();
            }
            else if (The_Menu_Chosen_Mode != 7 && drawn)
            {
                drawn = false;
                this.Paint -= DrawLine;
                RREF_Mode_Fields();
                this.Refresh();
            }
        }

        private void Button_Open_from_a_file_Click(object sender, EventArgs e)
        {
            the_matrix = new double[Rows, Columns];
            The_Stream = null;
            OpenFileDialog The_OF_Dialog = new OpenFileDialog();
            The_OF_Dialog.Title = Writings.Open_text_file;
            The_OF_Dialog.Filter = "TXT files|*.txt";
            The_OF_Dialog.InitialDirectory = @"C:\";
            if (The_OF_Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    The_Stream = The_OF_Dialog.OpenFile();
                    if (The_Stream != null)
                    {
                        using (The_Stream)
                        {
                            the_matrix = Matrix_Library.Read_File_and_Save_Matrix(The_Stream, Rows, Columns);
                            if (the_matrix != null)
                            {
                                this.Refresh_the_matrix();
                            }
                        }
                    }
                }
                catch (Exception The_Exception)
                {
                    MessageBox.Show(Writings.Could_not_read_file + The_Exception.Message);
                }
            }
        }

        private void Button_MS_Next_Click(object sender, EventArgs e)
        {
            this.End_of_fields = false; // pruchod od zacatku
            this.Next_Click_IDentificator = 1;
            this.Fill_With_Zeros_Method();
            this.Remember_the_matrix();
        }

        private void Button_Back_Click(object sender, EventArgs e) //neni ready
        {
            this.Back_Click_IDentificator = 1; //musim to vynulovat pri navratu *** done
        }

        private void Button_Fill_With_Zeros_Click(object sender, EventArgs e)
        {
            Fill_With_Zeros_Method();
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            Erase_Everything();
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // stisknul jsem Enter
            {
                NumericUpDown Link = (NumericUpDown)sender;
                e.Handled = true;
                e.SuppressKeyPress = true;
                int i = 0, j = 0;
                while (!Field[i, j].Equals(Link)) // najit Control pri kterem jsem stisknul enter 
                {
                    if (!End_of_fields)
                    {
                        j++;
                    }
                    if (i < Rows - 1 && j == Columns)
                    {
                        i++;
                        j = 0;
                    }
                    if (i == Rows - 1 && j == Columns - 1)
                    {
                        End_of_fields = true;
                    }
                    else
                    {
                        End_of_fields = false;
                    }
                }


                if (!End_of_fields)
                {
                    j++;
                    if (i < Rows - 1 && j == Columns)
                    {
                        i++;
                        j = 0;
                    }
                    this.Field[i, j].Focus();
                }
                else
                {
                    this.MS_Next.Focus();
                    this.Fill_With_Zeros_Method();
                }

            }
        }
    }
}
