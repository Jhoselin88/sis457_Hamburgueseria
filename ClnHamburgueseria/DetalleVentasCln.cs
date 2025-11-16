using CadHamburgueseria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnHamburgueseria
{
    public class DetalleVentasCln
    {
        public static int insertar(DetalleVentas detalle)
        {
            using (var context = new LabHamburgueseriaEntities())
            { 
                context.DetalleVentas.Add(detalle);
                context.SaveChanges();
                return detalle.id;
            }
        }
        public static List<DetalleVentas> obtenerPorVenta(int idVenta)
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                return context.DetalleVentas.Where(x => x.idVenta == idVenta).ToList();
            }
        }
    }
}
