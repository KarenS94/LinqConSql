using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace LinqConSql
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dataContext;
        //HACEMOS LA CONEXION
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqConSql.Properties.Settings.PractSqlConnectionString"].ConnectionString;
            dataContext = new DataClasses1DataContext(connectionString);

            // AgregarUniversidad();
            // AgregarEstudiante();
            // AgregarMaterias();
            //AgregarAsociacionesEstudiantesMateria();
            //ObtenerUniversidadDeKaren();
            //ObtenerMateriaDeKaren();
            // ObtenerEstudiantesdeUNC();
            // ObtenerUniversidadesFemenino();
            //ObtenerMateriasdeUCA();
            //ActualizarNombre();
            EliminarDatos();
        }
        // creamos un metodo que se encargue de agregar universidades en la tabla universidad
        public void AgregarUniversidad()
        {
            dataContext.ExecuteCommand("Delete from Universidad");

            Universidad UNC = new Universidad();
            UNC.Nombre = "UNC";
      
            //AGREGAMOS UNC A LA TABLA:
            dataContext.Universidad.InsertOnSubmit(UNC);
            //para agregar otra univ

            Universidad UCA = new Universidad();
            UCA.Nombre = "UCA";
            dataContext.Universidad.InsertOnSubmit(UCA);
            // Guardar 
            dataContext.SubmitChanges();
            //Mostrarlo en pantalla, mandamos a llamar a la GRID
            DataGridPrincipal.ItemsSource = dataContext.Universidad;
           
        }
        public void AgregarEstudiante()
        {
            Universidad UNC = dataContext.Universidad.First(un => un.Nombre.Equals("UNC"));
            Universidad UCA = dataContext.Universidad.First(un => un.Nombre.Equals("UCA"));

            List<Estudiante> estudiantes = new List<Estudiante>();  
            estudiantes.Add(new Estudiante { Nombre = "Karen", Genero = "Femenino", Universidad = UCA});
            estudiantes.Add(new Estudiante { Nombre = "Santos", Genero = "Masculino", Universidad = UCA});
            estudiantes.Add(new Estudiante { Nombre = "Ana", Genero = "Femenino", Universidad = UNC});
            estudiantes.Add(new Estudiante { Nombre = "Allan de Jesus", Genero = "Masculino", Universidad = UNC});
            estudiantes.Add(new Estudiante { Nombre = "Andrea", Genero = "Femenino", Universidad = UCA});
            
            dataContext.Estudiante.InsertAllOnSubmit(estudiantes);
            dataContext.SubmitChanges();
           
            DataGridPrincipal.ItemsSource = dataContext.Estudiante;
        
        
        }
        public void AgregarMaterias()
        {
            dataContext.ExecuteCommand("Delete from Materia");

            //Materia matematica = new Materia();
            //matematica.Nombre = "Matematica";
            //Materia ingles = new Materia();
            //ingles.Nombre = "Ingles";

            dataContext.Materia.InsertOnSubmit(new Materia { Nombre = "Matematica" });
            dataContext.Materia.InsertOnSubmit(new Materia { Nombre = "Ingles" });
            dataContext.Materia.InsertOnSubmit(new Materia { Nombre = "Español" });
           
            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.Materia;

        }

        public void AgregarAsociacionesEstudiantesMateria()
        {
            Estudiante Karen = dataContext.Estudiante.First(es => es.Nombre.Equals("Karen"));
            Estudiante Santos = dataContext.Estudiante.First(es => es.Nombre.Equals("Santos"));
            Estudiante Ana = dataContext.Estudiante.First(es => es.Nombre.Equals("Ana"));
            Estudiante AllandeJesus = dataContext.Estudiante.First(es => es.Nombre.Equals("Allan de Jesus"));
            Estudiante Andrea = dataContext.Estudiante.First(es => es.Nombre.Equals("Andrea"));

            Materia Matematica = dataContext.Materia.First(mat => mat.Nombre.Equals("Matematica"));
            Materia Ingles = dataContext.Materia.First(mat => mat.Nombre.Equals("Ingles"));
            Materia Español = dataContext.Materia.First(mat => mat.Nombre.Equals("Español"));

            dataContext.EstudianteMateria.InsertOnSubmit(new EstudianteMateria { Estudiante = Karen, Materia = Matematica });
            dataContext.EstudianteMateria.InsertOnSubmit(new EstudianteMateria { Estudiante = Santos, Materia = Ingles });

            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.EstudianteMateria;
        }
        public void ObtenerUniversidadDeKaren()
        {
            Estudiante karen = dataContext.Estudiante.First(es => es.Nombre.Equals("Karen"));
            Universidad karenUni = karen.Universidad;

            List<Universidad> universidades = new List<Universidad>();
            universidades.Add(karenUni);

            DataGridPrincipal.ItemsSource = universidades;
        }

        public void ObtenerMateriaDeKaren()
        {
            Estudiante matKaren = dataContext.Estudiante.First(es => es.Nombre.Equals("Karen"));
            var materiaKaren = from em in matKaren.EstudianteMateria select em.Materia;

            DataGridPrincipal.ItemsSource = materiaKaren;

        }

        public void ObtenerEstudiantesdeUNC()
        {
            var estudianteUNC = from estudiante in dataContext.Estudiante where estudiante.Universidad.Nombre == "UNC" select estudiante;
            DataGridPrincipal.ItemsSource = estudianteUNC;
        }

        public void ObtenerUniversidadesFemenino()
        {
            var estudiantesFemenino = from estudiante in dataContext.Estudiante
                                      join
                                      universidad in dataContext.Universidad on
                                      estudiante.Universidad equals universidad
                                      where estudiante.Genero == "Femenino"
                                      select universidad;
            DataGridPrincipal.ItemsSource = estudiantesFemenino;
        }
    
        public void ObtenerMateriasdeUCA()
        {
            var materiasRegistradasEnUCA = from em in dataContext.EstudianteMateria
                                           join estudiante in dataContext.Estudiante
                                           on em.EstudianteId equals estudiante.Id
                                           where estudiante.Universidad.Nombre == "UCA"
                                           select em.Materia;
                                           
            DataGridPrincipal.ItemsSource = materiasRegistradasEnUCA;
        }

        // ACTUALIZAR DATOS

        public void ActualizarNombre()
        {
            Estudiante santos = dataContext.Estudiante.FirstOrDefault( es => es.Nombre == "santos");
            santos.Nombre = " Santos Gilberto";

            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.Estudiante;
        }

        //ELIMINAR NOMBRE DE ESTUDIANTE
        public void EliminarDatos()
        {
            Estudiante ana = dataContext.Estudiante.FirstOrDefault(es => es.Nombre == "Ana");

            dataContext.Estudiante.DeleteOnSubmit(ana);
            dataContext.SubmitChanges();

            DataGridPrincipal.ItemsSource = dataContext.Estudiante;
        }
    }

}
