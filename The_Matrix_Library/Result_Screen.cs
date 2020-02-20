using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Matrix_Operations;

namespace Extended_Matrix_Calculator
{
    class Result_Screen : Panel
    {
        public Approximation_Graph The_Diagram;
        Label Title;
        Label Determinant_Lable;
        Label Eigenvectors_sign;
        Label Eigenvalues_sign;
        TextBox Additional_Text_Box;
        TextBox[,,] Result_Matrix;
        TextBox[] EigenValues;
        TextBox[,] EigenVectors;
        TextBox[] LSM_Scale_Info;
        public Label[] Matrix_ID;
        Label[,] Row_Labels;
        Label[,] Column_labels;
        Label[] EigenValues_Labels;
        Label[] Eigenvalue_Alg_Multiplicity;
        Label[] Eigenvalue_Geo_Multiplicity;
        Label[,] EigenVectors_Labels;
        Label[] Eigenvectors_ids;
        Label[] No_vectors_message;
        Label result_status;


        Button[] Save_as_buttons;
        Button[] Open_as_buttons;
        Button[] RS_Buttons;

        Pen pen1;

        public string[] more_names;
        public StreamWriter Files_S;

        public string Result_file_name = $@"{Environment.CurrentDirectory}\The_Result.txt";
        public StreamWriter Result_File_Writer;

        public string Partial_Result_file_name = $@"{Environment.CurrentDirectory}\The_Partial_Result.txt";
        //public StreamWriter Partial_Result_File_Writer;

        int RS_chosen_mode = -1;
        public int Number_of_Matrices = 0;
        int Res_number_of_rows = 0;
        int Res_number_of_columns = 0;
        double[,] Res_matrix1;
        double[,] Res_matrix2;
        double[,] Res_matrix3;
        int[] rows_arry;
        int[] columns_arry;

        double determinant_value = 0;

        int number_of_simulations = 100000;

        public int Small_Open_IDentificator = -1;
        public int Small_Save_as_IDentificator = -1;
        public int Small_Buttons_Memory = -1;
        public int Open_IDentificator = 0;
        public int Save_as_IDentificator = 0;
        public int Back_IDentificator = 0;
        public int Close_IDentificator = 0;

        int vertical_padding = 190;
        int horizontal_padding = 120;
        int additional_vertical_padding = 0;
        int last_horizontal;
        int eigen_vectors_last_horizontal;
        int[] last_vertical;

        Boolean file_created = false;

        Tuple<int, double[], int[]> EigValues_Final = new Tuple<int, double[], int[]>(0, null, null);
        Tuple<double[,], int[]> Eigenvectors = new Tuple<double[,], int[]>(null, null);
        Tuple<Boolean, double[,], double[,], double[,]> SVD_matrices = new Tuple<Boolean, double[,], double[,], double[,]>(false, null, null, null);

        //********************************** Help Functions and Constructors *********************************\\

