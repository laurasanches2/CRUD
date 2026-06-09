using System.Data;
using System.Windows;
using CRUD.Modelos;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Feed : Window
{
    public Feed()
    {
        InitializeComponent();
        CarregarPosts_QuandoIniciar();
    }

    private void CarregarPosts_QuandoIniciar()
    {
        List<Postagem> listasPostagems = [];

        const string query =
            "SELECT p.id,p.conteudo,p.curtidas, p.postado_em, u.nome, u.username FROM postagens p INNER JOIN usuarios u ON p.usuario_id = u.id";

        using var conexao = new MySqlConnection(App.StringConexao);

        using var comando = new MySqlCommand(query, conexao);

        try
        {
            conexao.Open();

            var leitor = comando.ExecuteReader();

            if (!leitor.HasRows)
            {
                MessageBox.Show("Nenhuma postagem foi encontrada");
                return;
            }

            while (leitor.Read())
            {
                var post = new Postagem
                {
                    Id = leitor.GetInt32("Id"),
                    Conteudo = leitor.GetString("Conteudo"),
                    Curtidas = leitor.GetInt32("Curtidas"),
                    Postado_em = leitor.GetDateTime("Postado_em"),
                    Usuario = new Usuario
                    {
                        Nome = leitor.GetString("Nome"),
                        Username = leitor.GetString("Username")
                    }
                };
                listasPostagems.Add(post);
            }
            ItemsControlfeed.ItemsSource = listasPostagems;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}