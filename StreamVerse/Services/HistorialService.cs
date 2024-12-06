using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVerse.Services
{
    public class HistorialService
    {
        // Lista observable para mantener el historial de películas
        public ObservableCollection<Pelicula> Peliculas { get; private set; }

        public HistorialService()
        {
            // Inicializamos con algunos datos de ejemplo (puedes eliminarlos después)
            Peliculas = new ObservableCollection<Pelicula>
            {
                new Pelicula { Titulo = "Inception", FechaVista = DateTime.Now.AddDays(-10) },
                new Pelicula { Titulo = "The Matrix", FechaVista = DateTime.Now.AddDays(-5) },
                new Pelicula { Titulo = "Avatar", FechaVista = DateTime.Now.AddDays(-2) }
            };
        }

        // Método para agregar una nueva película al historial
        public void AgregarPelicula(string titulo)
        {
            Peliculas.Add(new Pelicula
            {
                Titulo = titulo,
                FechaVista = DateTime.Now
            });
        }
    }

    // Clase para representar las películas
    public class Pelicula
    {
        public string Titulo { get; set; }
        public DateTime FechaVista { get; set; }
    }
}
