using System;
using System.Drawing;
using System.Windows.Forms;

namespace Extended_Matrix_Calculator
{
    class The_Menu: Panel
    {
        public Boolean Menu_is_opened = false;

        Button[] Languages;
        int languages_count = 4;
        public int Chosen_Language = 1;
        public int Language_Button_Clicked = 0;

        public CustomNumericUpDown Accuracy;
        Label Accuracy_Label = new Label();
        Label Accuracy_Cover = new Label();
        public int Accuracy_number = 2;

        Button[] Mods;
        public int first, second;
        public int Chosen_Mode = 9000; // Ulozi, ktery mod byl aktivovan. O mody se staraji forma a Result Screen; 9000 = nebyl vybran zadny mod
        public int Memory = 9000;
        public int Long_Lasting_Memory = 9000; //dulezite v modech 2,3 a 4 (Sum, Subtract, Multiply); ukladani se provede pri ukonceni Matrix1 screenu
        public Boolean swap_rows = false;
        public Boolean swap_columns = false;


        //********************************** Constructors *********************************\\

        public The_Menu()
        {
            this.Visible = false;
            this.Enabled = false;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.AutoScroll = true;
            this.AllowDrop = false;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.FromArgb(0, 170, 255, 255);
            this.Location = new Point(1, 44);
            this.Width = 210;
            this.Height = 698;

            this.Languages = new Button[this.languages_count];
            for (int i = 0; i < languages_count; i++)
            {
                this.Languages[i] = new Button();
                this.Languages[i].Location = new Point(i * 50 + 10, 7);
                this.Languages[i].Size = new Size(40, 30);
                this.Languages[i].FlatStyle = FlatStyle.Popup;
                this.Languages[i].BackColor = Color.FromArgb(5, 170, 255, 255);
                this.Languages[i].BackgroundImageLayout = ImageLayout.Stretch;
                switch (i)
                {
                    case 0:
                        this.Languages[i].Tag = 0;
                        this.Languages[i].Name = "Bulgarian";
                        this.Languages[i].BackgroundImage = Image.FromFile($"{Environment.CurrentDirectory}/Pics/Bulgarian.png");
                        break;
                    case 1:
                        this.Languages[i].Tag = 1;
                        this.Languages[i].Name = "Czech";
                        this.Languages[i].Enabled = false;
                        this.Languages[i].BackColor = Color.FromArgb(0, 255, 177, 150);
                        this.Languages[i].BackgroundImage = Image.FromFile($"{Environment.CurrentDirectory}/Pics/Czech.png");
                        break;
                    case 2:
                        this.Languages[i].Tag = 2;
                        this.Languages[i].Name = "English";
                        this.Languages[i].BackgroundImage = Image.FromFile($"{Environment.CurrentDirectory}/Pics/UK.png");
                        break;
                    case 3:
                        this.Languages[i].Tag = 3;
                        this.Languages[i].Name = "German";
                        this.Languages[i].BackgroundImage = Image.FromFile($"{Environment.CurrentDirectory}/Pics/German.png");
                        break;
                }
                this.Languages[i].Click += new EventHandler(Language_Button_Click);
                this.Controls.Add(this.Languages[i]);
            }

            this.Accuracy_Label.Location = new Point(10, 48);
            this.Accuracy_Label.Size = new Size(111, 26);
            this.Accuracy_Label.Font = new Font(this.Accuracy_Label.Font.FontFamily, 13);
            this.Accuracy_Label.Text = Writings.Accuracy;
            this.Accuracy_Label.BackColor = this.BackColor;
            this.Accuracy_Label.ForeColor = this.ForeColor;
            this.Controls.Add(this.Accuracy_Label);

            this.Accuracy = new CustomNumericUpDown(75, 52, 14, 0, 15);
            this.Accuracy.Location = new Point(120, 49);
            this.Accuracy.BackColor = Color.Black;
            this.Accuracy.ForeColor = this.ForeColor;
            this.Accuracy.BorderStyle = BorderStyle.None;
            this.Accuracy.TextAlign = HorizontalAlignment.Center;
            this.Accuracy.Value = this.Accuracy_number;
            this.Controls.Add(this.Accuracy);

            this.Accuracy_Cover.Size = new Size(16, 27);
            this.Accuracy_Cover.Location = new Point(Accuracy.Location.X + Accuracy.Width - 16, Accuracy.Location.Y);
            this.Accuracy_Cover.BackColor = this.BackColor;
            this.Controls.Add(Accuracy_Cover);
            this.Accuracy_Cover.BringToFront();


            this.Mods = new Button[16];
            for (int i = 0; i < 16; i++)
            {
                this.Mods[i] = new Button();
                this.Mods[i].Location = new Point(10, 38 * i + 85);
                this.Mods[i].Size = new Size(190, 32);
                this.Mods[i].FlatStyle = FlatStyle.Popup;
                this.Mods[i].Font = new Font(this.Mods[i].Font.FontFamily, 14);
                this.Mods[i].ForeColor = Color.White;
                this.Mods[i].BackColor = Color.FromArgb(5, 170, 255, 255);
                this.Mods[i].Enabled = false;
                this.Mods[i].Click += Click_Mode;
                this.Mods[i].Tag = i;
                switch (i)
                {
                    case 0:
                        Mods[i].Name = "Swap_rows";
                        Mods[i].Text = Writings.Swap_rows;
                        break;
                    case 1:
                        Mods[i].Name = "Swap_columns";
                        Mods[i].Text = Writings.Swap_columns;
                        break;
                    case 2:
                        Mods[i].Name = "Sum";
                        Mods[i].Text = Writings.Sum;
                        break;
                    case 3:
                        Mods[i].Name = "Subtract";
                        Mods[i].Text = Writings.Subtract;
                        break;
                    case 4:
                        Mods[i].Name = "Multiply";
                        Mods[i].Text = Writings.Multiply;
                        break;
                    case 5:
                        Mods[i].Name = "Transpose";
                        Mods[i].Text = Writings.Transpose;
                        break;
                    case 6:
                        Mods[i].Name = "REF";
                        Mods[i].Text = Writings.REF;
                        break;
                    case 7:
                        Mods[i].Name = "RREF";
                        Mods[i].Text = Writings.RREF;
                        break;
                    case 8:
                        Mods[i].Name = "Inverse";
                        Mods[i].Text = Writings.Inverse;
                        break;
                    case 9:
                        Mods[i].Name = "Determinant";
                        Mods[i].Text = Writings.Determinant;
                        break;
                    case 10:
                        Mods[i].Name = "Power_Iteration";
                        Mods[i].Text = Writings.Power_Iteration;
                        break;
                    case 11:
                        Mods[i].Name = "EigenVals";
                        Mods[i].Text = Writings.EigenValues_txt;
                        break;
                    case 12:
                        Mods[i].Name = "Cholesky";
                        Mods[i].Text = Writings.Cholesky;
                        break;
                    case 13:
                        Mods[i].Name = "Spectral_decomposition";
                        Mods[i].Text = Writings.Spectral_Decomposition;
                        break;
                    case 14:
                        Mods[i].Name = "SVD";
                        Mods[i].Text = Writings.SVD;
                        break;
                    case 15:
                        Mods[i].Name = "LSM";
                        Mods[i].Text = Writings.LSM;
                        break;
                }
                this.Controls.Add(Mods[i]);
            }
        }

