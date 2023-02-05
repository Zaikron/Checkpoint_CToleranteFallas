using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkpoint
{
    public partial class Principal : Form
    {
        private int x;
        private int y;
        bool drawMode = false;

        Graphics g;
        Brush brush;
        Bitmap bmp;
        int brushRadio = 6;
        Color brushColor = Color.Black;

        public Principal()
        {
            InitializeComponent();

            bmp = new Bitmap(drawArea.Width, drawArea.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            drawArea.Image = bmp;

            brush = new SolidBrush(brushColor);

            drawTimer.Start();
        }

        //Dibuja un circulo
        void drawCircle(int x, int y, int r)
        {
            g.FillEllipse(brush, (x - r), (y - r), (2 * r), (2 * r));
            drawArea.Image = bmp;
        }

        //Eventos: Movimiento, mouse presionado, mouse soltado
        private void drawArea_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;

            if (drawMode == true)
            {
                drawCircle(x, y, brushRadio);
            }
        }

        private void drawArea_MouseDown(object sender, MouseEventArgs e)
        {
            drawMode = true;
        }

        private void drawArea_MouseUp(object sender, MouseEventArgs e)
        {
            drawMode = false;
        }

        //Comboboxes: tamaño de brocha, colores
        private void tamBrush_SelectedIndexChanged(object sender, EventArgs e)
        {
            brushRadio = int.Parse(tamBrush.SelectedIndex.ToString());
        }

        //Timer: Guardado cada minuto
        private void drawTimer_Tick(object sender, EventArgs e)
        {
            if (checkBoxSave.Checked == true)
            {
                Debug.WriteLine("saving...");
                createCheckpoint();
            }
        }

        //Seleccionar el color de pintado
        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                brushColor = colorDialog.Color;
                brush = new SolidBrush(brushColor);
            }
        }

        //Guardar la imagen
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "JPEG(*.JPG)|*.JPG|BMP(*.BMP)|*.BMP|PNG(*.PNG)|*.PNG";

            if (save.ShowDialog() == DialogResult.OK)
            {
                createCheckpoint();
                bmp.Save(save.FileName);
            }
        }

        //Guardado de seguridad del archivo
        private void createCheckpoint()
        {
            String date = DateTime.Now.ToString("yyyy_MM_dd_THH_mm_ss", System.Globalization.CultureInfo.InvariantCulture);
            Debug.WriteLine(date);
            try
            {
                StreamWriter sw = new StreamWriter(".\\" + date + ".smae");
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                    {
                        //Formato: x|y|(color->) r|g|b
                        sw.WriteLine(x + "|" + y + "|" + 
                                    bmp.GetPixel(x, y).R + "|" +
                                    bmp.GetPixel(x, y).G + "|" +
                                    bmp.GetPixel(x, y).B + "|");
                    }
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        //Funcion para abrir y restaurar un archivo
        private void restoreProject()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "SMAE(*.SMAE)|*.SMAE";
            openFileDialog.InitialDirectory = ".\\..";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String filePath = openFileDialog.FileName;
                String line;
                try
                {
                    StreamReader sr = new StreamReader(filePath);
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        //Formato: x|y|(color->) r|g|b
                        //         0 1           2 3 4
                        List<String> values = getValues(line);
                        Color currentColor = Color.FromArgb(int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]));
                        bmp.SetPixel(int.Parse(values[0]), int.Parse(values[1]), currentColor);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
                drawArea.Image = bmp;
            }
        }

        //Obtienen los valores separados por " | "
        private List<String> getValues(String line)
        {
            List<String> values = new List<String>();
            String currentValue = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (!line[i].Equals('|'))
                {
                    currentValue += line[i];
                }
                else if (line[i].Equals('|'))
                {
                    values.Add(currentValue);
                    currentValue = "";
                }
            }

            return values;
        }

        private void abrirProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            restoreProject();
        }

        //Guardado de seguridad al cerrar la ventana
        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBoxSave.Checked == true)
            {
                Debug.WriteLine("saving...");
                createCheckpoint();
            }
        }
    }
}