        private void Create_Text_File(int matrix_number, int rows, int columns)
        {
            if (File.Exists(Result_file_name))
            {
                File.Delete(Result_file_name);
            }
            //Result_File_Stream = File.Create(Result_file_name);
            Result_File_Writer = File.CreateText(Result_file_name);
            if (matrix_number == 1)
            {
                using (Result_File_Writer)
                {
                    Result_File_Writer.WriteLine(Writings.The_result_is);
                    Result_File_Writer.WriteLine();
                    if (this.RS_chosen_mode == 9)
                    {
                        Result_File_Writer.WriteLine(Writings.Determinant + ": {0}", this.determinant_value);
                        Result_File_Writer.WriteLine();
                        Result_File_Writer.WriteLine(Writings.REF + ": ");
                    }
                    else if (this.RS_chosen_mode == 10)
                    {
                        Result_File_Writer.WriteLine(Writings.First_EigenVal + "{0}", this.Additional_Text_Box.Text);
                        Result_File_Writer.WriteLine();
                        Result_File_Writer.WriteLine(Writings.The_First_EigenVector);
                    }
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            Result_File_Writer.Write(Convert.ToString(Res_matrix1[i, j]) + "  ");
                        }
                        Result_File_Writer.WriteLine();
                    }
                }
            }
            else if (matrix_number > 1 && RS_chosen_mode != 14)
            {
                Result_File_Writer.WriteLine(Writings.The_result_is);
                Result_File_Writer.WriteLine();

                this.more_names = new string[matrix_number];
                for (int k = 0; k < matrix_number; k++)
                {
                    this.more_names[k] = $@"{Environment.CurrentDirectory}\Matrix{k}.txt";

                    if (File.Exists(more_names[k]))
                    {
                        File.Delete(more_names[k]);
                    }
                    using (Files_S = File.CreateText(more_names[k]))
                    {
                        Result_File_Writer.WriteLine(Writings.Matrix + " {0}:", Matrix_ID[k].Text);
                        Files_S.WriteLine(Writings.Matrix + " {0}:", Matrix_ID[k].Text);
                        Files_S.WriteLine();
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                switch (k)
                                {
                                    case 0:
                                        Files_S.Write(Convert.ToString(Res_matrix1[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(Res_matrix1[i, j]) + "  ");
                                        break;
                                    case 1:
                                        Files_S.Write(Convert.ToString(Res_matrix2[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(Res_matrix2[i, j]) + "  ");
                                        break;
                                    case 2:
                                        Files_S.Write(Convert.ToString(Res_matrix3[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(Res_matrix3[i, j]) + "  ");
                                        break;
                                }
                            }
                            Files_S.WriteLine();
                            Result_File_Writer.WriteLine();
                        }
                        Result_File_Writer.WriteLine();
                        Result_File_Writer.WriteLine();
                    }
                }
                Result_File_Writer.Close();
            }
            else
            {
                this.more_names = new string[matrix_number];

                Result_File_Writer.WriteLine(Writings.The_result_is);
                Result_File_Writer.WriteLine();
                for (int k = 0; k < matrix_number; k++)
                {
                    this.more_names[k] = $@"{Environment.CurrentDirectory}\SVD_Matrix{k}.txt";

                    if (File.Exists(more_names[k]))
                    {
                        File.Delete(more_names[k]);
                    }
                    using (Files_S = File.CreateText(more_names[k]))
                    {
                        Result_File_Writer.WriteLine(Writings.Matrix + " {0}:", Matrix_ID[k].Text);
                        Files_S.WriteLine(Writings.Matrix + " {0}:", Matrix_ID[k].Text);
                        Files_S.WriteLine();
                        for (int i = 0; i < rows_arry[k]; i++)
                        {
                            for (int j = 0; j < columns_arry[k]; j++)
                            {
                                switch (k)
                                {
                                    case 0:
                                        Files_S.Write(Convert.ToString(SVD_matrices.Item2[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(SVD_matrices.Item2[i, j]) + "  ");
                                        break;
                                    case 1:
                                        Files_S.Write(Convert.ToString(SVD_matrices.Item3[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(SVD_matrices.Item3[i, j]) + "  ");
                                        break;
                                    case 2:
                                        Files_S.Write(Convert.ToString(SVD_matrices.Item4[i, j]) + "  ");
                                        Result_File_Writer.Write(Convert.ToString(SVD_matrices.Item4[i, j]) + "  ");
                                        break;
                                }

                            }
                            Files_S.WriteLine();
                            Result_File_Writer.WriteLine();
                        }
                        Result_File_Writer.WriteLine();
                        Result_File_Writer.WriteLine();
                    }
                }
                Result_File_Writer.Close();
            }
        }

        private void Create_Text_File_EigenValues(int N, Tuple<int, double[], int[]> Values, Tuple<double[,], int[]> Vectors)
        {
            if (File.Exists(Result_file_name))
            {
                File.Delete(Result_file_name);
            }
            Result_File_Writer = File.CreateText(Result_file_name);
            int k = 0;
            using (Result_File_Writer)
            {
                Result_File_Writer.WriteLine(Writings.The_result_is);
                Result_File_Writer.WriteLine();
                for (int i = 0; i < Values.Item1; i++)
                {
                    Result_File_Writer.WriteLine();
                    Result_File_Writer.WriteLine(Writings.Eigenvalue + Convert.ToString(i) + " :  " + Convert.ToString(Values.Item2[i]));
                    Result_File_Writer.WriteLine(Writings.Algebraic_multiplicity + Convert.ToString(Values.Item3[i]));
                    Result_File_Writer.WriteLine(Writings.Geometric_multiplicity + Convert.ToString(Vectors.Item2[i]));
                    Result_File_Writer.WriteLine();

                    if (Vectors.Item2[i] != 0 && Vectors.Item1[k, N] == i)
                    {
                        for (int l = 0; l < Vectors.Item2[i]; l++) // nasobnost (aktualni vektory)
                        {
                            Result_File_Writer.WriteLine(Writings.Eigenvector + " " + Convert.ToString(l) + " :");

                            for (int m = 0; m < N; m++)
                            {
                                Result_File_Writer.Write(Convert.ToString(Vectors.Item1[k, m]) + " ");
                            }
                            if (k < Vectors.Item1.GetLength(0))
                            {
                                k++;
                            }
                            Result_File_Writer.WriteLine();
                        }
                        Result_File_Writer.WriteLine();
                    }
                    else
                    {
                        Result_File_Writer.WriteLine(Writings.No_eigenvectors);
                        Result_File_Writer.WriteLine();
                    }
                }

            }
        }


        private void Save_panel_as_Image(int width, int height, Panel The_Panel)
        {
            try
            {
                Bitmap The_Bitmap = new Bitmap(width, height);
                The_Panel.DrawToBitmap(The_Bitmap, new Rectangle(0, 0, width, height));
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = Writings.Save_as_Image;
                saveFileDialog1.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }
                    The_Bitmap.Save(saveFileDialog1.FileName);
                }
            }
            catch (Exception The_Exception)
            {
                MessageBox.Show(The_Exception.Message);
            }
        }


        // Result TextBox Initialize
        private void Result_Text_Box_Init(int width, int height, int loc_X, int loc_Y, int font_size, string text, ref TextBox the_box)
        {
            the_box = new TextBox();
            the_box.ReadOnly = true;
            the_box.Text = text;
            the_box.Font = new Font(the_box.Font.FontFamily, font_size);
            the_box.TextAlign = HorizontalAlignment.Center;
            the_box.Size = new Size(width, height);
            the_box.Margin = new Padding(10, 10, 10, 10);
            the_box.Location = new Point(loc_X, loc_Y);
            this.Controls.Add(the_box);
        }


        // Additional Button init
        private void Additional_Button_Init(int width, int height, int loc_X, int loc_Y, int font_size, string text, ref Button The_Button, int button_index)
        {
            The_Button = new Button();
            The_Button.Tag = button_index;
            The_Button.FlatStyle = FlatStyle.Popup;
            The_Button.Font = new Font(The_Button.Font.FontFamily, font_size);
            The_Button.ForeColor = Color.White;
            The_Button.BackColor = Color.FromArgb(15, 190, 255, 255);
            The_Button.Text = text;
            The_Button.Location = new Point(loc_X, loc_Y);
            The_Button.Size = new Size(width, height);
            this.Controls.Add(The_Button);
        }


        // Row/Column Label Init
        private void Label_Init(int width, int height, int loc_X, int loc_Y, int font_size, string text, ContentAlignment The_Alignment, Color Text_colour, ref Label The_Label)
        {
            The_Label = new Label();
            The_Label.Location = new Point(loc_X, loc_Y);
            The_Label.Size = new Size(width, height);
            The_Label.Font = new Font(The_Label.Font.FontFamily, font_size);
            The_Label.Text = text;
            The_Label.TextAlign = The_Alignment;
            The_Label.ForeColor = Text_colour;
            this.Controls.Add(The_Label);
        }

        // Basic Result Screen Initialize
        private void Basic_Res_Screen_Init() // + buttons init without location // add to form + location in the next 2 functions
        {
            this.Dock = DockStyle.Fill; //Panel se meni spolecne s formou (spolecne s rodicem)
            this.BackColor = Color.Black;
            this.AutoScroll = true;
            this.AllowDrop = false;

            this.Title = new Label();
            this.Title.ForeColor = Color.LawnGreen;
            this.Title.Font = new Font("Comic Sans MS", 25, FontStyle.Bold);
            this.Title.TextAlign = ContentAlignment.MiddleCenter;
            this.Title.Size = new Size(250, 80);
            this.Title.Text = Writings.Result_Title;

            RS_Buttons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                RS_Buttons[i] = new Button();
                RS_Buttons[i].FlatStyle = FlatStyle.Popup;
                RS_Buttons[i].Font = new Font(RS_Buttons[i].Font.FontFamily, 14);
                RS_Buttons[i].ForeColor = Color.LawnGreen;
                RS_Buttons[i].BackColor = Color.FromArgb(15, 170, 255, 255);
                RS_Buttons[i].Size = new Size(105, 60);
                RS_Buttons[i].TextAlign = ContentAlignment.MiddleCenter;
                //Open_As_Text_File.Location = new Point(last_horizontal + 150, last_vertical + 75);
                //this.Controls.Add(Open_As_Text_File);
                switch (i)
                {
                    case 0:
                        RS_Buttons[i].Text = Writings.Open_as_txt;
                        RS_Buttons[i].Click += Button_Open_Click;
                        break;
                    case 1:
                        RS_Buttons[i].Text = Writings.Save_all_as;
                        RS_Buttons[i].Click += Button_Save_as_Click;
                        break;
                    case 2:
                        RS_Buttons[i].Text = Writings.Back;
                        RS_Buttons[i].Click += Button_Back_Click;
                        break;
                    case 3:
                        RS_Buttons[i].Text = Writings.Close;
                        RS_Buttons[i].Click += Button_Close_Click;
                        break;
                }
            }
        }

        private void Matrix_Init(int number_of_matrices, int number_of_rows, int number_of_columns, double[,] matrix1, double[,] matrix2, double[,] matrix3)
        {
            this.last_vertical = new int[number_of_matrices + 1];
            last_vertical[0] = vertical_padding;
            if (number_of_columns <= 2)
            {
                horizontal_padding = horizontal_padding + 50;
            }
            this.Number_of_Matrices = number_of_matrices;

            this.last_horizontal = horizontal_padding + (number_of_columns - 1) * 100 / 2; // hledam stred, proto delim 2
            this.Title.Location = new Point(last_horizontal - Title.Width / 3 + 5, 25);

            this.Controls.Add(Title);

            if (number_of_matrices > 1)
            {
                this.Save_as_buttons = new Button[number_of_matrices];
                this.Open_as_buttons = new Button[number_of_matrices];
            }
            this.Matrix_ID = new Label[number_of_matrices];
            for (int i = 0; i < number_of_matrices; i++)
            {
                this.last_vertical[i + 1] = last_vertical[i] + number_of_rows * 45 + 135;
                this.Label_Init(50, 24, horizontal_padding - 35, last_vertical[i] - 38 + additional_vertical_padding, 12, "", ContentAlignment.MiddleLeft, Color.LawnGreen, ref Matrix_ID[i]);
                if (i == 0)
                {
                    if (number_of_matrices == 1 && RS_chosen_mode != 10 && RS_chosen_mode != 15)
                    {
                        this.Matrix_ID[i].Text = "A";
                    }
                    else if (number_of_matrices == 1 && RS_chosen_mode == 10)
                    {
                        this.Matrix_ID[i].Text = Writings.Vect;
                    }
                    else if (number_of_matrices == 1 && RS_chosen_mode == 15)
                    {
                        horizontal_padding += 65;
                        this.Matrix_ID[i].Text = Writings.Coefficient;
                        this.Matrix_ID[i].Width += 90;
                        this.Matrix_ID[i].Location = new Point(horizontal_padding - 140, last_vertical[i] + additional_vertical_padding);
                        this.Matrix_ID[i].Font = new Font(this.Matrix_ID[i].Font.FontFamily, 14);
                        this.Matrix_ID[i].ForeColor = Color.Yellow;
                    }
                    else if (this.RS_chosen_mode == 13)
                    {
                        this.Matrix_ID[i].Text = "S";
                    }
                    else if (this.RS_chosen_mode == 12)
                    {
                        this.Matrix_ID[i].Text = "L";
                    }
                }
                else if (i == 1)
                {
                    if (this.RS_chosen_mode == 13)
                    {
                        this.Matrix_ID[i].Font = new Font("Symbol", 12);
                        this.Matrix_ID[i].Text = "L";
                    }
                    else if (this.RS_chosen_mode == 12)
                    {
                        this.Matrix_ID[i].Text = "U";
                    }
                }
                else if (i == 2 && this.RS_chosen_mode == 13)
                {
                    this.Matrix_ID[i].Text = "S " + Writings.Inv;
                }
                this.Controls.Add(Matrix_ID[i]);

                if (number_of_matrices > 1)
                {
                    int horizontal_button_position = horizontal_padding - 34;
                    int vertical_button_position = last_vertical[i + 1] - 105 + additional_vertical_padding; ;
                    this.Additional_Button_Init(90, 27, horizontal_button_position, vertical_button_position, 10, Writings.Save_as, ref this.Save_as_buttons[i], i);
                    this.Save_as_buttons[i].Click += new EventHandler(Small_Button_Save_as_Click);

                    horizontal_button_position = horizontal_padding + 63;
                    this.Additional_Button_Init(100, 27, horizontal_button_position, vertical_button_position, 10, Writings.Open_as_txt, ref this.Open_as_buttons[i], i);
                    this.Open_as_buttons[i].Click += Small_Button_Open_Click;
                }
            }

            Result_Matrix = new TextBox[number_of_matrices, number_of_rows, number_of_columns];

            string the_text = "";

            try
            {
                for (int k = 0; k < number_of_matrices; k++) // jednotlive matice, max. pocet: 3
                {
                    for (int i = 0; i < number_of_rows; i++)
                    {
                        for (int j = 0; j < number_of_columns; j++)
                        {
                            if (k == 0 && matrix1 != null)
                            {
                                the_text = Convert.ToString(matrix1[i, j]);
                            }
                            else if (k == 1 && matrix2 != null)
                            {
                                the_text = Convert.ToString(matrix2[i, j]);
                            }
                            else if (k == 2 && matrix3 != null)
                            {
                                the_text = Convert.ToString(matrix3[i, j]);
                            }

                            Result_Text_Box_Init(85, 70, horizontal_padding + j * 100, last_vertical[k] + i * 45 + additional_vertical_padding, 13, the_text, ref Result_Matrix[k, i, j]);
                        }
                    }
                }
            }
            catch (Exception The_Exception)
            {
                MessageBox.Show(Writings.Null_matrix + The_Exception.Message);
            }

            int counter;
            int label_horizontal = 0;
            int label_vertical = 0;
            string the_number = "";

            if (RS_chosen_mode != 15)
            {
                Row_Labels = new Label[number_of_matrices, 2 * number_of_rows];
                Column_labels = new Label[number_of_matrices, 2 * number_of_columns];

                for (int k = 0; k < number_of_matrices; k++) // row labels
                {
                    counter = 0;
                    for (int i = 0; i < 2 * number_of_rows; i++)
                    {
                        if (i < number_of_rows)
                        {
                            label_horizontal = horizontal_padding - 40;
                            label_vertical = last_vertical[k] + i * 45 + additional_vertical_padding;
                            the_number = Convert.ToString(counter);
                            counter++;
                        }
                        else
                        {
                            label_horizontal = horizontal_padding + number_of_columns * 100;
                            label_vertical = last_vertical[k] + (i - number_of_rows) * 45 + additional_vertical_padding;
                            if (i == number_of_rows)
                            {
                                counter = 0;
                            }
                            the_number = Convert.ToString(counter);
                            counter++;
                        }

                        Label_Init(30, 27, label_horizontal, label_vertical, 9, the_number, ContentAlignment.MiddleCenter, Color.Gray, ref Row_Labels[k, i]);
                    }
                }
            }
            else
            {
                Column_labels = new Label[number_of_matrices, number_of_columns];
            }

            int font = 9;
            Boolean stop = false;
            for (int k = 0; k < number_of_matrices; k++) // column labels
            {
                counter = 0;
                for (int i = 0; i < 2 * number_of_columns && !stop; i++)
                {
                    if (i < number_of_columns)
                    {
                        label_horizontal = horizontal_padding + i * 100;
                        label_vertical = last_vertical[k] - 35 + additional_vertical_padding;
                        if (RS_chosen_mode != 15)
                        {
                            the_number = Convert.ToString(counter);
                            counter++;
                        }
                        else
                        {
                            font = 12;
                            switch (i)
                            {
                                case 0:
                                    the_number = "a";
                                    break;
                                case 1:
                                    the_number = "b";
                                    break;
                                case 2:
                                    the_number = "c";
                                    stop = true;
                                    vertical_padding -= 45;
                                    break;
                            }
                        }
                    }
                    else if (RS_chosen_mode != 15)
                    {
                        font = 9;
                        label_horizontal = horizontal_padding + (i - number_of_columns) * 100;
                        label_vertical = last_vertical[k + 1] - 145 + additional_vertical_padding;
                        if (i == number_of_columns)
                        {
                            counter = 0;
                        }
                        the_number = Convert.ToString(counter);
                        counter++;
                    }

                    Label_Init(85, 27, label_horizontal, label_vertical, font, the_number, ContentAlignment.MiddleCenter, Color.Gray, ref Column_labels[k, i]);
                }
            }
        }


        private void Eigenvectors_and_Values_Init(int N, Tuple<int, double[], int[]> Values, Tuple<double[,], int[]> Vectors)
        {
            int last_vertical_position = 0;
            int values_vertical_padding = 240;
            int values_horizontal_padding = 120;
            int eigenvectors_vertical_padding = 0;
            int eigenvectors_horizontal_padding = 0;
            this.EigenValues = new TextBox[Values.Item1];
            this.EigenVectors = new TextBox[Vectors.Item1.GetLength(0), Vectors.Item1.GetLength(1)];
            this.Eigenvectors_ids = new Label[N];
            this.EigenValues_Labels = new Label[Values.Item1];
            this.No_vectors_message = new Label[Values.Item1];
            this.Eigenvalue_Alg_Multiplicity = new Label[Values.Item1];
            this.Eigenvalue_Geo_Multiplicity = new Label[Values.Item1];
            this.EigenVectors_Labels = new Label[Vectors.Item1.GetLength(0), Vectors.Item1.GetLength(1)];

            this.Eigenvalues_sign = new Label();
            this.Eigenvalues_sign.ForeColor = Color.LightCoral;
            this.Eigenvalues_sign.Text = Writings.Eigenvalues;
            this.Eigenvalues_sign.Font = new Font(Eigenvalues_sign.Font.FontFamily, 17);
            this.Eigenvalues_sign.Size = new Size(270, 35);
            this.Eigenvalues_sign.Location = new Point(values_horizontal_padding, values_vertical_padding - 90);
            this.Controls.Add(Eigenvalues_sign);

            this.Eigenvectors_sign = new Label();
            this.Eigenvectors_sign.ForeColor = Color.LightCoral;
            this.Eigenvectors_sign.Text = Writings.Eigenvectors;
            this.Eigenvectors_sign.Font = new Font(Eigenvectors_sign.Font.FontFamily, 17);
            this.Eigenvectors_sign.Size = new Size(270, 35);
            this.Eigenvectors_sign.Location = new Point(values_horizontal_padding + 150 + 170 - 1, values_vertical_padding - 90);
            this.Controls.Add(Eigenvectors_sign);

            int k = 0;

            for (int i = 0; i < Values.Item1; i++)
            {
                EigenValues_Labels[i] = new Label();
                EigenValues_Labels[i].ForeColor = Color.Yellow;
                EigenValues_Labels[i].Text = Writings.Eigenvalue + Convert.ToString(i) + " :";
                EigenValues_Labels[i].Font = new Font(EigenValues_Labels[i].Font.FontFamily, 13);
                EigenValues_Labels[i].Size = new Size(145, 26);
                EigenValues_Labels[i].Location = new Point(values_horizontal_padding, values_vertical_padding + 2);
                this.Controls.Add(EigenValues_Labels[i]);

                Eigenvalue_Alg_Multiplicity[i] = new Label();
                Eigenvalue_Alg_Multiplicity[i].ForeColor = Color.White;
                Eigenvalue_Alg_Multiplicity[i].Text = Writings.Algebraic_multiplicity + Convert.ToString(Values.Item3[i]);
                Eigenvalue_Alg_Multiplicity[i].Font = new Font(Eigenvalue_Alg_Multiplicity[i].Font.FontFamily, 11);
                Eigenvalue_Alg_Multiplicity[i].Size = new Size(210, 26);
                Eigenvalue_Alg_Multiplicity[i].Location = new Point(values_horizontal_padding, values_vertical_padding + 56);
                this.Controls.Add(Eigenvalue_Alg_Multiplicity[i]);

                Eigenvalue_Geo_Multiplicity[i] = new Label();
                Eigenvalue_Geo_Multiplicity[i].ForeColor = Color.White;
                Eigenvalue_Geo_Multiplicity[i].Text = Writings.Geometric_multiplicity + Convert.ToString(Vectors.Item2[i]);
                Eigenvalue_Geo_Multiplicity[i].Font = new Font(Eigenvalue_Geo_Multiplicity[i].Font.FontFamily, 11);
                Eigenvalue_Geo_Multiplicity[i].Size = new Size(210, 26);
                Eigenvalue_Geo_Multiplicity[i].Location = new Point(values_horizontal_padding, values_vertical_padding + 90);
                this.Controls.Add(Eigenvalue_Geo_Multiplicity[i]);

                EigenValues[i] = new TextBox();
                EigenValues[i].ReadOnly = true;
                EigenValues[i].Text = Convert.ToString(Values.Item2[i]);
                EigenValues[i].Font = new Font(EigenValues[i].Font.FontFamily, 14);
                EigenValues[i].TextAlign = HorizontalAlignment.Center;
                EigenValues[i].Size = new Size(85, 40);
                EigenValues[i].Margin = new Padding(10, 10, 10, 10);
                EigenValues[i].Location = new Point(EigenValues_Labels[i].Left + 150, values_vertical_padding);
                this.Controls.Add(EigenValues[i]);

                eigenvectors_vertical_padding = 0;
                eigenvectors_horizontal_padding = EigenValues_Labels[i].Left + 150 + 170;

                if (Vectors.Item2[i] != 0 && Vectors.Item1[k, N] == i)
                {
                    for (int l = 0; l < Vectors.Item2[i]; l++) // nasobnost (aktualni vektory)
                    {
                        this.Eigenvectors_ids[l] = new Label();
                        this.Eigenvectors_ids[l].ForeColor = Color.Gray;
                        this.Eigenvectors_ids[l].Text = Convert.ToString(l);
                        this.Eigenvectors_ids[l].TextAlign = ContentAlignment.MiddleCenter;
                        this.Eigenvectors_ids[l].Font = new Font(Eigenvectors_ids[l].Font.FontFamily, 10);
                        this.Eigenvectors_ids[l].Size = new Size(20, 20);
                        this.Eigenvectors_ids[l].Location = new Point(eigenvectors_horizontal_padding + 33, values_vertical_padding - 35);
                        this.Controls.Add(Eigenvectors_ids[l]);

                        for (int m = 0; m < N; m++)
                        {
                            EigenVectors[k, m] = new TextBox();
                            EigenVectors[k, m].ReadOnly = true;
                            EigenVectors[k, m].Text = Convert.ToString(Vectors.Item1[k, m]);
                            EigenVectors[k, m].Font = new Font(EigenVectors[k, m].Font.FontFamily, 14);
                            EigenVectors[k, m].TextAlign = HorizontalAlignment.Center;
                            EigenVectors[k, m].Size = new Size(85, 40);
                            EigenVectors[k, m].Margin = new Padding(10, 10, 10, 10);
                            EigenVectors[k, m].Location = new Point(eigenvectors_horizontal_padding, values_vertical_padding + eigenvectors_vertical_padding);
                            last_vertical_position = values_vertical_padding + eigenvectors_vertical_padding;
                            this.Controls.Add(EigenVectors[k, m]);
                            eigenvectors_vertical_padding += 45;
                        }
                        if (k < Vectors.Item1.GetLength(0))
                        {
                            k++;
                        }
                        eigenvectors_vertical_padding = 0;
                        eigenvectors_horizontal_padding += 140;
                        if (eigenvectors_horizontal_padding > eigen_vectors_last_horizontal)
                        {
                            eigen_vectors_last_horizontal = eigenvectors_horizontal_padding;
                        }
                    }
                }
                else
                {
                    No_vectors_message[i] = new Label();
                    No_vectors_message[i].ForeColor = Color.White;
                    No_vectors_message[i].Text = Writings.No_eigenvectors;
                    No_vectors_message[i].Font = new Font(No_vectors_message[i].Font.FontFamily, 12);
                    No_vectors_message[i].Size = new Size(275, 65);
                    No_vectors_message[i].Location = new Point(eigenvectors_horizontal_padding, values_vertical_padding);
                    this.Controls.Add(No_vectors_message[i]);
                    last_vertical_position = Eigenvalue_Geo_Multiplicity[i].Location.Y + 10;
                    if (eigenvectors_horizontal_padding > eigen_vectors_last_horizontal)
                    {
                        eigen_vectors_last_horizontal = eigenvectors_horizontal_padding + No_vectors_message[i].Size.Width;
                    } // result title position to potrebuje
                }

                values_vertical_padding = last_vertical_position + 120;
            }
            additional_vertical_padding = last_vertical_position;
            if (eigen_vectors_last_horizontal < Eigenvectors_sign.Location.X + Eigenvectors_sign.Width)
            {
                eigen_vectors_last_horizontal += 30;
            }

            //last_horizontal = eigen_vectors_last_horizontal - 290;
            this.Title.Location = new Point(eigen_vectors_last_horizontal / 2 - Title.Width / 5 - 10, 25);
            this.Controls.Add(Title);
        }

        private void SVD_matrices_Init(Tuple<Boolean, double[,], double[,], double[,]> SVD_matrices)
        {
            int maximal_number_of_columns = 0;
            int maximal_number_of_rows = 0;
            this.Number_of_Matrices = 3;

            rows_arry = new int[] { SVD_matrices.Item2.GetLength(0), SVD_matrices.Item3.GetLength(0), SVD_matrices.Item4.GetLength(0) };
            columns_arry = new int[] { SVD_matrices.Item2.GetLength(1), SVD_matrices.Item3.GetLength(1), SVD_matrices.Item4.GetLength(1) };
            this.last_vertical = new int[Number_of_Matrices + 1];
            last_vertical[0] = vertical_padding; // prvni prvek je jenom ten padding, vyuziva se to pri rozmistovani matic po panelu
            TextBox[,] the_U_matrix = new TextBox[rows_arry[0], columns_arry[0]];
            TextBox[,] the_Sigma_matrix = new TextBox[rows_arry[1], columns_arry[1]];
            TextBox[,] the_Vtr_matrix = new TextBox[rows_arry[2], columns_arry[2]];

            maximal_number_of_columns = columns_arry.Max();
            maximal_number_of_rows = rows_arry.Max();

            if (maximal_number_of_columns <= 2 && maximal_number_of_rows <= 2)
            {
                horizontal_padding = horizontal_padding + 50;
            }
            else
            {
                horizontal_padding = 120;
                result_status.Location = new Point(horizontal_padding - 35, vertical_padding - 95);
            }

            this.last_horizontal = horizontal_padding + (maximal_number_of_columns - 1) * 100 / 2; // hledam stred, proto delim 2
            this.Title.Location = new Point(last_horizontal - Title.Width / 3 + 5, 25);
            this.Controls.Add(Title);

            this.Save_as_buttons = new Button[Number_of_Matrices];
            this.Open_as_buttons = new Button[Number_of_Matrices];

            this.Matrix_ID = new Label[Number_of_Matrices];

            for (int i = 0; i < Number_of_Matrices; i++)
            {

                last_vertical[i + 1] = last_vertical[i] + rows_arry[i] * 45 + 120;

                int additional_buttons_vertical = last_vertical[i + 1] - 95 + additional_vertical_padding;
                this.Additional_Button_Init(90, 27, horizontal_padding, additional_buttons_vertical, 10, Writings.Save_as, ref Save_as_buttons[i], i);
                Save_as_buttons[i].Click += Small_Button_Save_as_Click;

                this.Additional_Button_Init(100, 27, horizontal_padding + 100, additional_buttons_vertical, 10, Writings.Open_as_txt, ref Open_as_buttons[i], i);
                Open_as_buttons[i].Click += Small_Button_Open_Click;

                this.Matrix_ID[i] = new Label();
                this.Matrix_ID[i].Font = new Font(this.Matrix_ID[i].Font.FontFamily, 12);
                this.Matrix_ID[i].ForeColor = Color.LawnGreen;
                this.Matrix_ID[i].Location = new Point(horizontal_padding - 35, last_vertical[i] - 36 + additional_vertical_padding);
                this.Matrix_ID[i].Size = new Size(50, 26);
                if (i == 0)
                {
                    this.Matrix_ID[i].Text = "U";

                }
                else if (i == 1)
                {
                    this.Matrix_ID[i].Font = new Font("Symbol", 12);
                    this.Matrix_ID[i].Text = "S";
                }
                else if (i == 2)
                {
                    this.Matrix_ID[i].Text = "V " + Writings.Tr;
                }
                this.Controls.Add(Matrix_ID[i]);
            }

            int text_box_width = 85;
            int text_box_height = 70;
            string the_text = "";
            int location_X = 0;
            int location_Y = 0;
            for (int k = 0; k < Number_of_Matrices; k++) // jednotlive matice
            {
                for (int i = 0; i < rows_arry[k]; i++)
                {
                    for (int j = 0; j < columns_arry[k]; j++)
                    {
                        location_X = horizontal_padding + j * 100;
                        location_Y = last_vertical[k] + i * 45 + additional_vertical_padding;
                        if (k == 0 && SVD_matrices.Item2 != null)
                        {
                            the_text = Convert.ToString(SVD_matrices.Item2[i, j]);
                            Result_Text_Box_Init(text_box_width, text_box_height, location_X, location_Y, 13, the_text, ref the_U_matrix[i, j]);
                        }
                        else if (k == 1 && SVD_matrices.Item3 != null)
                        {
                            the_text = Convert.ToString(SVD_matrices.Item3[i, j]);
                            Result_Text_Box_Init(text_box_width, text_box_height, location_X, location_Y, 13, the_text, ref the_Sigma_matrix[i, j]);
                        }
                        else if (k == 2 && SVD_matrices.Item4 != null)
                        {
                            the_text = Convert.ToString(SVD_matrices.Item4[i, j]);
                            Result_Text_Box_Init(text_box_width, text_box_height, location_X, location_Y, 13, the_text, ref the_Vtr_matrix[i, j]);
                        }
                    }
                }
            }


            int counter;
            int label_horizontal = 0;
            int label_vertical = 0;
            string the_number = "";

            for (int k = 0; k < Number_of_Matrices; k++)
            {
                Row_Labels = new Label[1, 2 * rows_arry[k]];
                counter = 0;
                for (int i = 0; i < 2 * rows_arry[k]; i++)
                {
                    if (i < rows_arry[k])
                    {
                        label_horizontal = horizontal_padding - 40;
                        label_vertical = last_vertical[k] + i * 45 + additional_vertical_padding;
                        the_number = Convert.ToString(counter);
                        counter++;
                    }
                    else
                    {
                        label_horizontal = horizontal_padding + columns_arry[k] * 100;
                        label_vertical = last_vertical[k] + (i - rows_arry[k]) * 45 + additional_vertical_padding;
                        if (i == rows_arry[k])
                        {
                            counter = 0;
                        }
                        the_number = Convert.ToString(counter);
                        counter++;
                    }

                    Label_Init(30, 27, label_horizontal, label_vertical, 9, the_number, ContentAlignment.MiddleCenter, Color.Gray, ref Row_Labels[0, i]);

                }
            }


            for (int k = 0; k < Number_of_Matrices; k++)
            {
                Column_labels = new Label[1, 2 * columns_arry[k]];
                counter = 0;
                for (int i = 0; i < 2 * columns_arry[k]; i++)
                {
                    if (i < columns_arry[k])
                    {
                        label_horizontal = horizontal_padding + i * 100;
                        label_vertical = last_vertical[k] - 35 + additional_vertical_padding;
                        the_number = Convert.ToString(counter);
                        counter++;
                    }
                    else
                    {
                        label_horizontal = horizontal_padding + (i - columns_arry[k]) * 100;
                        label_vertical = last_vertical[k + 1] - 135 + additional_vertical_padding;
                        if (i == columns_arry[k])
                        {
                            counter = 0;
                        }
                        the_number = Convert.ToString(counter);
                        counter++;
                    }

                    Label_Init(85, 27, label_horizontal, label_vertical, 9, the_number, ContentAlignment.MiddleCenter, Color.Gray, ref Column_labels[0, i]);
                }
            }


        }

        private void Scale_Info_Init(int item_count, TextBox[] Scale_Information)
        {
            this.LSM_Scale_Info = new TextBox[item_count];
            for (int i = 0; i < item_count; i++)
            {
                this.LSM_Scale_Info[i] = Scale_Information[i];
            }
        }

        //*************************************************** Constructors **************************************************\\

        public Result_Screen(int number_of_rows, int number_of_columns, double[,] the_original_matrix, int Mode, int accuracy, TextBox[] the_scale_info)
        {
            this.RS_chosen_mode = Mode;
            this.Basic_Res_Screen_Init();
            if ((number_of_columns <= 2 && (Mode != 5 || Mode != 10)) || (Mode == 5 && number_of_rows <= 2)) // 5: Transpose
            {
                horizontal_padding = 300;
            }

            if (Mode >= 5 && Mode < 10)
            {
                if (Mode == 5)
                {
                    Res_number_of_rows = number_of_columns;
                    Res_number_of_columns = number_of_rows;
                    Res_matrix1 = new double[Res_number_of_rows, Res_number_of_columns];
                    Res_matrix1 = Matrix_Library.Transposition(number_of_rows, number_of_columns, the_original_matrix, accuracy);
                }
                else if (Mode == 6 || Mode == 7 || Mode == 8 || Mode == 9)
                {
                    Res_number_of_rows = number_of_rows;
                    Res_number_of_columns = number_of_columns;
                    Res_matrix1 = new double[Res_number_of_rows, Res_number_of_columns];
                    if (Mode == 6)
                    {
                        Res_matrix1 = Matrix_Library.REF(number_of_rows, number_of_columns, the_original_matrix, accuracy);
                    }
                    else if (Mode == 7)
                    {
                        Res_matrix1 = Matrix_Library.RREF(number_of_rows, number_of_columns, the_original_matrix, false, accuracy);
                    }
                    else if (Mode == 8)
                    {
                        var Inverse_Result = Matrix_Library.Inverse(number_of_rows, number_of_columns, the_original_matrix, accuracy);
                        Res_matrix1 = Inverse_Result.Item2;
                        if (!Inverse_Result.Item1)
                        {
                            int res_status_horizontal = horizontal_padding - 35;
                            if (horizontal_padding == 300)
                            {
                                res_status_horizontal += 15;
                            }
                            this.result_status = new Label();
                            result_status.Font = new Font(result_status.Font.FontFamily, 12);
                            result_status.Location = new Point(res_status_horizontal, 155);
                            result_status.Size = new Size(550, 26);
                            Title.ForeColor = Color.Orange;
                            result_status.ForeColor = Color.Orange;
                            result_status.Text = Writings.Inverse_not_successful;
                            this.Controls.Add(result_status);
                            vertical_padding += 40;
                        }
                    }
                    else if (Mode == 9)
                    {
                        double the_determinant = 0;
                        var Tuple_Det_Res_matrix = new Tuple<double, double[,]>(the_determinant, Res_matrix1);
                        Tuple_Det_Res_matrix = Matrix_Library.Determinant(number_of_rows, number_of_columns, the_original_matrix, accuracy);
                        vertical_padding += 50;

                        Matrix_Init(1, Res_number_of_rows, Res_number_of_columns, Tuple_Det_Res_matrix.Item2, null, null);

                        this.determinant_value = Tuple_Det_Res_matrix.Item1;
                        this.Res_matrix1 = Tuple_Det_Res_matrix.Item2;

                        this.Label_Init(135, 30, this.horizontal_padding - 48, this.Matrix_ID[0].Top - 60, 14, Writings.Determinant + " :", ContentAlignment.MiddleLeft, Color.Yellow, ref Determinant_Lable);
                        this.Result_Text_Box_Init(85, 30, this.Matrix_ID[0].Left + 135, this.Determinant_Lable.Top + 2, 14, Convert.ToString(this.determinant_value), ref Additional_Text_Box);

                        this.Matrix_ID[0].Text = Writings.REF + ":";
                        this.Matrix_ID[0].Left -= 13;

                    }
                }

                if (Mode != 9) // u modu 9 matice se inicializuje v predchozim if modulu
                {
                    Matrix_Init(1, Res_number_of_rows, Res_number_of_columns, Res_matrix1, null, null);
                }

                if (Mode == 7) // u RREF je zapotrebi nakreslit caru a posunout nektera policka a labely o trosicku vpravo
                {
                    for (int i = number_of_rows; i < 2 * number_of_rows; i++)
                    {
                        this.Result_Matrix[0, i - number_of_rows, number_of_columns - 1].Left += 30;
                        this.Row_Labels[0, i].Left += 30;
                    }
                    this.Column_labels[0, number_of_columns - 1].Left += 30;
                    this.Column_labels[0, number_of_columns - 1].Text += " (b)";
                    this.Column_labels[0, 2 * number_of_columns - 1].Left += 30;
                    this.Column_labels[0, 2 * number_of_columns - 1].Text += " (b)";
                    this.Paint += Draw_RREF_Line;
                    this.Refresh();
                }
            }
            else if (Mode == 15)
            {
                horizontal_padding = 270;
                int mx_columns = 3;
                int vector_rows = 1;
                Res_number_of_rows = vector_rows;
                Res_number_of_columns = mx_columns;
                Res_matrix1 = new double[vector_rows, mx_columns];
                double[] Res_vector = new double[mx_columns];
                double[,] Sorted_Data = new double[number_of_rows, number_of_columns];
                Tuple<double[], double[,]> The_LSM_Results = Matrix_Library.LSM(number_of_rows, number_of_columns, the_original_matrix, mx_columns, accuracy);

                Res_vector = The_LSM_Results.Item1;
                Res_matrix1 = Matrix_Library.Vector_to_1_row_matrix(mx_columns, Res_vector);
                Matrix_Init(1, vector_rows, mx_columns, Res_matrix1, null, null);

                this.The_Diagram = new Approximation_Graph(785, 500, 30, last_vertical[Number_of_Matrices] + 100, number_of_rows, The_LSM_Results.Item2);
                this.Controls.Add(The_Diagram);
                this.Scale_Info_Init(2, the_scale_info);
                this.The_Diagram.Set_Graph(number_of_rows, The_LSM_Results.Item2, The_LSM_Results.Item1, LSM_Scale_Info);
                this.Save_as_buttons = new Button[1];
                this.Additional_Button_Init(175, 40, 30, The_Diagram.Location.Y + 520, 9, Writings.Save_as_Image, ref Save_as_buttons[0], 100);
                this.Save_as_buttons[0].Click += new EventHandler(Save_panel_as_Image_Click);
            }
            else if (Mode >= 10)
            {
                int number_of_comparisons = 15;
                if (accuracy < 8)
                {
                    this.number_of_simulations = 320000;
                }
                else if (accuracy == 15)
                {
                    this.number_of_simulations = this.number_of_simulations * 30; // number_of_simulations == 100 000
                    number_of_comparisons = 10;
                }
                else
                {
                    this.number_of_simulations = (accuracy + 1) * 40000;
                }

                int res_status_horizontal = horizontal_padding - 35;
                if (horizontal_padding == 300 && RS_chosen_mode != 14 && RS_chosen_mode != 13)
                {
                    res_status_horizontal -= 95;
                }
                else if (horizontal_padding == 300 && RS_chosen_mode == 13)
                {
                    res_status_horizontal += 50;
                }
                this.result_status = new Label();
                result_status.Font = new Font(result_status.Font.FontFamily, 12);
                result_status.Location = new Point(res_status_horizontal, 155);
                result_status.Size = new Size(550, 26);

                if (Mode == 10)
                {
                    horizontal_padding = 120;
                    Res_number_of_rows = 1;
                    Res_number_of_columns = number_of_rows; // ctvercova matice: number_of_rows = number_of_columns
                    Res_matrix1 = new double[Res_number_of_rows, Res_number_of_columns];
                    var Tuple_EigenValue_EigenVector = new Tuple<double, double[]>(0, null);
                    Tuple_EigenValue_EigenVector = Matrix_Library.Power_Iteration(number_of_rows, number_of_columns, the_original_matrix, number_of_simulations, accuracy);
                    this.Res_matrix1 = Matrix_Library.Vector_to_1_row_matrix(Res_number_of_columns, Tuple_EigenValue_EigenVector.Item2);

                    this.Label_Init(215, 36, horizontal_padding - 35, vertical_padding - 50, 15, Writings.First_EigenVal, ContentAlignment.MiddleLeft, Color.Yellow, ref Eigenvalues_sign);
                    this.Result_Text_Box_Init(85, 70, horizontal_padding + 190, vertical_padding - 45, 14, Convert.ToString(Tuple_EigenValue_EigenVector.Item1), ref this.Additional_Text_Box);
                    vertical_padding += 60;
                    Matrix_Init(1, Res_number_of_rows, Res_number_of_columns, Res_matrix1, null, null);
                    last_vertical[1] -= 50;
                }
                else if (Mode == 11 || Mode == 13)
                {
                    Res_number_of_columns = number_of_rows;
                    Res_number_of_rows = number_of_columns;

                    EigValues_Final = Matrix_Library.Find_Eigenvalues_Everything_Together(number_of_rows, the_original_matrix, number_of_simulations, accuracy, number_of_comparisons);
                    Eigenvectors = Matrix_Library.Find_All_EigenVectors(number_of_rows, the_original_matrix, EigValues_Final, 7);

                    if (Mode == 13)
                    {
                        Res_matrix1 = new double[Res_number_of_rows, Res_number_of_columns];
                        Res_matrix2 = new double[Res_number_of_rows, Res_number_of_columns];
                        Res_matrix3 = new double[Res_number_of_rows, Res_number_of_columns];
                        Tuple<Boolean, double[,], double[,], double[,]> EigVectors_Matrices = new Tuple<Boolean, double[,], double[,], double[,]>(false, null, null, null);
                        EigVectors_Matrices = Matrix_Library.Spectral_Decomposition(number_of_rows, Eigenvectors.Item1, EigValues_Final, accuracy);
                        this.Res_matrix1 = EigVectors_Matrices.Item2;
                        this.Res_matrix2 = EigVectors_Matrices.Item3;
                        this.Res_matrix3 = EigVectors_Matrices.Item4;

                        Boolean Spectral_successful = EigVectors_Matrices.Item1;
                        if (Spectral_successful)
                        {
                            Spectral_successful = Matrix_Library.Spectral_Decomposition_Check(Res_number_of_rows, the_original_matrix, Res_matrix1, Res_matrix2, Res_matrix3);
                        }
                        if (!Spectral_successful)
                        {
                            Title.ForeColor = Color.Orange;
                            result_status.ForeColor = Color.Orange;
                            result_status.Text = Writings.Spectral_decomposition_unsuccessful;
                        }
                        else
                        {
                            result_status.ForeColor = Color.LawnGreen;
                            result_status.Text = Writings.Spectral_decomposition_successful;
                        }
                        this.Controls.Add(result_status);
                        vertical_padding += 60;

                        this.Matrix_Init(3, number_of_rows, number_of_columns, Res_matrix1, Res_matrix2, Res_matrix3);
                    }
                    else
                    {
                        this.Eigenvectors_and_Values_Init(number_of_rows, EigValues_Final, Eigenvectors);
                    }
                }
                else if (Mode == 12)
                {
                    Res_number_of_columns = number_of_rows;
                    Res_number_of_rows = number_of_columns;
                    Res_matrix1 = new double[Res_number_of_rows, Res_number_of_columns];
                    Res_matrix2 = new double[Res_number_of_rows, Res_number_of_columns];
                    Tuple<Boolean, Boolean, double[,], double[,]> Cholesky_result = Matrix_Library.Cholesky(number_of_rows, the_original_matrix, accuracy);

                    Res_matrix1 = Cholesky_result.Item3;
                    Res_matrix2 = Cholesky_result.Item4;

                    if (!Cholesky_result.Item1 || !Cholesky_result.Item2)
                    {
                        Title.ForeColor = Color.Orange;
                        result_status.ForeColor = Color.Orange;
                        if (!Cholesky_result.Item1)
                        {
                            result_status.Text += Writings.Cholesky_Mx_not_positive_definite;
                        }
                        result_status.Text = Writings.Cholesky_failed;
                    }
                    else
                    {
                        result_status.ForeColor = Color.LawnGreen;
                        result_status.Text = Writings.Cholesky_successful;
                    }
                    this.Controls.Add(result_status);
                    vertical_padding += 60;

                    Matrix_Init(2, number_of_rows, number_of_columns, Res_matrix1, Res_matrix2, null);

                }
                else if (Mode == 14)
                {
                    vertical_padding += 65;
                    SVD_matrices = Matrix_Library.SVD(number_of_rows, number_of_columns, the_original_matrix, number_of_simulations, accuracy, number_of_comparisons);

                    if (!SVD_matrices.Item1)
                    {
                        Title.ForeColor = Color.Orange;
                        result_status.ForeColor = Color.Orange;
                        result_status.Text = Writings.SVD_unsuccessful;
                    }
                    else
                    {
                        result_status.ForeColor = Color.LawnGreen;
                        result_status.Text = Writings.SVD_successful;
                    }
                    this.Controls.Add(result_status);

                    SVD_matrices_Init(SVD_matrices);
                }

            }

            if (Mode != 11)
            {
                for (int i = 0; i < 4; i++) // Buttons
                {
                    this.RS_Buttons[i].Location = new Point(last_horizontal - 180 - 7 + i * 120 + eigen_vectors_last_horizontal / 2, last_vertical[Number_of_Matrices] - 40 + additional_vertical_padding);
                    this.Controls.Add(RS_Buttons[i]);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++) // Buttons
                {
                    this.RS_Buttons[i].Location = new Point(last_horizontal - 180 - 7 + i * 120 + eigen_vectors_last_horizontal / 2, vertical_padding - 100 + additional_vertical_padding);
                    this.Controls.Add(RS_Buttons[i]);
                }
            }

        }


        public Result_Screen(int rows_1, int columns_1, double[,] matrix_1, int rows_2, int columns_2, double[,] matrix_2, int Chosen_Mode, int accuracy)
        {
            this.RS_chosen_mode = Chosen_Mode;
            Res_matrix1 = new double[rows_1, columns_2];
            this.Basic_Res_Screen_Init();
            if (columns_2 <= 2)
            {
                horizontal_padding = 300;
            }
            if (Chosen_Mode == 2)
            {
                Res_matrix1 = Matrix_Library.Sum_Two_Matrices(rows_1, columns_2, matrix_1, matrix_2, accuracy);
                Res_number_of_rows = rows_1;
                Res_number_of_columns = columns_1;
            }
            else if (Chosen_Mode == 3)
            {
                Res_matrix1 = Matrix_Library.Subtract_Two_Matrices(rows_1, columns_2, matrix_1, matrix_2, accuracy);
                Res_number_of_rows = rows_1;
                Res_number_of_columns = columns_1;
            }
            else if (Chosen_Mode == 4)
            {
                Res_matrix1 = Matrix_Library.Matrix_Multiplication(rows_1, columns_1, matrix_1, rows_2, columns_2, matrix_2, accuracy);
                Res_number_of_rows = rows_1;
                Res_number_of_columns = columns_2;
            }
            //MessageBox.Show(Convert.ToString(Res_number_of_rows) + Convert.ToString(Res_number_of_columns) + " " + Convert.ToString(Chosen_Mode));
            Matrix_Init(1, Res_number_of_rows, Res_number_of_columns, Res_matrix1, null, null);

            for (int i = 0; i < 4; i++) // Buttons
            {
                this.RS_Buttons[i].Location = new Point(last_horizontal - 180 - 7 + eigen_vectors_last_horizontal / 2 + i * 120, last_vertical[Number_of_Matrices] + additional_vertical_padding);
                this.Controls.Add(RS_Buttons[i]);
            }
        }

        public void Disable_LSM_Additional_Buttons()
        {
            this.Save_as_buttons[0].Click -= Save_panel_as_Image_Click;
            this.Save_as_buttons[0].Enabled = false;
            this.Save_as_buttons[0].Visible = false;
        }


        //********************************* Event Handlers *********************************\\

        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close_IDentificator = 1;
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            this.Back_IDentificator = 1;
        }

        private void Button_Open_Click(object sender, EventArgs e)
        {
            this.Open_IDentificator = 1;
            if (!this.file_created)
            {
                if (RS_chosen_mode == 11)
                {
                    Create_Text_File_EigenValues(this.Res_number_of_rows, EigValues_Final, Eigenvectors);
                }
                else
                {
                    Create_Text_File(this.Number_of_Matrices, this.Res_number_of_rows, this.Res_number_of_columns);
                }
                this.file_created = true;
            }
        }

        private void Small_Button_Open_Click(object sender, EventArgs e)
        {
            Button The_sender = (Button)sender;
            this.Small_Open_IDentificator = (int)The_sender.Tag;
            this.Small_Buttons_Memory = this.Small_Open_IDentificator;
            if (!this.file_created)
            {
                Create_Text_File(this.Number_of_Matrices, this.Res_number_of_rows, this.Res_number_of_columns);
                this.file_created = true;
            }
        }

        private void Button_Save_as_Click(object sender, EventArgs e)
        {
            this.Save_as_IDentificator = 1;
            if (!this.file_created)
            {
                if (RS_chosen_mode == 11)
                {
                    Create_Text_File_EigenValues(this.Res_number_of_rows, EigValues_Final, Eigenvectors);
                }
                else
                {
                    Create_Text_File(this.Number_of_Matrices, this.Res_number_of_rows, this.Res_number_of_columns);
                }
                this.file_created = true;
            }
        }

        private void Small_Button_Save_as_Click(object sender, EventArgs e)
        {
            Button The_sender = (Button)sender;
            this.Small_Save_as_IDentificator = (int)The_sender.Tag;
            this.Small_Buttons_Memory = Small_Save_as_IDentificator;
            if (!this.file_created)
            {
                Create_Text_File(this.Number_of_Matrices, this.Res_number_of_rows, this.Res_number_of_columns);
                this.file_created = true;
            }
        }

        private void Save_panel_as_Image_Click(object sender, EventArgs e)
        {
            Button The_sender = (Button)sender;
            this.Small_Save_as_IDentificator = (int)The_sender.Tag;
            this.Small_Buttons_Memory = Small_Save_as_IDentificator;
            Save_panel_as_Image(The_Diagram.Width, The_Diagram.Height, The_Diagram);
        }

        public void Draw_RREF_Line(object sender, PaintEventArgs e)
        {
            this.pen1 = new Pen(Color.White, 3);
            Point A = new Point(Result_Matrix[0, 0, Res_number_of_columns - 1].Location.X - 22, Result_Matrix[0, 0, Res_number_of_columns - 1].Location.Y);
            Point B = new Point(Result_Matrix[0, 0, Res_number_of_columns - 1].Location.X - 22, Result_Matrix[0, Res_number_of_rows - 1, Res_number_of_columns - 1].Location.Y + Result_Matrix[0, Res_number_of_rows - 1, Res_number_of_columns - 1].Height);
            e.Graphics.DrawLine(pen1, A, B);
        }
    }




    // The Diagram *********************************************************************************************************************************
    class Approximation_Graph: Panel
    {
        int number_of_samples;
        public double[,] Data;
        public double[] Max_Values = new double[2];
        public double[] Min_Values = new double[2];
        int[] item_count = new int[2];
        float[] gap_length = new float[2];
        double[] mapping_scale = new double[2];

        double[,] Aproximation;

        Label[] X_labels;
        Label[] Y_labels;
        Label Data_Label;
        Label Aproximation_Label;
        Label[] Scale_Info;

        Pen pen2;
        Brush The_Brush;
        PointF[] Horizontal_Points;
        PointF[] Vertical_Points;
        PointF[] The_Curve_Points;
        PointF[] Aproximation_Points;

        int vertical_padding = 50;
        int horizontal_padding = 75;

        public Boolean Error = false;


        public Approximation_Graph(int width, int height, int location_X, int location_Y, int samples, double[,] data)
        {
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(width, height);
            this.Location = new Point(location_X, location_Y);
            this.number_of_samples = samples;
            this.Data = new double[samples, 2];
            this.Data = data;
        }


        private void Label_Init_2(int width, int height, int loc_X, int loc_Y, int font_size, string text, Color Text_colour, ref Label The_Label)
        {
            The_Label = new Label();
            The_Label.Location = new Point(loc_X, loc_Y);
            The_Label.Size = new Size(width, height);
            The_Label.Font = new Font(The_Label.Font.FontFamily, font_size);
            The_Label.Text = text;
            The_Label.TextAlign = ContentAlignment.MiddleCenter;
            The_Label.ForeColor = Text_colour;
            this.Controls.Add(The_Label);
        }


        public void Set_Scale_Labels(int count, TextBox[] The_Info)
        {
            this.Scale_Info = new Label[count];
            int the_bottom_middle = (int)Horizontal_Points[item_count[0] - 1].Y + (int)((this.Height - Horizontal_Points[item_count[0] - 1].Y) / 2);
            Point The_Point = new Point((int)((Horizontal_Points[item_count[0] - 1].X - Horizontal_Points[0].X) / 2 ) + 15, the_bottom_middle);
            Label_Init_2(120, 30, The_Point.X, The_Point.Y, 11, The_Info[0].Text, Color.Black, ref Scale_Info[0]);
            The_Point = new Point((int)Vertical_Points[0].X - 60, (int)Vertical_Points[0].Y - 41);
            Label_Init_2(120, 30, The_Point.X, The_Point.Y, 11, The_Info[1].Text, Color.Black, ref Scale_Info[1]);
        }


        private double Find_Maximum_in_one_column(int samples, int the_column, double[,] data)
        {
            double max_value = double.MinValue;
            for (int i = 0; i < samples; i++)
            {
                if (data[i, the_column] > max_value)
                {
                    max_value = data[i, the_column];
                }
            }
            return max_value;
        }



        private double Find_Minimum_in_spec_column(int samples, int the_column, double[,] data)
        {
            double min_value = double.MaxValue;
            for (int i = 0; i < samples; i++)
            {
                if (data[i, the_column] < min_value)
                {
                    min_value = data[i, the_column];
                }
            }
            return min_value;
        }



        private void Make_Mapping_Scale()
        {
            double[] Value_Difference = new double[2];
            double[] Coordinates_Difference = new double[2];

            for (int i = 0; i < 2; i++)
            {
                Value_Difference[i] = Math.Abs(Max_Values[i] - Min_Values[i]);
                if (i == 0)
                {
                    Coordinates_Difference[i] = Horizontal_Points[item_count[i] - 1].X - Horizontal_Points[0].X;
                }
                else
                {
                    Coordinates_Difference[i] = Vertical_Points[item_count[i] - 1].Y - Vertical_Points[0].Y;
                }
                this.mapping_scale[i] = Coordinates_Difference[i] / Value_Difference[i];
            }
        }



        private float Calculate_X(int padding, double The_Difference, int data_column_index)
        {
            float coordinate = 0;
            coordinate = (float)(padding + The_Difference * mapping_scale[data_column_index]);
            return coordinate;
        }



        private float Calculate_Y(float the_zero_point, double The_Difference, int data_column_index)
        {
            float coordinate = the_zero_point;
            coordinate -= (float)(The_Difference * mapping_scale[data_column_index]);
            return coordinate;
        }



        private void Graph_points(int samples, double[,] data, int mode)
        {
            double difference = 0;
            float X = 0, Y = 0;
            float the_grand_zero = Y_labels[item_count[1] - 1].Location.Y;
            if (mode == 0)
            {
                this.The_Curve_Points = new PointF[samples];
            }
            else
            {
                this.Aproximation_Points = new PointF[samples];
            }


            for (int i = 0; i < samples; i++)
            {
                X = 0;
                Y = 0;
                for (int j = 0; j < 2; j++)
                {
                    difference = data[i, j] - Min_Values[j];
                    if (j == 0)
                    {
                        X = Calculate_X(horizontal_padding, difference, j);
                    }
                    else
                    {
                        Y = Calculate_Y(the_grand_zero, difference, j);
                        Y += 10;
                    }
                }

                if (mode == 0)
                {
                    this.The_Curve_Points[i] = new PointF(X, Y);
                }
                else
                {
                    this.Aproximation_Points[i] = new PointF(X, Y);
                }
            }
            //this.Refresh();
        }


        public double[,] Set_Aproxiation(int Data_Difference, double Min_X, double Max_X, double[] coeficients)
        {
            double[,] Result = new double[Data_Difference, 2];
            double temp = 0;
            double saver = Min_X;
            for (int i = 0; i < Data_Difference; i++)
            {
                temp = 0;
                Result[i, 0] = saver;
                for (int j = 0; j < 3; j++)
                {
                    switch (j)
                    {
                        case 0:
                            temp += coeficients[j] * saver * saver;
                            break;
                        case 1:
                            temp += coeficients[j] * saver;
                            break;
                        case 2:
                            temp += coeficients[j];
                            break;
                    } 
                }
                Result[i, 1] = temp;
                saver++;
            }
            return Result;
        }


        public void Set_Graph(int samples, double[,] data, double[] coeficients, TextBox[] Scale_Info) // 0 == X; 1 == Y
        {
            double temp_rounder = 0;
            float temp_padding = 0;
            float last_item_position = 0;
            float the_reminder = 0;
            int total_padding = 0;
            int[] divisor = new int[] { 10, 10 };

            this.Label_Init_2(92, 20, this.Width - 88, 26, 10, Writings.Data_str, Color.DarkBlue, ref Data_Label);
            Data_Label.TextAlign = ContentAlignment.MiddleLeft;
            this.Label_Init_2(92, 20, this.Width - 87, 6, 10, Writings.Aproximation, Color.Red, ref Aproximation_Label);
            Aproximation_Label.TextAlign = ContentAlignment.MiddleLeft;

            Min_Values[0] = data[0, 0];
            Max_Values[0] = data[samples - 1, 0];
            Min_Values[1] = Find_Minimum_in_spec_column(samples, 1, data);
            //MessageBox.Show(Convert.ToString(Min_Values[1]) + " : Before");
            Max_Values[1] = Find_Maximum_in_one_column(samples, 1, data);
            for (int i = 0; i < 2; i++)
            {
                the_reminder = 100;
                if (i == 0)
                {
                    total_padding = 95;
                    temp_rounder = Math.Round((Min_Values[i] - 5) / 10) * 10;
                    Min_Values[i] = temp_rounder;
                    temp_rounder = Math.Round((Max_Values[i] + 5) / 10) * 10;
                    Max_Values[i] = temp_rounder;
                }
                else
                {
                    total_padding = 80;
                    temp_rounder = Math.Round((Min_Values[i] - 20) / 10) * 10;
                    Min_Values[i] = temp_rounder;
                    temp_rounder = Math.Round((Max_Values[i] + 10) / 10) * 10;
                    Max_Values[i] = temp_rounder;
                }

                item_count[i] = (int)(Math.Abs(Max_Values[i] - Min_Values[i])) / divisor[i] + 1;
                if (item_count[i] <= 2)
                {
                    divisor[i] = 2;
                    item_count[i] = (int)(Math.Abs(Max_Values[i] - Min_Values[i])) / 2 + 1;
                }
                else if (item_count[i] <= 4)
                {
                    divisor[i] = 5;
                    item_count[i] = (int)(Math.Abs(Max_Values[i] - Min_Values[i])) / 5 + 1;
                }

                Boolean stop = false;
                for (int k = 1001; k > 0 && !stop; k --)
                {
                    if (item_count[i] > 4 * k)
                    {
                        divisor[i] = 4 * k;
                        item_count[i] = (int)(Math.Abs(Max_Values[i] - Min_Values[i])) / (4 * k) + 1; // je zapotrebi pricist jednicku, protoze pocet bodu je o 1 vetsi nez pocet sektoru
                        stop = true;
                    }
                }

                int temp_counter = 0;
                while (the_reminder > 75 && temp_counter < 35)
                {
                    if (i == 0)
                    {
                        gap_length[i] = (float)((this.Width - total_padding) / (double)item_count[i]);
                        last_item_position = (item_count[i] - 1) * gap_length[i] + horizontal_padding;
                        the_reminder = this.Width - last_item_position;
                    }
                    else
                    {
                        gap_length[i] = (float)((this.Height - total_padding) / (double)item_count[i]);
                        last_item_position = (item_count[i] - 1) * gap_length[i] + vertical_padding;
                        the_reminder = this.Height - last_item_position;
                    }

                    if (the_reminder < 50 && i == 0)
                    {
                        gap_length[i] = (float)((this.Width - total_padding + 10) / (double)item_count[i]);
                    }
                    else if (the_reminder < 50 && i == 1)
                    {
                        gap_length[i] = (float)((this.Height - total_padding + 10) / (double)item_count[i]);
                    }
                    else
                    {
                        total_padding -= 10;
                    }
                    temp_counter++;
                }
            }

            double temp_value = 0;
            float X = 0;
            float Y = 0;
            //MessageBox.Show(Convert.ToString(Min_Values[1]) + " : After");
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    temp_padding = horizontal_padding;
                    Horizontal_Points = new PointF[(int)item_count[i]];
                    this.X_labels = new Label[item_count[i]];
                    temp_value = Min_Values[i];
                }
                else
                {
                    temp_padding = vertical_padding;
                    Vertical_Points = new PointF[(int)item_count[i]];
                    this.Y_labels = new Label[item_count[i]];
                    temp_value = Max_Values[i];
                }
                //MessageBox.Show(Convert.ToString(item_count[i]) + ": IT_COUNT");
                //MessageBox.Show(Convert.ToString(divisor[i])+" :Div");
                for (int k = 0; k < item_count[i]; k++)
                {
                    if (i == 0)
                    {
                        X = temp_padding;
                        Y = (item_count[1] - 1) * gap_length[1] + vertical_padding;
                        Horizontal_Points[k] = new PointF(X, Y);
                        this.Label_Init_2(36, 20, (int)Math.Round((double)(X - 18)), (int)Math.Round((double)(Y + 15)), 7, Convert.ToString(temp_value), Color.Black, ref X_labels[k]);
                        temp_value += divisor[i];
                    }
                    else
                    {
                        X = horizontal_padding;
                        Y = temp_padding;
                        Vertical_Points[k] = new PointF(X, Y);
                        this.Label_Init_2(50, 20, (int)Math.Round((double)(X - 60)), (int)Math.Round((double)(Y - 10)), 7, Convert.ToString(temp_value), Color.Black, ref Y_labels[k]);
                        temp_value -= divisor[i];
                    }
                    temp_padding += gap_length[i];
                }
            }
            Min_Values[1] = temp_value + divisor[1];
            Make_Mapping_Scale();
            Graph_points(samples, data, 0);

            double X_Min = data[0, 0];
            double X_Max = data[samples - 1, 0] + 1;

            int Data_Difference = (int)Math.Round(X_Max - X_Min);
            Aproximation = new double[Data_Difference, 2];
            Aproximation = Set_Aproxiation(Data_Difference, X_Min, X_Max, coeficients);
            Graph_points(Data_Difference, Aproximation, 1);
            Set_Scale_Labels(2, Scale_Info);

            this.Paint += new PaintEventHandler(Scale_Painting);
            this.Paint += new PaintEventHandler(Curve_Painting);
            this.Refresh();
        }



        // Event Handlers

        private void Scale_Painting(object sender, PaintEventArgs e)
        {
            float X = 0;
            float Y = 0;
            this.The_Brush = new SolidBrush(Color.Black);
            this.pen2 = new Pen(Color.Black, 2);
            Graphics graphics = e.Graphics;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int k = 0; k < item_count[i]; k++)
                    {
                        if (i == 0)
                        {
                            X = Horizontal_Points[k].X;
                            Y = Horizontal_Points[k].Y - 1;
                            if (k == 0)
                            {
                                graphics.FillRectangle(The_Brush, X, Y, 7, 9);
                            }
                            else
                            {
                                graphics.FillRectangle(The_Brush, X, Y, 2, 7);
                            }
                        }
                        else
                        {
                            X = Vertical_Points[k].X - 6;
                            Y = Vertical_Points[k].Y;
                            graphics.FillRectangle(The_Brush, X, Y, 7, 2);
                        }
                    }
                }


