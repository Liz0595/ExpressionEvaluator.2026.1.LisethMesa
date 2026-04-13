using System;
using System.Windows.Forms;

namespace ExpressionEvaluator.UI.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void OnButtonClick(object? sender, EventArgs e)
        {
            if (sender is not Button b) return;
            if (b.Text == "Clear")
            {
                txtDisplay.Text = string.Empty;
                return;
            }

            if (b.Text == "Delete")
            {
                var text = txtDisplay.Text;
                if (string.IsNullOrEmpty(text)) return;

                txtDisplay.Text = text.Substring(0, text.Length - 1);
                txtDisplay.SelectionStart = txtDisplay.Text.Length;
                return;
            }

            if (b.Text == "=")
            {
                EvaluateExpression();
                return;
            }

            txtDisplay.Text += b.Text;
            txtDisplay.SelectionStart = txtDisplay.Text.Length;
        }

        private void EvaluateExpression()
        {
            try
            {
                var expr = txtDisplay.Text;
                var result = ExpressionEvaluator.Core.Evaluator.Evaluate(expr);
                txtDisplay.Text = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
                txtDisplay.SelectionStart = txtDisplay.Text.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Creación de botones.
        private void InitializeButtons()
        {
            // Grid para ver los botones y el Delete - Clear
            string[] buttons = new string[] {
                "7","8","9","/",
                "4","5","6","*",
                "1","2","3","-",
                "0",".","=","+",
                "Delete","Clear","(",")"
            };
            int btnIndex = 0;
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (btnIndex >= buttons.Length) break;
                    var btn = new System.Windows.Forms.Button();
                    btn.Dock = System.Windows.Forms.DockStyle.Fill;
                    btn.Text = buttons[btnIndex++];
                    btn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                    btn.Click += new System.EventHandler(this.OnButtonClick);
                    this.tableLayoutPanel1.Controls.Add(btn, c, r);
                }
            }
        }
    }
}
