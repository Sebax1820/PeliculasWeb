using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeliculasWeb.Data;
using PeliculasWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace PeliculasWeb.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly AppDbContext _context;

        public PeliculasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var appDbContext = from p in  _context.Peliculas
                .Include(p => p.Director)
                .Include(p => p.Genero)
                select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                appDbContext = appDbContext.Where(p => p.Titulo.Contains(searchString));
            }

            return View(await appDbContext.ToListAsync());
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Director)
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre");
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
            ViewData["Actores"] = new MultiSelectList(_context.Actores, "Id", "Nombre");
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula, int[] actoresSeleccionados)
        {
            if (ModelState.IsValid)
            {
                if (pelicula.ImagenArchivo != null && pelicula.ImagenArchivo.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(pelicula.ImagenArchivo.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await pelicula.ImagenArchivo.CopyToAsync(stream);
                    }
                    pelicula.ImagenRuta = "/imagenes/" + nombreArchivo;
                }


                pelicula.Actores = _context.Actores
                    .Where(a => actoresSeleccionados.Contains(a.Id))
                    .ToList();

                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            ViewData["Actores"] = new MultiSelectList(_context.Actores, "Id", "Nombre", actoresSeleccionados);
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.Include(p => p.Actores).FirstOrDefaultAsync(p => p.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            var actoresSeleccionados = pelicula.Actores.Select(a => a.Id).ToArray();
            ViewData["Actores"] = new MultiSelectList(_context.Actores, "Id", "Nombre", actoresSeleccionados);
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pelicula pelicula, int[] actoresSeleccionados)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            //Recuperar pelicula anterior de la base de datos
            var peliculaExistente = await _context.Peliculas
                .AsNoTracking()
                .Include(p => p.Actores)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (peliculaExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (pelicula.ImagenArchivo != null && pelicula.ImagenArchivo.Length > 0)
                    {
                        // Eliminar la imagen anterior del servidor
                        if (!string.IsNullOrEmpty(peliculaExistente.ImagenRuta))
                        {
                            var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", peliculaExistente.ImagenRuta.TrimStart('/'));
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                        }

                        // Guardar la nueva imagen
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(pelicula.ImagenArchivo.FileName);
                        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await pelicula.ImagenArchivo.CopyToAsync(stream);
                        }

                        pelicula.ImagenRuta = "/imagenes/" + nombreArchivo;
                    }
                    else
                    {
                        //Recuperar la imagen anterior de la base de datos
                        pelicula.ImagenRuta = peliculaExistente.ImagenRuta;
                    }
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();



                    if (peliculaExistente == null)
                    {
                        return NotFound();
                    }

                    peliculaExistente.Titulo = pelicula.Titulo;
                    peliculaExistente.Sinopsis = pelicula.Sinopsis;
                    peliculaExistente.Duracion = pelicula.Duracion;
                    peliculaExistente.AnioEstreno = pelicula.AnioEstreno;
                    peliculaExistente.GeneroId = pelicula.GeneroId;
                    peliculaExistente.DirectorId = pelicula.DirectorId;

                    peliculaExistente.Actores.Clear();

                    if (actoresSeleccionados != null && actoresSeleccionados.Length > 0)
                    {
                        var nuevosActores = await _context.Actores
                            .Where(a => actoresSeleccionados.Contains(a.Id))
                            .ToListAsync();

                        foreach (var actor in nuevosActores)
                        {
                            peliculaExistente.Actores.Add(actor);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            ViewData["Actores"] = new MultiSelectList(_context.Actores, "Id", "Nombre", actoresSeleccionados);
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Director)
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                //Eliminar la imagen del servidor
                if (!string.IsNullOrEmpty(pelicula.ImagenRuta))
                {
                    var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pelicula.ImagenRuta.TrimStart('/'));
                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }
                _context.Peliculas.Remove(pelicula);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
    }
}
