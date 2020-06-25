using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LocalicaPalavraChave
{
    public partial class FrmTela : Form
    {
        public FrmTela()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Evento dos Botões
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            DesabilitarBotoes();
            var caminho = txtCaminho.Text;
            var filtro = !string.IsNullOrEmpty(txtFiltro.Text) ? txtFiltro.Text.Split(';') : new string[1];
            var listaResultado = new List<string>();
            var arquivos = Directory.GetFiles(@caminho, "*.*", SearchOption.AllDirectories)
                .Where(s => !s.EndsWith(".dll")
                && !s.EndsWith(".csproj")
                && !s.EndsWith(".git")
                && !s.EndsWith(".snk")
                && !s.EndsWith(".png")
                && !s.EndsWith(".git")
                && !s.EndsWith(".gif")
                && !s.EndsWith(".jpg")
                && !s.EndsWith(".css")
                && !s.EndsWith(".config")).ToList();

            if (!string.IsNullOrEmpty(txtFiltro.Text))
            {
                LerArquivos(filtro, arquivos);
            }

            HabilitarBotoes();
        }

        private void bntLimpar_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblResultado.Text);
        }

        private void LerArquivos(string[] filtro, List<string> arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                try
                {
                    var stream = new FileStream(arquivo, FileMode.Open);

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string linha = default;
                        // Lê linha por linha
                        while ((linha = sr.ReadLine()) != null)
                        {
                            foreach (var item in filtro)
                            {
                                if (linha.ToUpper().Contains(item.ToUpper()))
                                {
                                    lblResultado.Text += string.Format("{0} \n", arquivo);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    //Caso tenha algum arquivo sem acesso passa para o próximo
                    break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Erro");
                }
            }

            if (lblResultado.Text == string.Empty)
            {
                MessageBox.Show("Sem dados!", "Mensagem");
            }
        }
        #endregion



        #region |Métodos Privados|        

        private void DesabilitarBotoes()
        {
            btnPesquisar.Enabled = false;
            btnCopy.Enabled = false;
            btnClean.Enabled = false;
        }

        private void HabilitarBotoes()
        {
            btnPesquisar.Enabled = true;
            btnCopy.Enabled = true;
            btnClean.Enabled = true;
        }
        #endregion

    }
}
