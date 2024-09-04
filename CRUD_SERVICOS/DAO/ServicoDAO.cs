using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_SERVICOS.Models;
using MySql.Data.MySqlClient;

namespace CRUD_SERVICOS.DAO
{
	internal class ServicoDAO
	{
        public void Insert(Servicos servico)
        {
            try
            {
                string tempoEspera = servico.tempo.ToString("hh-mm-nn");
                string sql = $"INSERT INTO servicos(valor, descricao, tempo)" +
                    "VALUES(@Valor, @Descricao, @Tempo)";

                MySqlCommand comando = new MySqlCommand(sql, Conexao.Conectar());
                comando.Parameters.AddWithValue("@Valor", servico.valor);
                comando.Parameters.AddWithValue("@Descricao", servico.descricao);
                comando.Parameters.AddWithValue("@Tempo", servico.tempo);

                comando.ExecuteNonQuery();
                Console.WriteLine("Serviço cadastrado com sucesso! ");

                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


      




        public void Delete(Servicos servico)
            {
                try
                {
                    string sql = "DELETE FROM servicos WHERE id_servico = @id_servicos";
                    MySqlCommand comando = new MySqlCommand(sql, Conexao.Conectar());
                    comando.Parameters.AddWithValue("@id_servicos", servico.id_servico);
                    comando.ExecuteNonQuery();
                    Console.WriteLine("Servico excluido com sucesso!");
                    Conexao.FecharConexao();
                }
                catch (Exception ex)
                {

                    throw new Exception($"Erro ao excluir o serviço {ex.Message}");
                }
                finally
                {
                     Conexao.FecharConexao();
                }

            }



        public List<Servicos> List()
        {
            List<Servicos> servicos = new List<Servicos>();

            try
            {
                var sql = "SELECT * FROM servicos ";
                MySqlCommand comando = new MySqlCommand(sql, Conexao.Conectar());
                using (MySqlDataReader dr = comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Servicos servico = new Servicos();
                        servico.id_servico = dr.GetInt32("id_servico");
                        servico.valor = dr.GetDouble("valor");
                        servico.descricao = dr.GetString("descricao");
                        TimeSpan timeSpan = dr.GetTimeSpan("tempo");
                        servico.tempo = TimeOnly.FromTimeSpan(timeSpan);

                        servicos.Add(servico);
                    }
                }
                Conexao.FecharConexao();  
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao cadastrar serviço! {ex.Message}");
            }
            return servicos;
        }

        public void Update (Servicos servicos)
        {
            try
            {
                string sql = "UPDATE Servicos SET descricao = @descricao, tempo = @tempo, valor = @valor WHERE id_servico = @id_servicos";
                MySqlCommand comando = new MySqlCommand(sql, Conexao.Conectar());
                comando.Parameters.AddWithValue("descricao", servicos.descricao);
                comando.Parameters.AddWithValue("tempo", servicos.tempo);
                comando.Parameters.AddWithValue("valor", servicos.valor);
                comando.Parameters.AddWithValue("id_servicos", servicos.id_servico);
                comando.ExecuteNonQuery();
                Console.WriteLine("Serviço atualizado com sucesso!");
                Conexao.FecharConexao();
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao atualizar cliente {ex.Message}");
            }




        }

        public void MenuOp()
        {
            int opcao;

            do
            {

                ServicoDAO adao = new ServicoDAO();

                Console.WriteLine("Escolha uma operação:");
                Console.WriteLine("1 - Inserir serviço");
                Console.WriteLine("2 - Deletar serviço");
                Console.WriteLine("3 - Listar serviços");
                Console.WriteLine("4 - Atualizar serviço");
                Console.WriteLine("0 - Para sair do programa");
                Console.Write("Opção: ");
                opcao = int.Parse(Console.ReadLine());

                List<Servicos> todos = adao.List();


                switch (opcao)
                {
                    case 1:
                        try
                        {
                            Servicos novoSer = new Servicos();

                            Console.WriteLine("Insira o valor do serviço:");
                            novoSer.valor = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Insira a descrição do serviço:");
                            novoSer.descricao = Convert.ToString(Console.ReadLine());

                            Console.WriteLine("Insira o tempo de demora do serviço (HH:MM:SS):");
                            string inserirTempo = Console.ReadLine();

                            if (TimeOnly.TryParse(inserirTempo, out TimeOnly tempo))
                            {
                                novoSer.tempo = tempo;

                                adao.Insert(novoSer);
                            }
                            else
                            {
                                Console.WriteLine("Formato de horas inválido! Por favor, insira no formato HH:MM:SS.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Formato de horas inválido!" + ex.Message);
                        }

                        break;

                    case 2:

                        Console.WriteLine("Digite qual ID deseja deletar:");

                        int a = Convert.ToInt32(Console.ReadLine());

                        Servicos deletarServic = todos.First(x => x.id_servico == a);

                        adao.Delete(deletarServic);

                        break;

                    case 3:
                        foreach (Servicos servico in todos)
                        {
                            Console.WriteLine($"ID: {servico.id_servico}, Descrição: {servico.descricao}, Preço: {servico.valor}, Tempo: {servico.tempo}");
                        }
                        break;

                    case 4:
                        Console.WriteLine("Digite qual ID deseja atualizar:");

                        int b = Convert.ToInt32(Console.ReadLine());

                        Servicos atualizarServ = todos.First(x => x.id_servico == b);

                        if (atualizarServ != null)
                        {
                            try
                            {
                                Console.WriteLine($"Valor atual: {atualizarServ.valor}. Insira o novo valor (ou deixe em branco para manter o atual):");
                                string valor = Console.ReadLine();
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    atualizarServ.valor = Convert.ToDouble(valor);
                                }

                                Console.WriteLine($"Descrição atual: {atualizarServ.descricao}. Insira a nova descrição (ou deixe em branco para manter a atual):");
                                string descricao = Console.ReadLine();
                                if (!string.IsNullOrEmpty(descricao))
                                {
                                    atualizarServ.descricao = descricao;
                                }

                                Console.WriteLine($"Tempo atual: {atualizarServ.tempo}. Insira o novo tempo (HH:MM:SS) ou deixe em branco para manter o atual:");
                                string inserirTempo = Console.ReadLine();
                                if (!string.IsNullOrEmpty(inserirTempo) && TimeOnly.TryParse(inserirTempo, out TimeOnly tempo))
                                {
                                    atualizarServ.tempo = tempo;
                                }

                                adao.Update(atualizarServ);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro ao atualizar o serviço: " + ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Serviço não encontrado. Verifique o ID informado.");
                        }

                        break;

                    case 0:

                        break;
                    default:
                        break;

                }


            } while (opcao != 0);
        }
    }

}   


    