        //********************************** Help Functions *********************************\\

        public void Open_Close()
        {
            if (this.Menu_is_opened)
            {
                this.Refresh_Accuracy();
                this.Menu_is_opened = false;
                this.Visible = false;
                this.Enabled = false;
            }
            else
            {
                this.BringToFront();
                this.Menu_is_opened = true;
                this.Visible = true;
                this.Enabled = true;
            }
        }

        public void Refresh_Accuracy()
        {
            this.Accuracy_number = (int)this.Accuracy.Value;
        }

        public void Refresh_Text()
        {
            this.Accuracy_Label.Text = Writings.Accuracy;
            for (int i = 0; i < 16; i++)
            {
                switch (i)
                {
                    case 0:
                        Mods[i].Text = Writings.Swap_rows;
                        break;
                    case 1:
                        Mods[i].Text = Writings.Swap_columns;
                        break;
                    case 2:
                        Mods[i].Text = Writings.Sum;
                        break;
                    case 3:
                        Mods[i].Text = Writings.Subtract;
                        break;
                    case 4:
                        Mods[i].Text = Writings.Multiply;
                        break;
                    case 5:
                        Mods[i].Text = Writings.Transpose;
                        break;
                    case 6:
                        Mods[i].Text = Writings.REF;
                        break;
                    case 7:
                        Mods[i].Text = Writings.RREF;
                        break;
                    case 8:
                        Mods[i].Text = Writings.Inverse;
                        break;
                    case 9:
                        Mods[i].Text = Writings.Determinant;
                        break;
                    case 10:
                        Mods[i].Text = Writings.Power_Iteration;
                        break;
                    case 11:
                        Mods[i].Text = Writings.EigenValues_txt;
                        break;
                    case 12:
                        Mods[i].Text = Writings.Cholesky;
                        break;
                    case 13:
                        Mods[i].Text = Writings.Spectral_Decomposition;
                        break;
                    case 14:
                        Mods[i].Text = Writings.SVD;
                        break;
                    case 15:
                        Mods[i].Text = Writings.LSM;
                        break;
                }
            }
        }

