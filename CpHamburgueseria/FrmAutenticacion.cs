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
using ClnHamburgueseria;

namespace CpHamburgueseria
{
    public partial class FrmAutenticacion : Form
    {
        public FrmAutenticacion()
        {
            InitializeComponent();
        }
        [DllImport("user32.Dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.Dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var usuario = UsuarioCln.validar(txtUsuario.Text, txtClave.Text);
            if (usuario != null) 
            {
                new FrmPrincipal(this).Show();
            }
            else
            {
                MessageBox.Show("Usuario o clave incorrecta", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
