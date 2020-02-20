using System.Drawing;
using System.Windows.Forms;

namespace Extended_Matrix_Calculator
{
    public class CustomNumericUpDown: System.Windows.Forms.NumericUpDown
    {

        public CustomNumericUpDown(int width, int height, int font_size, int minimum, int maximum)
        {
            //this.BackColor = ColorTranslator.FromHtml(BackColor);
            this.Size = new Size(width, height);
            this.Font = new Font(this.Font.FontFamily, font_size);
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs Handled_M_Args = (HandledMouseEventArgs) e;
            if (Handled_M_Args != null)
            {
                Handled_M_Args.Handled = true;
            }

            if (e.Delta > 0 && (this.Value + this.Increment) <= this.Maximum)
            {
                this.Value += this.Increment;
            }
            else if (e.Delta < 0 && (this.Value - this.Increment) >= this.Minimum)
            {
                this.Value -= this.Increment;
            }
        }
    }
}
