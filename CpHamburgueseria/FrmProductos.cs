using ClnHamburgueseria;
using CadHamburgueseria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpHamburgueseria
{
    public partial class FrmProductos : Form
    {
        private bool modoEdicion = false;
        private System.Threading.Timer searchTimer;
        private const int SearchDelay = 500; // milliseconds
        public FrmProductos()
        {
            InitializeComponent();
            pnlAgregar.Visible = false;
        }
        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            searchTimer?.Dispose();

            searchTimer = new System.Threading.Timer(_ =>
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (!string.IsNullOrWhiteSpace(txtBuscar.Text) || txtBuscar.Text == "")
                    {
                        listar();
                    }
                });
            }, null, SearchDelay, System.Threading.Timeout.Infinite);
        }
        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            txtBuscar.ForeColor = Color.Black;
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.ForeColor = Color.DimGray;
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }
        private void listar()
        { 
            var lista = ProductoCln.listarPa(txtBuscar.Text.Trim());
            dgvProductos.DataSource = lista;
            dgvProductos.Columns["id"].Visible = false;
            dgvProductos.Columns["idCategoria"].Visible = false;
            dgvProductos.Columns["estado"].Visible = false;
            dgvProductos.Columns["codigo"].HeaderText = "Código";
            dgvProductos.Columns["nombre"].HeaderText = "Nombre";
            dgvProductos.Columns["descripcion"].HeaderText = "Descripción";
            dgvProductos.Columns["categoria"].HeaderText = "Categoría";
            dgvProductos.Columns["saldo"].HeaderText = "Saldo";
            dgvProductos.Columns["precioVenta"].HeaderText = "Precio de Venta";
            dgvProductos.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvProductos.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
            if (lista.Count > 0) dgvProductos.CurrentCell = dgvProductos.Rows[0].Cells["codigo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void cargarCategorias()
        {
            var categorias = CategoriasCln.listar();
            cbxCategoria.DataSource = categorias;
            cbxCategoria.ValueMember = "id";
            cbxCategoria.DisplayMember = "nombre";
        }

        private void limpiar()
        { 
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            nudSaldo.Value = 0;
            nudPrecioVenta.Value = 0;
            cbxCategoria.SelectedIndex = -1;
        }

        private void mostrarPanelAgregar()
        {
            pnlAgregar.Visible = true;
            pnlAgregar.BringToFront();

            dgvProductos.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAgregarCategoria.Enabled = false;
            txtBuscar.Enabled = false;
        }

        private void ocultarPanelAgregar()
        { 
            pnlAgregar.Visible = false;

            dgvProductos.Enabled = true;
            btnAgregar.Enabled = true;
            btnEditar.Enabled = dgvProductos.Rows.Count > 0;
            btnEliminar.Enabled = dgvProductos.Rows.Count > 0;
            btnAgregarCategoria.Enabled = true;
            txtBuscar.Enabled = true;

            txtBuscar.Focus();
        }

        private bool validar()
        {
            bool esValido = true;
            erpCodigo.SetError(txtCodigo, "");
            erpNombre.SetError(txtNombre, "");
            erpDescripcion.SetError(txtDescripcion, "");
            erpCategoria.SetError(cbxCategoria, "");
            erpSaldo.SetError(nudSaldo, "");
            erpPrecioVenta.SetError(nudPrecioVenta, "");

            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                erpCodigo.SetError(txtCodigo, "El campo Codigo es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El campo Nombre es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                erpDescripcion.SetError(txtDescripcion, "El campo Descripcion es obligatorio");
                esValido = false;
            }

            if (string.IsNullOrEmpty(cbxCategoria.Text))
            {
                erpDescripcion.SetError(cbxCategoria, "El campo Categoria es obligatorio");
                esValido = false;
            }

            if (nudSaldo.Value <= 0)
            {
                erpSaldo.SetError(nudSaldo, "El campo Saldo no puede ser menor a 0");
                esValido = false;
            }

            if (nudPrecioVenta.Value <= 0)
            {
                erpPrecioVenta.SetError(nudPrecioVenta, "El campo Precio de Venta no puede ser menor a 0");
                esValido = false;
            }
            return esValido;
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            cargarCategorias();
            listar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            cargarCategorias();
            limpiar();
            mostrarPanelAgregar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int index = dgvProductos.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvProductos.Rows[index].Cells["id"].Value);
            var producto = ProductoCln.obtenerUno(id);
            txtCodigo.Text = producto.codigo;
            txtNombre.Text = producto.nombre;
            txtDescripcion.Text = producto.descripcion;
            cbxCategoria.SelectedValue = producto.idCategoria;
            nudSaldo.Value = producto.saldo;
            nudPrecioVenta.Value = producto.precioVenta;
            mostrarPanelAgregar();
            cargarCategorias();
            txtCodigo.Focus();
            modoEdicion = true;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var producto = new Producto();
                producto.codigo = txtCodigo.Text.Trim();
                producto.nombre = txtNombre.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.idCategoria = Convert.ToInt32(cbxCategoria.SelectedValue);
                producto.saldo = nudSaldo.Value;
                producto.precioVenta = nudPrecioVenta.Value;
                producto.usuarioRegistro = Util.usuario.usuario1;
                if (!modoEdicion)
                {
                    producto.fechaRegistro = DateTime.Now;
                    producto.estado = 1;
                    ProductoCln.insertar(producto);
                }
                else
                {
                    int index = dgvProductos.CurrentCell.RowIndex;
                    int id = Convert.ToInt32(dgvProductos.Rows[index].Cells["id"].Value);
                    var productoExistente = ProductoCln.obtenerUno(id);
                    productoExistente.codigo = txtCodigo.Text.Trim();
                    productoExistente.nombre = txtNombre.Text.Trim();
                    productoExistente.descripcion = txtDescripcion.Text.Trim();
                    productoExistente.idCategoria = Convert.ToInt32(cbxCategoria.SelectedValue);
                    productoExistente.saldo = nudSaldo.Value;
                    productoExistente.precioVenta = nudPrecioVenta.Value;
                    productoExistente.usuarioRegistro = Util.usuario.usuario1;
                    ProductoCln.actualizar(productoExistente);
                }
                ocultarPanelAgregar();
                limpiar();
                listar();
                MessageBox.Show("Producto guardado correctamente", "::: Cafeteria :::",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ocultarPanelAgregar();
        }

        private void btnCerrarAgregar_Click(object sender, EventArgs e)
        {
            ocultarPanelAgregar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvProductos.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvProductos.Rows[index].Cells["id"].Value);
            string nombre = dgvProductos.Rows[index].Cells["nombre"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar el Producto {nombre}?",
                "::: Cafeteria - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ProductoCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("El Producto se ha eliminado correctamente", "::: Cafeteria - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            new FrmCategorias().ShowDialog();
        }
    }
}
