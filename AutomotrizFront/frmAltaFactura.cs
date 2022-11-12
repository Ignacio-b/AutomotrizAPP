using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomotrizApp.dominio;
using AutomotrizFront.Http;
using Newtonsoft.Json;

namespace AutomotrizApp.formularios
{
    public partial class frmAltaFactura : Form
    {
        private Factura nuevo;
        public frmAltaFactura()
        {
            InitializeComponent();

            nuevo = new Factura();
            
        }

        private void frmAltaFactura_Load(object sender, EventArgs e)
        {
            ProximaFactura();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            CargarClientesAsync();
            

        }

        private async void CargarTiposClientesAsync()
        {
            string url = "https://localhost:5001/TiposCliente";
            var result = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Vehiculo>>(result);
            cboTipoCliente.DataSource = lst;
            cboTipoCliente.DisplayMember = "Tipo";
            cboTipoCliente.ValueMember = "id_tipo";
        }

        private async void ProximaFactura()
        {
            string url = "https://localhost:5001/ProximoID";
            var result = await ClientSingleton.GetInstance().GetAsync(url);
            string next = JsonConvert.DeserializeObject<string>(result); 
            //if (next =='0')
                lblFactura.Text = "Factura Nº:" + next;
            //else
            //    MessageBox.Show("Error de datos. No se puede obtener Nº de presupuesto!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void CargarArticulosAsync(string tabla)
        {
            if(tabla == "Vehiculos")
            {
                string url = "http://localhost:5031/Vehiculos";
                var result = await ClientSingleton.GetInstance().GetAsync(url);
                var lst = JsonConvert.DeserializeObject<List<Vehiculo>>(result);
                cboArticulos.DataSource = lst;
                cboArticulos.DisplayMember = "Nombre";
                cboArticulos.ValueMember = "Nro";
            }
            else
            {
                string url = "http://localhost:5031/Autopartes";
                var result = await ClientSingleton.GetInstance().GetAsync(url);
                var lst = JsonConvert.DeserializeObject<List<Vehiculo>>(result);
                cboArticulos.DataSource = lst;
                cboArticulos.DisplayMember = "Nombre";
                cboArticulos.ValueMember = "Nro";
            }
            


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArticulosAsync(cboItem.Text.ToString());
        }

        private void cboItem_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cboTipoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarClientesAsync();
        }

        private async void CargarClientesAsync()
        {
            string url = "https://localhost:5001/Clientes";
            var result = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Vehiculo>>(result);
            cboClientes.DataSource = lst;
            cboClientes.DisplayMember = "Nombre";
            cboClientes.ValueMember = "id_cliente";

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboArticulos.Text.Equals(String.Empty))
            {
                MessageBox.Show("Debe seleccionar un Articulo!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCantidad.Text == "" || !int.TryParse(txtCantidad.Text, out _))
            {
                MessageBox.Show("Debe ingresar una cantidad válida!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["colArt"].Value.ToString().Equals(cboArticulos.Text))
                {
                    MessageBox.Show("Articulo: " + cboArticulos.Text + " ya se encuentra dentro de la Factura!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;

                }
            }

            Vehiculo v = (Vehiculo)cboArticulos.SelectedItem;
            int cantidad = Convert.ToInt32(txtCantidad.Text);

            DetalleFactura detalle = new DetalleFactura(v,null,cantidad,0);
            nuevo.AgregarDetalle(detalle);
            dgvDetalles.Rows.Add(new object[] { v.VehiculoNro, v.Modelo, v.Precio, txtCantidad.Text });

            //CalcularTotal();
        }
    }
}
