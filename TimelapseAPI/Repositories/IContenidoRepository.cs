using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public interface IContenidoRepository
    {
        Task<List<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<Contenido> CreateAsync(Contenido contenido);
        Task<Contenido?> UpdateAsync(Contenido contenido);
        Task<bool> DeleteAsync(int id);

        // Búsqueda filtrada con ordenación
        
    }
}