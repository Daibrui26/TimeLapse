using System.Data.SqlClient;
using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TimelapseAPI.Repositories
{
    public interface IUsuarioCapsulaRepository
    {
        Task<List<UsuarioCapsula>> GetAllAsync();
        Task<UsuarioCapsula?> GetByIdAsync(int id);
        Task<UsuarioCapsula> CreateAsync(UsuarioCapsula usuarioCapsula);
        Task<UsuarioCapsula?> UpdateAsync(UsuarioCapsula usuarioCapsula);
        Task<bool> DeleteAsync(int id);
    }
}