using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GestionFicha.Models.DTO;

namespace GestionFicha.Utils
{
    public static class Paginador
    {
        public static async Task<PaginadorDTO> ProcesarPaginador<T>(IQueryable<T> query, ParametrosPaginadorDTO parametrosPaginador, Func<T, BaseDTO> func)
        {
            var lista = new List<BaseDTO>();
            var count = await query.CountAsync();
            var metadata = ObtenerMetadataPaginador(count, parametrosPaginador);

            var objects = metadata.elementosPorPagina == 0 ? await query.ToListAsync() : await query.Skip((metadata.paginaActual - 1) * metadata.elementosPorPagina).Take(metadata.elementosPorPagina).ToListAsync();

            // Por alguna razón el Count devuelve 1 aún cuando no hay ningún elemento
            // En estos casos, el único elemento de la lista es null
            if (objects.Count == 1 && objects[0] == null)
            {
                metadata.cantidadTotal = 0;
                metadata.totalDePaginas = 0;
            }
            else
            {
                foreach (var person in objects)
                {
                    lista.Add(func(person));
                }
            }

            return new PaginadorDTO(lista, metadata);
        }

        private static PaginadorMetaData ObtenerMetadataPaginador(int count, ParametrosPaginadorDTO parametrosPaginador)
        {
            var metadata = new PaginadorMetaData
            {
                cantidadTotal = count,
                elementosPorPagina = (int)parametrosPaginador.elementosPorPagina
            };

            // Si el tamaño de página es 0, significa que el usuario quiere todos los elementos
            if (metadata.elementosPorPagina == 0)
            {
                metadata.paginaActual = 1;
                metadata.totalDePaginas = 1;
                metadata.paginaAnterior = false;
                metadata.paginaSiguiente = false;
            }
            else
            {
                metadata.paginaActual = (int)parametrosPaginador.paginaActual;
                metadata.totalDePaginas = (int)Math.Ceiling(metadata.cantidadTotal / (double)metadata.elementosPorPagina);
                metadata.paginaAnterior = metadata.paginaActual > 1;
                metadata.paginaSiguiente = metadata.paginaActual < metadata.totalDePaginas;
            }

            return metadata;
        }
    }
}