                graphics.DrawLines(pen2, Horizontal_Points);
                graphics.DrawLines(pen2, Vertical_Points);


                this.The_Brush = new SolidBrush(Color.DarkBlue);
                X = this.Width - 124;
                Y = 36;
                graphics.FillRectangle(The_Brush, X, Y, 33, 5);

                this.The_Brush = new SolidBrush(Color.Red);
                X = this.Width - 124;
                Y = 16;
                graphics.FillRectangle(The_Brush, X, Y, 33, 5);
            }
            catch (Exception The_Exception)
            {
                MessageBox.Show(Writings.Could_not_draw_scale + The_Exception.Message);
                this.Visible = false;
            }
        }


        private void Curve_Painting(object sender, PaintEventArgs e)
        {
            float X = 0;
            float Y = 0;
            this.The_Brush = new SolidBrush(Color.Blue);
            Graphics graphics = e.Graphics;
            this.pen2 = new Pen(Color.DarkBlue, 2);

            try
            {
                for (int i = 0; i < number_of_samples; i++)
                {
                    X = The_Curve_Points[i].X - (float)2.5;
                    Y = The_Curve_Points[i].Y - (float)2.5;

                    graphics.FillRectangle(The_Brush, X, Y, 5, 5);
                }
                graphics.DrawCurve(pen2, The_Curve_Points, 0.23F);
                //graphics.DrawLines(pen2, The_Curve_Points);
                this.pen2 = new Pen(Color.Red, 3);
                Boolean stop = false;
                int offset = 0;
                for (int i = 0; i < Aproximation_Points.Length && !stop; i++)
                {
                    if (Aproximation_Points[i].Y > this.Vertical_Points[Vertical_Points.Length - 1].Y + 5)
                    {
                        offset++;
                    }
                    else
                    {
                        stop = true;
                    }
                }
                graphics.DrawCurve(pen2, Aproximation_Points, offset, Aproximation_Points.Length - offset - 1);
            }
            catch (Exception The_Exception)
            {
                MessageBox.Show(Writings.Could_not_draw_the_curve + The_Exception.Message);
                this.Visible = false;
                this.Enabled = false;
                this.Parent.Controls.Remove(this);
                this.Error = true;
            }
        }
    }
}
