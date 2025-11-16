using CadHamburgueseria;
using ClnHamburgueseria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpHamburgueseria
{
    public partial class FrmClientes : Form
    {
        private bool modoEdicion = false;
        private System.Threading.Timer searchTimer;
        private const int SearchDelay = 500; // milliseconds
        public FrmClientes()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var lista = ClientesCln.listarPa(txtBuscar.Text.Trim());
            dgvClientes.DataSource = lista;
            dgvClientes.Columns["id"].Visible = false;
            dgvClientes.Columns["estado"].Visible = false;
            dgvClientes.Columns["cedulaIdentidad"].HeaderText = "Cédula de Identidad";
            dgvClientes.Columns["nombres"].HeaderText = "Nombres";
            dgvClientes.Columns["apellidos"].HeaderText = "Apellidos";
            dgvClientes.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvClientes.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
            if (lista.Count > 0) dgvClientes.CurrentCell = dgvClientes.Rows[0].Cells["cedulaIdentidad"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
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

        private void FrmClientes_FormClosing(object sender, FormClosingEventArgs e)
        {
            searchTimer?.Dispose();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            listar();
            pnlAgregar.Visible = false;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtCedulaIdentidad.KeyPress += Util.onlyNumbers;
            txtNombres.KeyPress += Util.onlyLetters;
            txtApellidos.KeyPress += Util.onlyLetters;
        }
        private void limpiar()
        {
            txtCedulaIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
        }
        private void mostrarPanelAgregar()
        {
            pnlAgregar.Visible = true;
            pnlAgregar.BringToFront();

            dgvClientes.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            txtBuscar.Enabled = false;
        }
        private void ocultarPanelAgregar()
        {
            pnlAgregar.Visible = false;

            dgvClientes.Enabled = true;
            btnAgregar.Enabled = true;
            btnEditar.Enabled = dgvClientes.Rows.Count > 0;
            btnEliminar.Enabled = dgvClientes.Rows.Count > 0;
            txtBuscar.Enabled = true;

            txtBuscar.Focus();
        }
        private bool validar()
        {
            bool esValido = true;
            erpCedulaIdentidad.SetError(txtCedulaIdentidad, "");
            erpNombres.SetError(txtNombres, "");
            erpApellidos.SetError(txtApellidos, "");

            if (string.IsNullOrEmpty(txtCedulaIdentidad.Text))
            {
                erpCedulaIdentidad.SetError(txtCedulaIdentidad, "El campo CI es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtNombres.Text))
            {
                erpNombres.SetError(txtNombres, "El campo Nombres es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtApellidos.Text) && string.IsNullOrEmpty(txtApellidos.Text))
            {
                erpApellidos.SetError(txtApellidos, "Debe introducir al un apellido");
                esValido = false;
            }
            return esValido;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            modoEdicion = false; ;
            limpiar();
            mostrarPanelAgregar();
            txtCedulaIdentidad.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var cliente = new Cliente();
                cliente.cedulaIdentidad = txtCedulaIdentidad.Text.Trim();
                cliente.nombres = txtNombres.Text.Trim();
                cliente.apellidos = txtApellidos.Text.Trim();
                cliente.usuarioRegistro = Util.usuario.usuario1;

                if (!modoEdicion)
                {
                    cliente.fechaRegistro = DateTime.Now;
                    cliente.estado = 1;
                    ClientesCln.insertar(cliente);
                }
                else
                {
                    int index = dgvClientes.CurrentCell.RowIndex;
                    cliente.id = Convert.ToInt32(dgvClientes.Rows[index].Cells["id"].Value);
                    ClientesCln.actualizar(cliente);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Registro guardado correctamente", "::: Hamburgueseria - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int index = dgvClientes.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvClientes.Rows[index].Cells["id"].Value);
            var cliente = ClientesCln.obtenerUno(id);
            txtCedulaIdentidad.Text = cliente.cedulaIdentidad;
            txtNombres.Text = cliente.nombres;
            txtApellidos.Text = cliente.apellidos;
            mostrarPanelAgregar();
            txtCedulaIdentidad.Focus();
            modoEdicion = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvClientes.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvClientes.Rows[index].Cells["id"].Value);
            string ci = dgvClientes.Rows[index].Cells["cedulaIdentidad"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar al Cliente con {ci}?",
                "::: Hamburgueseria - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ClientesCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("El Producto se ha eliminado correctamente", "::: Hamburgueseria - Mensaje :::",
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
    }
}
