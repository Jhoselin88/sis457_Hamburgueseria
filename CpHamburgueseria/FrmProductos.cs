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
    public partial class FrmProductos : Form
    {
        public FrmProductos()
        {
            InitializeComponent();
            pnlAgregar.Visible = false;
        }
        private void listar()
        { 
            var lista = ProductoCln.listarPa(txtBuscar.Text.Trim());
            dgvProductos.DataSource = lista;
            
            dgvProductos.Columns["estado"].Visible = false;
            dgvProductos.Columns["idCategoria"].Visible = false;
            dgvProductos.Columns["codigo"].HeaderText = "Código";
            dgvProductos.Columns["nombre"].HeaderText = "Nombre";
            dgvProductos.Columns["descripcion"].HeaderText = "Descripción";

            dgvProductos.Columns["precioVenta"].HeaderText = "Precio de Venta";
            if(lista.Count > 0) dgvProductos.CurrentCell = dgvProductos.Rows[0].Cells["codigo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            listar();
        }
    }
}
