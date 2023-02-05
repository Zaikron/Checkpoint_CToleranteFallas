> # UDG - CUCEI
### Materia: Computacion Tolerante a Fallas
### Profesor: Michel Emanuel López Franco
#### Anthony Esteven Sandoval Marquez, Ingenieria en Computacion
#### Codigo: 215660767
#### Ciclo: 2023-A

> ## Checkpointing

#### Se realizo un programa en C# el cual permite hacer dibujos simples, esto para hacer una demostración de restauración de algún trabajo que se estuviera haciendo en caso de haber un fallo en el software.

#### Existe una zona en la cual se pueden hacer los dibujos, en esta demostración se realizara un el cual no se guardara en ningún momento para así demostrar que podrá ser recuperable en caso de fallo en el software. 

#### Para hacer los guardados de seguridad decidí hacerlo en un archivo de texto en el cual se registrarán los pixeles de la imagen además de su color, el formato quedaría así: x|y|R|G|B

#### El guardado se realizara cada minuto, cuando se guarde el dibujo y cuando se cierre el programa.

#### En caso de un cierre inesperado se podrá acceder a la opción de abrir proyecto y elegir el archivo de seguridad últimamente creado, estos se nombran a base de la fecha y hora actual por lo cual será fácil identificar los más recientes.

#### Cuando se abra un archivo de seguridad se podrá recuperar el progreso últimamente guardado

<p align="center"> <img src="https://github.com/Zaikron/ManejoDeErrores_CToleranteFallas/blob/main/Imagenes/c4.PNG"/> </p>
