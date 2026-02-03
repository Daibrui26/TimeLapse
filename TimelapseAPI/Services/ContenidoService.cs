using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{

    public class ContenidoService : IContenidoService
    {
        private readonly IContenidoRepository _contenidoRepository;

        public ContenidoService(IContenidoRepository contenidoRepository)
        {
            _contenidoRepository = contenidoRepository;
        }

        public async Task<List<Contenido>> GetAllAsync()
        {
            return await _contenidoRepository.GetAllAsync();
        }

        public async Task<Contenido?> GetByIdAsync(int id)
        {
            return await _contenidoRepository.GetByIdAsync(id);
        }

        public async Task<Contenido> CreateAsync(Contenido contenido)
        {
            // Validaciones básicas antes de crear contenido
            if (string.IsNullOrWhiteSpace(contenido.Tipo))
                throw new ArgumentException("El tipo de contenido no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(contenido.ContenidoTexto))
                throw new ArgumentException("El contenido no puede estar vacío.");

            if (contenido.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            // Si no se asigna fecha, se pone la actual
            if (contenido.FechaSubida == default)
                contenido.FechaSubida = DateTime.UtcNow;

            return await _contenidoRepository.CreateAsync(contenido);
        }

        public async Task<Contenido?> UpdateAsync(Contenido contenido)
        {
            // Validaciones similares a CreateAsync
            if (string.IsNullOrWhiteSpace(contenido.Tipo))
                throw new ArgumentException("El tipo de contenido no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(contenido.ContenidoTexto))
                throw new ArgumentException("El contenido no puede estar vacío.");

            if (contenido.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            return await _contenidoRepository.UpdateAsync(contenido);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _contenidoRepository.DeleteAsync(id);
        }
    }
}