        public void Reset_Button_Selectable(int index)
        {
            this.Mods[index].Enabled = true;
            this.Mods[index].FlatStyle = FlatStyle.Popup;
            this.Mods[index].ForeColor = Color.White;
            this.Mods[index].BackColor = Color.FromArgb(5, 170, 255, 255);
        }

        public void Reset_Button_Unselectable(int index)
        {
            this.Mods[index].Enabled = false;
            this.Mods[index].FlatStyle = FlatStyle.Popup;
            this.Mods[index].ForeColor = Color.White;
            this.Mods[index].BackColor = Color.FromArgb(5, 170, 255, 255);
        }


        public void Choose_Mode(int index)
        {
            bool stop = false;
            bool disabled = false; // pri testovani na konci musim dukladne zkontrolovat jestli koncept s tim disabled funguje
            this.Chosen_Mode = index;
            for (int i = 0; i < 16 && !stop; i++)
            {
                if (i == Chosen_Mode)
                {
                    if (Mods[i].Enabled)
                    {
                        Mods[i].Enabled = false;
                    }
                    else
                    {
                        disabled = true;
                    }
                    this.Mods[i].BackColor = Color.FromArgb(0, 255, 177, 240);
                    stop = true;
                }
            }
            stop = false;
            for (int i = 0; i < 16 && !stop; i++) // vratit do predchoziho stavu
            {
                if (i == Memory)
                {
                    if (!disabled)
                    {
                        Mods[i].Enabled = true;
                    }
                    this.Mods[i].BackColor = Color.FromArgb(5, 170, 255, 255);
                    stop = true;
                }
            }
            stop = false;
            if (Chosen_Mode == Memory) // resetnout vybrana podruhe tlacitka
            {
                Chosen_Mode = 9000;
                int i = 9000;
                if (Memory == 0) { i = 0; }
                else { i = 1; }
                Mods[i].Enabled = true;
                this.Mods[i].BackColor = Color.FromArgb(5, 170, 255, 255);
            }
            if (Chosen_Mode == 0)
            {
                Swap_Rows_Function();
            }
            else if (Chosen_Mode == 1)
            {
                Swap_Columns_Function();
            }
            else
            {
                this.swap_columns = false;
                this.swap_rows = false;
            }
            this.Memory = Chosen_Mode; // memorize at the end of operation
        }


        public void Reset_Menu_Selectable()
        {
            this.Chosen_Mode = 9000;
            this.Memory = 9000;
            this.Accuracy.Enabled = true;
            for (int i = 0; i < 15; i++)
            {
                Reset_Button_Selectable(i);
            }
        }

