using Microsoft.EntityFrameworkCore;
using PrestamosAPI.DAL;
using PrestamosAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrestamosAPI.Services
{
    public class PrestamosService
    {
        public static void Add(Prestamos prestamo)
        {
            Contexto contexto = new Contexto();

            try
            {
                prestamo.Balance = prestamo.Monto;
                contexto.Prestamos.Add(prestamo);

                Personas persona = PersonasService.Get(prestamo.PersonaID);
                persona.Balance += prestamo.Monto;
                contexto.Entry(persona).State = EntityState.Modified;

                contexto.SaveChanges();
            }
            catch
            {
                throw;

            }
            finally
            {
                contexto.Dispose();
            }
        }
        public static bool Update(Prestamos prestamo)
        {
            Contexto contexto = new Contexto();
            bool found = false;

            try
            {
                prestamo.Balance = prestamo.Monto;
                Prestamos viejoPrestamo = contexto.Prestamos.Find(prestamo.PrestamoID);
                float nuevoMonto = prestamo.Monto - viejoPrestamo.Monto;

                Personas persona = PersonasService.Get(prestamo.PersonaID);
                persona.Balance += nuevoMonto;
                contexto.Entry(persona).State = EntityState.Modified;
                contexto.Entry(prestamo).State = EntityState.Modified;

                found = contexto.SaveChanges() > 0;
            }
            catch
            {
                throw;

            }
            finally
            {
                contexto.Dispose();
            }

            return found;
        }
        public static void Delete(int id)
        {
            Contexto contexto = new Contexto();

            try
            {
                var prestamo = contexto.Prestamos.Find(id);

                if (prestamo != null)
                {
                    Personas persona = PersonasService.Get(prestamo.PersonaID);
                    persona.Balance -= prestamo.Monto;
                    contexto.Entry(persona).State = EntityState.Modified;

                    contexto.Prestamos.Remove(prestamo);
                    contexto.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public static Prestamos Get(int id)
        {
            Contexto contexto = new Contexto();
            var prestamo = new Prestamos();

            try
            {
                prestamo = contexto.Prestamos
                    .FirstOrDefault(p => p.PrestamoID == id);
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return prestamo;
        }

        public static List<Prestamos> GetAll()
        {
            Contexto context = new Contexto();
            List<Prestamos> list = new List<Prestamos>();

            try
            {
                list = context.Prestamos
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return list;
        }
    }
}
