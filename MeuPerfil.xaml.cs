using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private Usuario UsuarioAtual;

    public MeuPerfil(Usuario usuario)
    {
        InitializeComponent();
        UsuarioAtual = usuario;
        TxtNome.Text = UsuarioAtual.Nome;
        TxtEmail.Text = UsuarioAtual.Email;
        TxtUsuarioPerfil.Text = UsuarioAtual.Username;
    }

    private void BtnSalvar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtNome.Text))
        {
            MessageBox.Show("Campo nome não preenchido");
            TxtNome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtEmail.Text))
        {
            MessageBox.Show("Campo email não preenchido");
            TxtEmail.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtUsuarioPerfil.Text))
        {
            MessageBox.Show("Campo username não preenchido");
            TxtUsuarioPerfil.Focus();
            return;
        }

        var senhafoiAlterada = !string.IsNullOrWhiteSpace(TxtSenhaPerfil.Password);

        UsuarioAtual.Username = TxtUsuarioPerfil.Text;
        UsuarioAtual.Nome = TxtNome.Text;
        UsuarioAtual.Email = TxtEmail.Text;
        if (senhafoiAlterada) UsuarioAtual.senha = TxtSenhaPerfil.Password;

        using var conexao = new MySqlConnection(App.StringConexao);
        var query = "UPDATE usuarios SET username = @username, email = @email, nome = @nome";

        if (senhafoiAlterada) query += " , senha = @senha";

        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@username", UsuarioAtual.Username);
        comando.Parameters.AddWithValue("@email", UsuarioAtual.Email);
        comando.Parameters.AddWithValue("@nome", UsuarioAtual.Nome);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);
        if (senhafoiAlterada) comando.Parameters.AddWithValue("@senha", UsuarioAtual.senha);

        try

        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)

            {
                MessageBox.Show("Cadastro atualizado com sucesso!");
            }
            else
            {
                MessageBox.Show("Erro ao atualizar o cadastro! ");
            }
        }
        catch
            (Exception exception)
        {
            MessageBox.Show("Erro de DB:");
        }
    }

    private void Btndeletarperfil_OnClick(object sender, RoutedEventArgs e)
    {
        var resultado = MessageBox.Show("Voce tem certeza que deseja excluir o perfil?", "Cofirmação de exclusão.",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (resultado == MessageBoxResult.No) return;
        const string query = "DELETE FROM usuarios WHERE id = @id";
        using var conexao = new MySqlConnection(App.StringConexao);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);
        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Perfil deletado com sucesso! ");
                this.Close();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}