        public void Reset_Menu_Unselectable()
        {
            this.Chosen_Mode = 9000;
            this.Memory = 9000;
            for (int i = 0; i < 16; i++)
            {
                Reset_Button_Unselectable(i);
            }
        }

        public void Menu_Back_to_matrix_1()
        {
            this.Chosen_Mode = this.Long_Lasting_Memory;
            this.Accuracy.Enabled = true;
            this.Memory = this.Chosen_Mode;
            for (int i = 0; i < languages_count; i++)
            {
                if (i != Chosen_Language)
                {
                    this.Languages[i].Enabled = true;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                if (i != this.Long_Lasting_Memory)
                {
                    Reset_Button_Selectable(i);
                }
            }
        }

        public void Matrix1_New_Menu_Init()
        {
            for (int i = 0; i < 15; i++)
            {
                this.Mods[i].Enabled = true;
            }
        }

        public void Activate_first_two_buttons() // aktivace prvnich dvou tlacitek bez dalsich zmen
        {
            this.Accuracy.Enabled = true;
            for (int i = 0; i < languages_count; i++)
            {
                if (i != Chosen_Language)
                {
                    this.Languages[i].Enabled = true;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                this.Enable_ModeChoose_Button(i);
            }
        }

        public void Disable_all_No_Changes()
        {
            this.Accuracy.Enabled = false;
            for (int i = 0; i < languages_count; i++)
            {
                this.Languages[i].Enabled = false;
            }
            for (int i = 0; i < 16; i++)
            {
                this.Mods[i].Enabled = false;
            }
        }

        public void Disable_mods_No_Changes()
        {
            for (int i = 0; i < languages_count; i++)
            {
                if (i != Chosen_Language)
                {
                    this.Languages[i].Enabled = true;
                }
            }
            this.Accuracy.Enabled = true;
            for (int i = 0; i < 16; i++)
            {
                this.Mods[i].Enabled = false;
            }
        }

        public void Swap_Rows_Function()
        {
            this.Mods[0].Enabled = true; // po kliknuti tlacitka se jenom obarvi, zustane Enabled
            this.swap_rows = true;
            this.swap_columns = false;
        }

        public void Swap_Columns_Function()
        {
            this.Mods[1].Enabled = true;
            this.swap_columns = true;
            this.swap_rows = false;
        }

        public void Enable_ModeChoose_Button(int index) // taky vynuluje Chosen_Mode a Memory; je zapotrebi po vymene sloupcu/radku ve Form1
        {
            this.Chosen_Mode = 9000;
            this.Mods[index].Enabled = true;
            this.Mods[index].FlatStyle = FlatStyle.Popup;
            this.Mods[index].BackColor = Color.FromArgb(5, 170, 255, 255);
            this.Memory = 9000;
        }

        public void Change_Language(int Button_index, int memory)
        {
            this.Chosen_Language = Button_index;
            //this.Accuracy.Focus();
            if (this.Chosen_Language != memory)
            {
                for (int i = 0; i < languages_count; i++)
                {
                    if ((int)Languages[i].Tag == this.Chosen_Language)
                    {
                        Languages[i].Enabled = false;
                        Languages[i].BackColor = Color.FromArgb(0, 255, 177, 240);
                    }
                    else if ((int)Languages[i].Tag == memory)
                    {
                        Languages[i].Enabled = true;
                        Languages[i].BackColor = Color.FromArgb(5, 170, 255, 255);
                    }
                }
            }
        }

        //********************************* Event Handlers *********************************\\

        public void Click_Mode(object sender, EventArgs e)
        {
            Button Clicked_Button = (Button)sender;
            Choose_Mode((int)Clicked_Button.Tag);
        }

        public void Language_Button_Click(object sender, EventArgs e)
        {
            Button Clicked_Button = (Button)sender;
            Change_Language((int)Clicked_Button.Tag, this.Chosen_Language);
            this.Language_Button_Clicked = 1;
        }
    }
}
