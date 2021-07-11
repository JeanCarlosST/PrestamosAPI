using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.DAL;
using PrestamosAPI.Models;

namespace PrestamosAPI.Services{

    public class PersonasService{

        public static void Add(Personas persona){
            Contexto context = new Contexto();

            try{
                context.Personas.Add(persona);
                context.SaveChanges();
            
            } catch(Exception){
                throw;
            
            } finally{
                context.Dispose();
            }

        }

        public static void Update(Personas persona)
        {
            Contexto contexto = new Contexto();

            try
            {
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

        public static void Delete(int id)
        {
            Contexto contexto = new Contexto();

            try
            {
                var persona = Get(id);

                if (persona.PersonaID != 0)
                {
                    contexto.Personas.Remove(persona);
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

        public static Personas Get(int id)
        {
            Contexto contexto = new Contexto();
            var pizza = new Personas();

            try
            {
                pizza = contexto.Personas
                    .Include(p => p.Prestamos)
                    .FirstOrDefault(p => p.PersonaID == id);
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return pizza;
        }

        public static List<Personas> GetAll()
        {
            Contexto context = new Contexto();
            List<Personas> list = new List<Personas>();

            try
            {
                list = context.Personas
                    .Include(p => p.Prestamos)
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