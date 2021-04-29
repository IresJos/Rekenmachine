using System;
using System.Windows.Forms;

namespace Rekenmachine2
{
    public partial class Form1 : Form
    {
        bool isIngedrukt = false;
        bool FoutieveInvoer = false;
        string som = "";
        string string1 = "";
        string string2 = "";
        string subsom = "";
        string symbool = "";
        double nummer1 = 0;
        double nummer2 = 0;
        double uitkomst = 0;
        int j = 0;
        decimal eurosom = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (scherm.Text == "0")
            {
                scherm.Clear();
            }
            if (isIngedrukt || FoutieveInvoer)
            {
                isIngedrukt = false;
                FoutieveInvoer = false;
                som = "";
                string1 = "";
                string2 = "";
                nummer1 = 0;
                nummer2 = 0;
                uitkomst = 0;
                scherm.Text = "";
            }
            scherm.Text = scherm.Text + b.Text;
        }

        private void euro_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ClearEntry_Click(object sender, EventArgs e)
        {
            scherm.Text = "0";
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            scherm.Text = "0";
            vergelijking2.Text = "";
            nummer1 = 0;
            nummer2 = 0;
            isIngedrukt = false;
            som = "";
            string1 = "";
            string2 = "";
            uitkomst = 0;
            j = 0;
            symbool = "";
        }

        private void is_Click(object sender, EventArgs e)
        {
            isIngedrukt = true;
            som = scherm.Text;
            // check of de invoer correct is
            for (int z = 0; z < som.Length - 1; z++)
            {
                if (!char.IsDigit(som[z]))
                {
                    if (som[z] != '(' || som[z] != ')' || som[z] != '%')
                    {
                        z--;
                        if (!char.IsDigit(som[z]))
                        {
                            scherm.Text = "Foutieve invoer";
                            FoutieveInvoer = true;
                        }
                        z += 2;
                        if (!char.IsDigit(som[z]))
                        {
                            scherm.Text = "Foutieve invoer";
                            FoutieveInvoer = true;
                        }
                    }
                }   
            }
            if (!FoutieveInvoer)
            {
                som = SomBerekenen(som);
                vergelijking2.Text = som;
                for (int i = 0; i < som.Length; i++)
                {
                    if (som[i] == '*' || som[i] == '/' || som[i] == '+' || som[i] == '-' || som[i] == '(' || som[i] == ')')
                    {
                        som = SomBerekenen(som);
                    }
                }

                if (checkBox1.Checked)
                {
                    eurosom = Math.Round(Convert.ToDecimal(som), 2);
                    scherm.Text = "€" + Convert.ToString(eurosom);
                }
                else
                {
                    scherm.Text = Convert.ToString(som);
                }
            }
        }

        private string SomBerekenen(string som)
        {
            // check voor %
            som = procent(som);
            // check voor ( en )
            som = haakjes(som, 0);
            // check voor * en /
            som = KeerEnDelen(som);
            // check voor + en -
            som = PlusEnMin(som);

            return som;
           
        }

        private void KijkLenR(int start, string som)
        {
            string1 = "";
            string2 = "";
            // aan de linker kant kijken
            for (j = start - 1; j >= 0; j--)
            {
                if (char.IsDigit(som[j]) || som[j] == ',' || som[0] == '-')
                {
                    string1 = som[j] + string1;
                }
                else
                {
                    break;
                }
            }

            // aan de rechter kant kijken
            for (j = start + 1; j <= som.Length - 1; j++)
            {
                if (char.IsDigit(som[j]) || som[j] == ',' || som[0] == '-')
                {
                    string2 += som[j];
                }
                else
                {
                    break;
                }
            }
            nummer1 = Convert.ToDouble(string1);
            nummer2 = Convert.ToDouble(string2);
        }

        private void backspace_Click(object sender, EventArgs e)
        {
            if (scherm.Text.Length > 0)
            {
                scherm.Text = scherm.Text.Remove(scherm.Text.Length - 1, 1);
            }
        }

        public string KeerEnDelen(string som)
        {
            for (int i = 0; i < som.Length; i++)
            {
                if (som[i] == '*' || som[i] == '/')
                {
                    string1 = "";
                    string2 = "";
                    KijkLenR(i, som);
                    if (som[i] == '*')
                    {
                        uitkomst = nummer1 * nummer2;
                        symbool = "*";
                        som = som.Replace(Convert.ToString(nummer1) + Convert.ToString(symbool) + Convert.ToString(nummer2), Convert.ToString(uitkomst));
                    }
                    else if (som[i] == '/')
                    {
                        uitkomst = nummer1 / nummer2;
                        symbool = "/";
                        som = som.Replace(Convert.ToString(nummer1) + Convert.ToString(symbool) + Convert.ToString(nummer2), Convert.ToString(uitkomst));
                    }
                }
            }
            return som;
        }

        public string PlusEnMin(string som)
        {
            for (int i = 0; i < som.Length; i++)
            {
                if (som[i] == '+' || som[i] == '-')
                {
                    string1 = "";
                    string2 = "";
                    KijkLenR(i, som);
                    if (som[i] == '+')
                    {
                        uitkomst = nummer1 + nummer2;
                        symbool = "+";
                        som = som.Replace(Convert.ToString(nummer1) + Convert.ToString(symbool) + Convert.ToString(nummer2), Convert.ToString(uitkomst));
                    }
                    else if (som[i] == '-')
                    {
                        uitkomst = nummer1 - nummer2;
                        symbool = "-";
                        som = som.Replace(Convert.ToString(nummer1) + Convert.ToString(symbool) + Convert.ToString(nummer2), Convert.ToString(uitkomst));
                    }
                }
            }
            return som;
        }

        public string procent(string som)
        {
            for (int i = 0; i < som.Length; i++)
            {
                if (som[i] == '%')
                {
                    string1 = "";
                    string2 = "";
                    for (i = 0; i < som.Length; i++)
                    {
                        if (som[i] == '*')
                        {
                            KijkLenR(i, som);
                            uitkomst = nummer1 / 100 * nummer2;
                            som = Convert.ToString(uitkomst);
                        }

                    }
                }
            }
            return som;
        }

        public string haakjes(string som, int index)
        {
            for (int i = index; i < som.Length; i++)
            {
                if (som[i] == '(')
                {
                    i++;
                    int x = som.IndexOf(')');
                    textBox1.Text += "int x = " + x + Environment.NewLine;
                    for (int g = i; g < x; g++)
                    {
                        subsom += som[g];
                        textBox1.Text += "subsom = " + subsom + Environment.NewLine; 
                    }
                    subsom = KeerEnDelen(subsom);
                    textBox1.Text += "subsom na keer en delen = " + subsom + Environment.NewLine;
                    subsom = PlusEnMin(subsom);
                    textBox1.Text += "subsom na plus en min = " + subsom + Environment.NewLine;
                    som = som.Remove(x--, 1);
                    som = som.Replace(Convert.ToString(nummer1) + Convert.ToString(symbool) + Convert.ToString(nummer2), Convert.ToString(subsom));
                    if (char.IsDigit(som[i - 2]))
                    {
                        som = som.Replace('(', '*');
                    }
                    else
                    {
                        som = som.Remove(i--, 1);
                    }
                    textBox1.Text += "subsom compleet = " + subsom + Environment.NewLine;
                }
            }
            return som; 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor = System.Drawing.Color.LightSlateGray;
            } else
            {
                checkBox1.BackColor = System.Drawing.Color.Gainsboro;
            }
        }
    }
}
