using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public class ContenidoRepository : IContenidoRepository
    {
        private readonly string _connectionString;

        public ContenidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TimelapseDB") 
                ?? throw new Exception("Connection string not found");
        }

        public async Task<List<Contenido>> GetAllAsync()
        {
            var list = new List<Contenido>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_contenido, tipo, contenido, fecha_subida, id_capsula FROM Contenido";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Contenido
                {
                    IdContenido = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    ContenidoTexto = reader.GetString(2),
                    FechaSubida = reader.GetDateTime(3),
                    IdCapsula = reader.GetInt32(4)
                });
            }

            return list;
        }

        public async Task<Contenido?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT id_contenido, tipo, contenido, fecha_subida, id_capsula FROM Contenido WHERE id_contenido=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Contenido
                {
                    IdContenido = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    ContenidoTexto = reader.GetString(2),
                    FechaSubida = reader.GetDateTime(3),
                    IdCapsula = reader.GetInt32(4)
                };
            }

            return null;
        }

        public async Task<Contenido> CreateAsync(Contenido contenido)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                INSERT INTO Contenido (tipo, contenido, fecha_subida, id_capsula)
                OUTPUT INSERTED.id_contenido
                VALUES (@tipo, @contenido, @fechaSubida, @idCapsula)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@tipo", contenido.Tipo);
            cmd.Parameters.AddWithValue("@contenido", contenido.ContenidoTexto);
            cmd.Parameters.AddWithValue("@fechaSubida", contenido.FechaSubida);
            cmd.Parameters.AddWithValue("@idCapsula", contenido.IdCapsula);

            contenido.IdContenido = (int)await cmd.ExecuteScalarAsync();
            return contenido;
        }

        public async Task<Contenido?> UpdateAsync(Contenido contenido)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                UPDATE Contenido
                SET tipo=@tipo, contenido=@contenido, fecha_subida=@fechaSubida, id_capsula=@idCapsula
                WHERE id_contenido=@id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", contenido.IdContenido);
            cmd.Parameters.AddWithValue("@tipo", contenido.Tipo);
            cmd.Parameters.AddWithValue("@contenido", contenido.ContenidoTexto);
            cmd.Parameters.AddWithValue("@fechaSubida", contenido.FechaSubida);
            cmd.Parameters.AddWithValue("@idCapsula", contenido.IdCapsula);

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0 ? contenido : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Contenido WHERE id_contenido=@id";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}