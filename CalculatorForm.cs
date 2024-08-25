using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        private string currentInput = "0";
        private string previousInput = "";
        private string operatorSymbol = "";
        private Label displayLabel;
        private Point lastPoint;
        private enum CalculatorMode { Basic, Scientific, Programmer }
        private CalculatorMode currentMode = CalculatorMode.Basic;
        private Dictionary<CalculatorMode, List<string>> modeButtons = new Dictionary<CalculatorMode, List<string>>();
        private List<Button> modeSelectionButtons = new List<Button>();

        public CalculatorForm()
        {
            InitializeComponent();
            InitializeModeButtons();
            SetupCalculator();
        }

        private void InitializeModeButtons()
        {
            modeButtons[CalculatorMode.Basic] = new List<string> { "C", "±", "%", "÷", "7", "8", "9", "×", "4", "5", "6", "-", "1", "2", "3", "+", "0", ",", ".", "=", "⌫" };
            modeButtons[CalculatorMode.Scientific] = new List<string> { "C", "±", "%", "÷", "sin", "cos", "tan", "×", "7", "8", "9", "-", "4", "5", "6", "+", "1", "2", "3", "=", "0", ",", ".", "⌫", "π", "e", "^", "√" };
            modeButtons[CalculatorMode.Programmer] = new List<string> { "C", "±", "%", "÷", "AND", "OR", "XOR", "×", "7", "8", "9", "-", "4", "5", "6", "+", "1", "2", "3", "=", "0", "A", "B", "⌫", "C", "D", "E", "F" };
        }

        private void SetupCalculator()
        {
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(350, 600);
            this.Padding = new Padding(10);

            SetupDisplay();
            SetupModeButtons();
            SetupButtons();
            SetupControlButtons();

            this.MouseDown += CalculatorForm_MouseDown;
            this.MouseMove += CalculatorForm_MouseMove;
        }

        private void SetupDisplay()
        {
            displayLabel = new Label
            {
                Width = 330,
                Height = 80,
                Top = 40,
                Left = 10,
                Font = new Font("Arial", 36, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Text = "0"
            };
            this.Controls.Add(displayLabel);
        }

        private void SetupModeButtons()
        {
            string[] modes = { "Basic", "Scientific", "Programmer" };
            int buttonWidth = 100;
            int buttonHeight = 30;
            int margin = 10;
            int startY = 130;

            for (int i = 0; i < modes.Length; i++)
            {
                Button modeButton = new Button
                {
                    Text = modes[i],
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Left = margin + i * (buttonWidth + margin),
                    Top = startY,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black
                };
                modeButton.Click += ModeButton_Click;
                this.Controls.Add(modeButton);
                modeSelectionButtons.Add(modeButton);
            }
        }

        private void SetupButtons()
        {
            ClearButtons();
            int buttonWidth = 70;
            int buttonHeight = 60;
            int margin = 10;
            int startY = 170;

            var buttons = modeButtons[currentMode];
            int rows = (int)Math.Ceiling(buttons.Count / 4.0);

            for (int i = 0; i < buttons.Count; i++)
            {
                int row = i / 4;
                int col = i % 4;

                Button button = new Button
                {
                    Text = buttons[i],
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Left = margin + col * (buttonWidth + margin),
                    Top = startY + row * (buttonHeight + margin),
                    Font = new Font("Arial", 16, FontStyle.Regular),
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 }
                };

                if (col == 3)
                {
                    button.BackColor = Color.FromArgb(255, 149, 0);
                    button.ForeColor = Color.White;
                }
                else if (char.IsLetter(buttons[i][0]) || buttons[i] == "±" || buttons[i] == "%" || buttons[i] == "⌫")
                {
                    button.BackColor = Color.FromArgb(212, 212, 210);
                    button.ForeColor = Color.Black;
                }
                else
                {
                    button.BackColor = Color.White;
                    button.ForeColor = Color.Black;
                }

                button.Click += Button_Click;
                this.Controls.Add(button);
            }

            this.Height = startY + (rows * (buttonHeight + margin)) + 50;
        }

        private void ClearButtons()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                if (this.Controls[i] is Button && !modeSelectionButtons.Contains(this.Controls[i]) &&
                    this.Controls[i] != closeButton && this.Controls[i] != minimizeButton)
                {
                    this.Controls.RemoveAt(i);
                }
            }
        }

        private void SetupControlButtons()
        {
            closeButton = new Button
            {
                Text = "×",
                Width = 30,
                Height = 30,
                Left = this.Width - 35,
                Top = 5,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatAppearance = { BorderSize = 0 }
            };
            closeButton.Click += (s, e) => this.Close();

            minimizeButton = new Button
            {
                Text = "_",
                Width = 30,
                Height = 30,
                Left = this.Width - 70,
                Top = 5,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Gray,
                FlatAppearance = { BorderSize = 0 }
            };
            minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            this.Controls.Add(closeButton);
            this.Controls.Add(minimizeButton);
        }

        private void ModeButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            switch (clickedButton.Text)
            {
                case "Basic":
                    currentMode = CalculatorMode.Basic;
                    break;
                case "Scientific":
                    currentMode = CalculatorMode.Scientific;
                    break;
                case "Programmer":
                    currentMode = CalculatorMode.Programmer;
                    break;
            }
            SetupButtons();
        }

        private void CalculatorForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void CalculatorForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;

            switch (buttonText)
            {
                case "C":
                    ClearCalculator();
                    break;
                case "=":
                    Calculate();
                    break;
                case "±":
                    ChangeSign();
                    break;
                case "%":
                    CalculatePercentage();
                    break;
                case "⌫":
                    DeleteLastDigit();
                    break;
                case "+":
                case "-":
                case "×":
                case "÷":
                    HandleOperator(buttonText);
                    break;
                case ",":
                case ".":
                    AppendDecimalPoint(buttonText);
                    break;
                default:
                    if (currentMode == CalculatorMode.Scientific)
                    {
                        HandleScientificOperation(buttonText);
                    }
                    else if (currentMode == CalculatorMode.Programmer)
                    {
                        HandleProgrammerOperation(buttonText);
                    }
                    else
                    {
                        AppendToInput(buttonText);
                    }
                    break;
            }
        }

        private void AppendToInput(string value)
        {
            if (currentInput == "0" && value != "." && value != ",")
            {
                currentInput = value;
            }
            else
            {
                currentInput += value;
            }
            UpdateDisplay(currentInput);
        }

        private void AppendDecimalPoint(string decimalPoint)
        {
            if (!currentInput.Contains(".") && !currentInput.Contains(","))
            {
                currentInput += decimalPoint;
                UpdateDisplay(currentInput);
            }
        }

        private void HandleOperator(string op)
        {
            if (string.IsNullOrEmpty(currentInput))
                return;

            if (!string.IsNullOrEmpty(previousInput))
            {
                Calculate();
            }

            operatorSymbol = op;
            previousInput = currentInput;
            currentInput = "0";
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(previousInput) || string.IsNullOrEmpty(currentInput))
                return;

            double prev = double.Parse(previousInput.Replace(",", "."));
            double current = double.Parse(currentInput.Replace(",", "."));
            double result = 0;

            switch (operatorSymbol)
            {
                case "+":
                    result = prev + current;
                    break;
                case "-":
                    result = prev - current;
                    break;
                case "×":
                    result = prev * current;
                    break;
                case "÷":
                    if (current != 0)
                        result = prev / current;
                    else
                    {
                        MessageBox.Show("Nie można dzielić przez zero!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearCalculator();
                        return;
                    }
                    break;
            }

            currentInput = result.ToString().Replace(".", ",");
            previousInput = "";
            operatorSymbol = "";
            UpdateDisplay(currentInput);
        }

        private void ChangeSign()
        {
            if (currentInput != "0")
            {
                if (currentInput.StartsWith("-"))
                    currentInput = currentInput.Substring(1);
                else
                    currentInput = "-" + currentInput;

                UpdateDisplay(currentInput);
            }
        }

        private void CalculatePercentage()
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                double value = double.Parse(currentInput.Replace(",", "."));
                value /= 100;
                currentInput = value.ToString().Replace(".", ",");
                UpdateDisplay(currentInput);
            }
        }

        private void DeleteLastDigit()
        {
            if (currentInput.Length > 1)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else
            {
                currentInput = "0";
            }
            UpdateDisplay(currentInput);
        }

        private void UpdateDisplay(string value)
        {
            displayLabel.Text = value;
        }

        private void ClearCalculator()
        {
            currentInput = "0";
            previousInput = "";
            operatorSymbol = "";
            UpdateDisplay(currentInput);
        }

        private void HandleScientificOperation(string operation)
        {
            double value = double.Parse(currentInput.Replace(",", "."));
            double result = 0;

            switch (operation)
            {
                case "sin":
                    result = Math.Sin(value);
                    break;
                case "cos":
                    result = Math.Cos(value);
                    break;
                case "tan":
                    result = Math.Tan(value);
                    break;
                case "π":
                    result = Math.PI;
                    break;
                case "e":
                    result = Math.E;
                    break;
                case "^":
                    // Implement power function
                    break;
                case "√":
                    result = Math.Sqrt(value);
                    break;
                default:
                    AppendToInput(operation);
                    return;
            }

            currentInput = result.ToString().Replace(".", ",");
            UpdateDisplay(currentInput);
        }

        private void HandleProgrammerOperation(string operation)
        {
            // Implement programmer operations here
            AppendToInput(operation);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 20;
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);
            }
        }

        private Button closeButton;
        private Button minimizeButton;
    }
}