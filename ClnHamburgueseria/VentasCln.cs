using CadHamburgueseria;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ClnHamburgueseria
{
    public class VentasCln
    {
        /// <summary>
        /// Inserta una venta con sus detalles en una única transacción.
        /// Devuelve el id de la venta insertada.
        /// Lanza excepción si hay error (se hace rollback automático).
        /// </summary>
        public static int insertarConDetalles(Ventas venta, List<DetalleVentas> detalles)
        {
            if (venta == null) throw new ArgumentNullException(nameof(venta));
            if (detalles == null) throw new ArgumentNullException(nameof(detalles));
            using (var context = new LabHamburgueseriaEntities())
            {
                using (var trx = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Insertar la venta (genera id)
                        context.Ventas.Add(venta);
                        context.SaveChanges();

                        // Agregar detalles y actualizar stock
                        foreach (var det in detalles)
                        {
                            // Preparar detalle
                            det.idVenta = venta.id;
                            det.usuarioRegistro = string.IsNullOrWhiteSpace(det.usuarioRegistro) ? venta.usuarioRegistro : det.usuarioRegistro;
                            if (det.fechaRegistro == default(DateTime)) det.fechaRegistro = DateTime.Now;
                            if (det.estado == 0) det.estado = 1;

                            // Comprobar y actualizar producto
                            var producto = context.Producto.Find(det.idProducto);
                            if (producto == null)
                                throw new InvalidOperationException($"Producto con id {det.idProducto} no encontrado.");

                            if (producto.saldo < det.cantidad)
                                throw new InvalidOperationException($"Stock insuficiente para el producto {producto.nombre} (id {producto.id}).");

                            producto.saldo -= det.cantidad;

                            // Agregar detalle
                            context.DetalleVentas.Add(det);
                        }

                        context.SaveChanges();
                        trx.Commit();
                        return venta.id;
                    }
                    catch
                    {
                        trx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
