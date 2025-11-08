using CadHamburgueseria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClnHamburgueseria
{
    public class ProductoCln
    {
        public static int insertar(Producto producto)
        {
            using (var context = new LabHamburgueseriaEntities())
            { 
                context.Producto.Add(producto);
                context.SaveChanges();
                return producto.IdProducto;
            }
        }
        public static int actualizar(Producto producto)
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                var existente = context.Producto.Find(producto.IdProducto);
                existente.IdProducto = producto.IdProducto;
                existente.Codigo = producto.Codigo;
                existente.Nombre = producto.Nombre;
                existente.Descripcion = producto.Descripcion;
                existente.IdCategoria = producto.IdCategoria;
                existente.Stock = producto.Stock;
                existente.PrecioVenta = producto.PrecioVenta;
                return context.SaveChanges();
            }
        }
        public static int eliminar(int idProducto)
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                var producto = context.Producto.Find(idProducto);
                context.Producto.Remove(producto);
                return context.SaveChanges();
            }
        }
        public static Producto obtenerUno(int idProducto)
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                return context.Producto.Find(idProducto);
            }
        }
        public static List<Producto> listar()
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                return context.Producto.Where(x => x.estado != -1).ToList();
            }
        }
        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new LabHamburgueseriaEntities())
            {
                return context.paProductoListar(parametro).ToList();
            }
        }
    }
}
