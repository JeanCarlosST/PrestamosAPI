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
    public class MorasService
    {
        public static void Add(Moras mora)
        {
            Contexto contexto = new Contexto();

            try
            {
                contexto.Moras.Add(mora);

                List<MorasDetalle> detalles = mora.Detalle;
                foreach (MorasDetalle d in detalles)
                {
                    Prestamos prestamo = contexto.Prestamos.Find(d.PrestamoID);
                    prestamo.Mora += d.Valor;
                    contexto.Entry(prestamo).State = EntityState.Modified;
                }

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

        public static void Update(Moras mora)
        {
            Contexto contexto = new Contexto();

            try
            {
                List<MorasDetalle> viejosDetalles = Get(mora.MoraID).Detalle;
                foreach (MorasDetalle d in viejosDetalles)
                {
                    Prestamos prestamo = contexto.Prestamos.Find(d.PrestamoID);
                    prestamo.Mora -= d.Valor;
                    contexto.Entry(prestamo).State = EntityState.Modified;
                }

                contexto.Database.ExecuteSqlRaw($"delete from MorasDetalle where MoraID = {mora.MoraID}");
                foreach (var anterior in mora.Detalle)
                {
                    contexto.Entry(anterior).State = EntityState.Added;
                }

                List<MorasDetalle> nuevosDetalles = mora.Detalle;
                foreach (MorasDetalle d in nuevosDetalles)
                {
                    Prestamos prestamo = contexto.Prestamos.Find(d.PrestamoID);
                    prestamo.Mora += d.Valor;
                    contexto.Entry(prestamo).State = EntityState.Modified;
                }

                contexto.Entry(mora).State = EntityState.Modified;
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
                var mora = contexto.Moras.Find(id);

                if (mora != null)
                {
                    List<MorasDetalle> viejosDetalles = Get(mora.MoraID).Detalle;
                    foreach (MorasDetalle d in viejosDetalles)
                    {
                        Prestamos prestamo = contexto.Prestamos.Find(d.PrestamoID);
                        prestamo.Mora -= d.Valor;
                        contexto.Entry(prestamo).State = EntityState.Modified;
                    }

                    contexto.Entry(mora).State = EntityState.Deleted;
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

        public static Moras Get(int id)
        {
            Contexto contexto = new Contexto();
            var mora = new Moras();

            try
            {
                mora = contexto.Moras
                    .Include(m => m.Detalle)
                    .FirstOrDefault(m => m.MoraID == id);
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return mora;
        }

        public static List<Moras> GetAll()
        {
            Contexto context = new Contexto();
            List<Moras> list = new List<Moras>();

            try
            {
                list = context.Moras
                    .Include(m => m.Detalle)
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
