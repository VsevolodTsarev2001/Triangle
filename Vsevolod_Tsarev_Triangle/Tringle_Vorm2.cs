using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tringle
{
    public partial class Tringle_Vorm2 : Form
    {
        private Triangle triangle;

        public Tringle_Vorm2()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Minu kolmnurg (kõrgus ja külg A)";
            this.Size = new Size(400, 250);
            this.BackColor = Color.LightGreen;

            Label labelA = new Label
            {
                Text = "Külg A:",
                Location = new Point(10, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red
            };
            Controls.Add(labelA);

            TextBox textBoxA = new TextBox
            {
                Name = "textBoxA",
                Location = new Point(150, 20),
                Size = new Size(100, 30)
            };
            Controls.Add(textBoxA);

            Label labelHeight = new Label
            {
                Text = "Kõrgus:",
                Location = new Point(10, 60),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red
            };
            Controls.Add(labelHeight);

            TextBox textBoxHeight = new TextBox
            {
                Name = "textBoxHeight",
                Location = new Point(150, 60),
                Size = new Size(100, 30)
            };
            Controls.Add(textBoxHeight);

            Button calculateButton = new Button
            {
                Text = "Arvuta",
                Location = new Point(150, 100),
                Size = new Size(100, 40),
                BackColor = Color.LightYellow,
                Font = new Font("Arial", 12, FontStyle.Regular)
            };
            calculateButton.Click += CalculateButton_Click;
            Controls.Add(calculateButton);
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (double.TryParse(Controls["textBoxA"].Text, out double a) &&
                double.TryParse(Controls["textBoxHeight"].Text, out double height))
            {
                double area = 0.5 * a * height;
                MessageBox.Show($"Kolmnurga pindala: {area}", "Tulemused");
            }
            else
            {
                MessageBox.Show("Palun sisestage kehtivad väärtused.");
            }
        }
    }
}
