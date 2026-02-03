using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public interface IContenidoService
    {
        Task<List<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<Contenido> CreateAsync(Contenido contenido);
        Task<Contenido?> UpdateAsync(Contenido contenido);
        Task<bool> DeleteAsync(int id);
    }
}