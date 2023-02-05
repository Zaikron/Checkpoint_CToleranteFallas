> # UDG - CUCEI 
> #### 06 de febrero de 2023
### <p align="center"> Materia: Computacion Tolerante a Fallas </p>
### <p align="center"> Profesor: Michel Emanuel López Franco </p>
### <p align="center"> Anthony Esteven Sandoval Marquez, Ingenieria en Computacion </p>
### <p align="center"> Codigo: 215660767 </p>
### <p align="center"> Ciclo: 2023-A </p>

> ## Checkpointing

#### Se realizo un programa en C# el cual permite hacer dibujos simples, esto para hacer una demostración de restauración de algún trabajo que se estuviera haciendo en caso de haber un fallo en el software.
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c1.PNG"/> </p>

#### Existe una zona en la cual se pueden hacer los dibujos, en esta demostración se realizara un el cual no se guardara en ningún momento para así demostrar que podrá ser recuperable en caso de fallo en el software. 
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c2.PNG"/> </p>

#### Para hacer los guardados de seguridad decidí hacerlo en un archivo de texto en el cual se registrarán los pixeles de la imagen además de su color, el formato quedaría así: x|y|R|G|B
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c3.PNG"/> </p>
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c7.PNG"/> </p>

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


#### El guardado se realizara cada minuto, cuando se guarde el dibujo y cuando se cierre el programa.

        //Timer: Guardado cada minuto
        private void drawTimer_Tick(object sender, EventArgs e)
        {
            if (checkBoxSave.Checked == true)
            {
                Debug.WriteLine("saving...");
                createCheckpoint();
            }
        }
        
        
#### En caso de un cierre inesperado se podrá acceder a la opción de abrir proyecto y elegir el archivo de seguridad últimamente creado, estos se nombran a base de la fecha y hora actual por lo cual será fácil identificar los más recientes.
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c4.PNG"/> </p>
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c5.PNG"/> </p>

#### Cuando se abra un archivo de seguridad se podrá recuperar el progreso últimamente guardado
<p align="center"> <img src="https://github.com/Zaikron/Checkpoint_CToleranteFallas/blob/main/Checkpoint_Im/c6.PNG"/> </p>

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
