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
            using (var context = new LabHamburgueseriaEntities1())
            { 
                context.Producto.Add(producto);
                context.SaveChanges();
                return producto.id;
            }
        }
        public static int actualizar(Producto producto)
        {
            using (var context = new LabHamburgueseriaEntities1())
            {
                var existente = context.Producto.Find(producto.id);
                existente.id = producto.id;
                existente.codigo = producto.codigo;
                existente.nombre = producto.nombre;
                existente.descripcion = producto.descripcion;
                existente.saldo = producto.saldo;
                existente.precioVenta = producto.precioVenta;
                existente.usuarioRegistro = producto.usuarioRegistro;
                return context.SaveChanges();
            }
        }
        public static int eliminar(int idProducto)
        {
            using (var context = new LabHamburgueseriaEntities1())
            {
                var producto = context.Producto.Find(idProducto);
                context.Producto.Remove(producto);
                return context.SaveChanges();
            }
        }
        public static Producto obtenerUno(int idProducto)
        {
            using (var context = new LabHamburgueseriaEntities1())
            {
                return context.Producto.Find(idProducto);
            }
        }
        public static List<Producto> listar()
        {
            using (var context = new LabHamburgueseriaEntities1())
            {
                return context.Producto.Where(x => x.estado != -1).ToList();
            }
        }
        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new LabHamburgueseriaEntities1())
            {
                return context.paProductoListar(parametro).ToList();
            }
        }
    }
}
