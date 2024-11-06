using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tringle
{
    public partial class Tringle_Vorm : Form
    {
        private Triangle triangle;
        private Point[] trianglePoints;
        private Panel drawingPanel;

        public Tringle_Vorm()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 450);
            this.Text = "Töö kolmnurgaga"; // Заголовок окна на эстонском
            this.BackColor = Color.LightBlue; // Фон окна

            InitializeCustomComponents();
            Side();

            triangle = new Triangle(3, 4, 5);
            DisplayTriangleInfo();
            CalculateTrianglePoints();
        }

        private void InitializeCustomComponents()
        {
            // Панель для рисования треугольника, перемещена в правый нижний угол
            drawingPanel = new Panel
            {
                Size = new Size(400, 250),
                Location = new Point(420, 150),
                BackColor = Color.White
            };
            drawingPanel.Paint += DrawingPanel_Paint;
            Controls.Add(drawingPanel);

            // Кнопка запуска, перемещена в правый нижний угол
            Button startButton = new Button
            {
                Text = "Käivita", // Текст кнопки на эстонском
                Font = new Font("Arial", 20, FontStyle.Regular),
                Size = new Size(150, 80),
                BackColor = Color.LightYellow,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(570, 40),
                Cursor = Cursors.Hand
            };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            // Радиокнопка для перехода на вторую форму, расположена в левом нижнем углу
            RadioButton switchFormRadioButton = new RadioButton
            {
                Text = "Arvuta kõrguse ja külje A järgi", // Текст на эстонском
                Location = new Point(10, 400),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };
            switchFormRadioButton.CheckedChanged += SwitchFormRadioButton_CheckedChanged;
            Controls.Add(switchFormRadioButton);
        }

        private void SwitchFormRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                Tringle_Vorm2 secondForm = new Tringle_Vorm2();
                secondForm.Show();
            }
        }

        private void DisplayTriangleInfo()
        {
            // Таблица DataGridView для отображения информации о треугольнике, расположена слева
            DataGridView dataGridView = new DataGridView
            {
                ColumnCount = 2,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ColumnHeadersVisible = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ScrollBars = ScrollBars.Vertical, // Добавлено вертикальное прокручивание
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            dataGridView.Size = new Size(400, 250);
            dataGridView.Location = new Point(10, 10);

            dataGridView.Columns[0].Name = "Väli"; // "Поле" на эстонском
            dataGridView.Columns[1].Name = "Väärtus"; // "Значение" на эстонском
            dataGridView.DefaultCellStyle.Padding = new Padding(5);
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.MediumPurple,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            dataGridView.ColumnHeadersDefaultCellStyle = headerStyle;
            dataGridView.ColumnHeadersHeight = 40;

            // Информация о треугольнике на эстонском
            dataGridView.Rows.Add("Külje pikkus a", " ");
            dataGridView.Rows.Add("Külje pikkus b", " ");
            dataGridView.Rows.Add("Külje pikkus c", " ");
            dataGridView.Rows.Add("Ümbermõõt", " ");
            dataGridView.Rows.Add("Pindala", " ");
            dataGridView.Rows.Add("Kas eksisteerib?", " ");
            dataGridView.Rows.Add("Spetsifikaator", "");

            Controls.Add(dataGridView);
        }

        private void Side()
        {
            // Поля для ввода длин сторон треугольника
            Label labelA = new Label
            {
                Text = "Külg A:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 270),
                AutoSize = true
            };
            Controls.Add(labelA);

            TextBox textBoxA = new TextBox
            {
                Name = "textBoxA",
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(150, 270),
                Size = new Size(100, 30)
            };
            Controls.Add(textBoxA);

            Label labelB = new Label
            {
                Text = "Külg B:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 310),
                AutoSize = true
            };
            Controls.Add(labelB);

            TextBox textBoxB = new TextBox
            {
                Name = "textBoxB",
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(150, 310),
                Size = new Size(100, 30)
            };
            Controls.Add(textBoxB);

            Label labelC = new Label
            {
                Text = "Külg C:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 350),
                AutoSize = true
            };
            Controls.Add(labelC);

            TextBox textBoxC = new TextBox
            {
                Name = "textBoxC",
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(150, 350),
                Size = new Size(100, 30)
            };
            Controls.Add(textBoxC);
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            double a, b, c;

            if (double.TryParse(Controls["textBoxA"].Text, out a) &&
                double.TryParse(Controls["textBoxB"].Text, out b) &&
                double.TryParse(Controls["textBoxC"].Text, out c))
            {
                triangle = new Triangle(a, b, c);
                CalculateTrianglePoints();

                DataGridView dataGridView = Controls.OfType<DataGridView>().FirstOrDefault();
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear();
                    dataGridView.Rows.Add("Külje pikkus a", triangle.GetSetA);
                    dataGridView.Rows.Add("Külje pikkus b", triangle.GetSetB);
                    dataGridView.Rows.Add("Külje pikkus c", triangle.GetSetC);
                    dataGridView.Rows.Add("Ümbermõõt", triangle.Perimeter());

                    // Округляем площадь до двух знаков после запятой
                    double roundedArea = Math.Round(triangle.Area(), 2);
                    dataGridView.Rows.Add("Pindala", roundedArea);

                    dataGridView.Rows.Add("Kas eksisteerib?", triangle.ExistTriangle ? "Eksisteerib" : "Ei eksisteeri");
                    dataGridView.Rows.Add("Spetsifikaator", triangle.TriangleType);
                }

                drawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Palun sisestage külgede jaoks kehtivad väärtused."); // Сообщение об ошибке на эстонском
            }
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            if (trianglePoints != null && trianglePoints.Length == 3)
            {
                using Pen pen = new(Color.Red, 3);
                e.Graphics.DrawPolygon(pen, trianglePoints);
            }
        }

        private void CalculateTrianglePoints()
        {
            if (triangle != null && triangle.ExistTriangle)
            {
                double a = triangle.GetSetA;
                double b = triangle.GetSetB;
                double c = triangle.GetSetC;

                // Вычисление высоты треугольника для стороны a
                double s = (a + b + c) / 2; // Полупериметр
                double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
                double height = (2 * area) / a;

                // Определение масштабных коэффициентов для панели
                int panelWidth = drawingPanel.Width;
                int panelHeight = drawingPanel.Height;

                // Масштабирование для оптимального размещения треугольника
                double scale = Math.Min(panelWidth / a, panelHeight / height) * 0.8; // 80% от панели

                // Центр панели для правильного размещения треугольника
                int centerX = panelWidth / 2;
                int centerY = panelHeight / 2;

                // Вычисление координат точек треугольника
                trianglePoints = new Point[3];
                trianglePoints[0] = new Point(centerX, centerY - (int)(height * scale)); // Верхняя точка
                trianglePoints[1] = new Point(centerX - (int)(b * scale / 2), centerY + (int)(height * scale / 2)); // Левая нижняя
                trianglePoints[2] = new Point(centerX + (int)(a * scale / 2), centerY + (int)(height * scale / 2)); // Правая нижняя
            }
            else
            {
                trianglePoints = null; // Если треугольник не существует, не рисуем его
            }
        }
    }
}
