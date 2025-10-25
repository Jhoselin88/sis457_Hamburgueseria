namespace CpHamburgueseria
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.pnMenu = new System.Windows.Forms.Panel();
            this.pnContenedor = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.paBarraTitulo = new System.Windows.Forms.Panel();
            this.btnDeslizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnReportePedidos = new System.Windows.Forms.Button();
            this.btnPedidos = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnEmpleados = new System.Windows.Forms.Button();
            this.btnProductos = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnMenu.SuspendLayout();
            this.pnContenedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.paBarraTitulo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnMenu
            // 
            this.pnMenu.BackColor = System.Drawing.Color.Gray;
            this.pnMenu.Controls.Add(this.panel2);
            this.pnMenu.Controls.Add(this.panel3);
            this.pnMenu.Controls.Add(this.panel8);
            this.pnMenu.Controls.Add(this.panel5);
            this.pnMenu.Controls.Add(this.panel1);
            this.pnMenu.Controls.Add(this.btnReportePedidos);
            this.pnMenu.Controls.Add(this.btnPedidos);
            this.pnMenu.Controls.Add(this.btnClientes);
            this.pnMenu.Controls.Add(this.btnEmpleados);
            this.pnMenu.Controls.Add(this.btnProductos);
            this.pnMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnMenu.Location = new System.Drawing.Point(0, 0);
            this.pnMenu.Name = "pnMenu";
            this.pnMenu.Size = new System.Drawing.Size(236, 650);
            this.pnMenu.TabIndex = 2;
            // 
            // pnContenedor
            // 
            this.pnContenedor.Controls.Add(this.pictureBox1);
            this.pnContenedor.Location = new System.Drawing.Point(236, 50);
            this.pnContenedor.Name = "pnContenedor";
            this.pnContenedor.Size = new System.Drawing.Size(1064, 600);
            this.pnContenedor.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CpHamburgueseria.Properties.Resources.fondo;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1064, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // paBarraTitulo
            // 
            this.paBarraTitulo.BackColor = System.Drawing.Color.PapayaWhip;
            this.paBarraTitulo.Controls.Add(this.btnDeslizar);
            this.paBarraTitulo.Controls.Add(this.btnMaximizar);
            this.paBarraTitulo.Controls.Add(this.btnRestaurar);
            this.paBarraTitulo.Controls.Add(this.btnMinimizar);
            this.paBarraTitulo.Controls.Add(this.btnCerrar);
            this.paBarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.paBarraTitulo.Location = new System.Drawing.Point(236, 0);
            this.paBarraTitulo.Name = "paBarraTitulo";
            this.paBarraTitulo.Size = new System.Drawing.Size(1064, 50);
            this.paBarraTitulo.TabIndex = 5;
            this.paBarraTitulo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.paBarraTitulo_MouseDoubleClick);
            this.paBarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.paBarraTitulo_MouseDown);
            // 
            // btnDeslizar
            // 
            this.btnDeslizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeslizar.FlatAppearance.BorderSize = 0;
            this.btnDeslizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeslizar.Image = ((System.Drawing.Image)(resources.GetObject("btnDeslizar.Image")));
            this.btnDeslizar.Location = new System.Drawing.Point(3, 6);
            this.btnDeslizar.Name = "btnDeslizar";
            this.btnDeslizar.Size = new System.Drawing.Size(35, 35);
            this.btnDeslizar.TabIndex = 9;
            this.btnDeslizar.UseVisualStyleBackColor = true;
            this.btnDeslizar.Click += new System.EventHandler(this.btnDeslizar_Click);
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMaximizar.Image")));
            this.btnMaximizar.Location = new System.Drawing.Point(999, 11);
            this.btnMaximizar.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(25, 25);
            this.btnMaximizar.TabIndex = 8;
            this.btnMaximizar.UseVisualStyleBackColor = true;
            this.btnMaximizar.Click += new System.EventHandler(this.btnMaximizar_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestaurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestaurar.FlatAppearance.BorderSize = 0;
            this.btnRestaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar.Image = ((System.Drawing.Image)(resources.GetObject("btnRestaurar.Image")));
            this.btnRestaurar.Location = new System.Drawing.Point(999, 11);
            this.btnRestaurar.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(25, 25);
            this.btnRestaurar.TabIndex = 7;
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Visible = false;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimizar.Image")));
            this.btnMinimizar.Location = new System.Drawing.Point(974, 11);
            this.btnMinimizar.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(25, 25);
            this.btnMinimizar.TabIndex = 6;
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(1024, 11);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(0);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(25, 25);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnReportePedidos
            // 
            this.btnReportePedidos.FlatAppearance.BorderSize = 0;
            this.btnReportePedidos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnReportePedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportePedidos.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportePedidos.ForeColor = System.Drawing.Color.PapayaWhip;
            this.btnReportePedidos.Image = ((System.Drawing.Image)(resources.GetObject("btnReportePedidos.Image")));
            this.btnReportePedidos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReportePedidos.Location = new System.Drawing.Point(2, 301);
            this.btnReportePedidos.Name = "btnReportePedidos";
            this.btnReportePedidos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnReportePedidos.Size = new System.Drawing.Size(250, 40);
            this.btnReportePedidos.TabIndex = 31;
            this.btnReportePedidos.Text = "       Reporte Pedidos";
            this.btnReportePedidos.UseVisualStyleBackColor = true;
            // 
            // btnPedidos
            // 
            this.btnPedidos.FlatAppearance.BorderSize = 0;
            this.btnPedidos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnPedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPedidos.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPedidos.ForeColor = System.Drawing.Color.PapayaWhip;
            this.btnPedidos.Image = ((System.Drawing.Image)(resources.GetObject("btnPedidos.Image")));
            this.btnPedidos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPedidos.Location = new System.Drawing.Point(2, 242);
            this.btnPedidos.Name = "btnPedidos";
            this.btnPedidos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnPedidos.Size = new System.Drawing.Size(250, 40);
            this.btnPedidos.TabIndex = 30;
            this.btnPedidos.Text = "Pedidos";
            this.btnPedidos.UseVisualStyleBackColor = true;
            // 
            // btnClientes
            // 
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientes.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientes.ForeColor = System.Drawing.Color.PapayaWhip;
            this.btnClientes.Image = ((System.Drawing.Image)(resources.GetObject("btnClientes.Image")));
            this.btnClientes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClientes.Location = new System.Drawing.Point(2, 424);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnClientes.Size = new System.Drawing.Size(250, 40);
            this.btnClientes.TabIndex = 29;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            // 
            // btnEmpleados
            // 
            this.btnEmpleados.FlatAppearance.BorderSize = 0;
            this.btnEmpleados.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnEmpleados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmpleados.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpleados.ForeColor = System.Drawing.Color.PapayaWhip;
            this.btnEmpleados.Image = ((System.Drawing.Image)(resources.GetObject("btnEmpleados.Image")));
            this.btnEmpleados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmpleados.Location = new System.Drawing.Point(2, 361);
            this.btnEmpleados.Name = "btnEmpleados";
            this.btnEmpleados.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnEmpleados.Size = new System.Drawing.Size(250, 40);
            this.btnEmpleados.TabIndex = 28;
            this.btnEmpleados.Text = "Empleados";
            this.btnEmpleados.UseVisualStyleBackColor = true;
            // 
            // btnProductos
            // 
            this.btnProductos.FlatAppearance.BorderSize = 0;
            this.btnProductos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductos.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductos.ForeColor = System.Drawing.Color.PapayaWhip;
            this.btnProductos.Image = ((System.Drawing.Image)(resources.GetObject("btnProductos.Image")));
            this.btnProductos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductos.Location = new System.Drawing.Point(2, 186);
            this.btnProductos.Name = "btnProductos";
            this.btnProductos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnProductos.Size = new System.Drawing.Size(250, 40);
            this.btnProductos.TabIndex = 27;
            this.btnProductos.Text = "Productos";
            this.btnProductos.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel2.Location = new System.Drawing.Point(1, 301);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(8, 40);
            this.panel2.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel3.Location = new System.Drawing.Point(1, 243);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 40);
            this.panel3.TabIndex = 36;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel8.Location = new System.Drawing.Point(1, 424);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(8, 40);
            this.panel8.TabIndex = 34;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel5.Location = new System.Drawing.Point(1, 362);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(8, 40);
            this.panel5.TabIndex = 33;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel1.Location = new System.Drawing.Point(1, 187);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(8, 40);
            this.panel1.TabIndex = 32;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 650);
            this.Controls.Add(this.paBarraTitulo);
            this.Controls.Add(this.pnContenedor);
            this.Controls.Add(this.pnMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Principal";
            this.pnMenu.ResumeLayout(false);
            this.pnContenedor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.paBarraTitulo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMenu;
        private System.Windows.Forms.Panel pnContenedor;
        private System.Windows.Forms.Panel paBarraTitulo;
        private System.Windows.Forms.Button btnDeslizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnReportePedidos;
        private System.Windows.Forms.Button btnPedidos;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnEmpleados;
        private System.Windows.Forms.Button btnProductos;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
    }
}

