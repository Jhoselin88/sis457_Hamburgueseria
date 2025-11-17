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
    public partial class FrmVentas : Form
    {
        private System.Threading.Timer searchTimer;
        private const int SearchDelay = 500; // Delay in milliseconds
        public FrmVentas()
        {
            InitializeComponent();
            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void listar()
        {
            var lista = DetalleVentasCln.listarPa(txtBuscar.Text.Trim());
            dgvDetalleVentas.DataSource = lista;
            dgvDetalleVentas.Columns["id"].Visible = false;
            dgvDetalleVentas.Columns["idVenta"].Visible = false;
            dgvDetalleVentas.Columns["estado"].Visible = false;
            dgvDetalleVentas.Columns["cantidad"].HeaderText = "Cantidad";
            dgvDetalleVentas.Columns["precioUnitario"].HeaderText = "Precio Unitario";
            dgvDetalleVentas.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            listar();
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

        private void FrmVentas_FormClosing(object sender, FormClosingEventArgs e)
        {
            searchTimer?.Dispose();
        }
    }
}
