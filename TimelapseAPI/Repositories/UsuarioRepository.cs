using TimelapseAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") ?? throw new Exception("Connection string no encontrada");
        }

        // GET ALL
        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, email, contraseña FROM Usuario";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            Contraseña = reader.GetString(3)
                        });
                    }
                }
            }

            return usuarios;
        }

        // GET BY ID
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, email, contraseña FROM Usuario WHERE id_usuario = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Contraseña = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return null;
        }
        // GET ALL FILTERED + ORDER
        public async Task<List<Usuario>> GetAllFilteredAsync(string? nombre, string? email, string? orderBy, bool ascending)
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT id_usuario, nombre, email, contraseña FROM Usuario WHERE 1=1";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    query += " AND nombre LIKE @Nombre";
                    parameters.Add(new SqlParameter("@Nombre", $"%{nombre}%"));
                }

                if (!string.IsNullOrWhiteSpace(email))
                {
                    query += " AND email LIKE @Email";
                    parameters.Add(new SqlParameter("@Email", $"%{email}%"));
                }

                // Ordenación
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "id_usuario", "nombre", "email" };
                    var orderByLower = orderBy.ToLower();

                    if (validColumns.Contains(orderByLower))
                    {
                        var direction = ascending ? "ASC" : "DESC";
                        query += $" ORDER BY {orderByLower} {direction}";
                    }
                    else
                    {
                        query += " ORDER BY nombre ASC";
                    }
                }
                else
                {
                    query += " ORDER BY nombre ASC";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Contraseña = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return usuarios;
        }
    


        // CREATE
        public async Task CreateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Usuario (nombre, email, contraseña) VALUES (@Nombre, @Email, @Contraseña); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

                    // Obtener el ID generado
                    var result = await command.ExecuteScalarAsync();
                    usuario.IdUsuario = Convert.ToInt32(result);
                }
            }
        }

        // UPDATE
        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Usuario SET nombre=@Nombre, email=@Email, contraseña=@Contraseña WHERE id_usuario=@Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

                    var rows = await command.ExecuteNonQueryAsync();
                    if (rows == 0) return null;

                    return usuario;
                }
            }
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Usuario WHERE id_usuario=@Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rows = await command.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
    }
}