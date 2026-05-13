using AutoMapper;
using user_api.cs.Models;
using user_api.cs.Repositories;
using user_api.cs.Shared;

namespace user_api.cs.Services;

public abstract class GenericService<TEntity, TDto, TCreateDto, TUpdateDto>(
     IGenericRepository<TEntity> repository,
     IMapper mapper) : IGenericService<TDto, TCreateDto, TUpdateDto>
     where TEntity : BaseEntity
     where TDto : class
     where TCreateDto : class
     where TUpdateDto : class
{
     public virtual async Task<GenericResponse<IEnumerable<TDto>>> GetAllAsync()
     {
          var entities = await repository.GetAllAsync();
          return GenericResponse<IEnumerable<TDto>>.Ok(mapper.Map<IEnumerable<TDto>>(entities));
     }

     public virtual async Task<GenericResponse<TDto>> GetByIdAsync(Guid id)
     {
          var entity = await repository.GetByIdAsync(id);
          return entity is null ? GenericResponse<TDto>.NotFound() : GenericResponse<TDto>.Ok(mapper.Map<TDto>(entity));
     }

     public virtual async Task<GenericResponse<TDto>> CreateAsync(TCreateDto dto)
     {
          var entity = mapper.Map<TEntity>(dto);
          var created = await repository.CreateAsync(entity);
          return GenericResponse<TDto>.Created(mapper.Map<TDto>(created));
     }

     public virtual async Task<GenericResponse<TDto>> UpdateByIdAsync(Guid id, TUpdateDto dto)
     {
          var entity = await repository.GetByIdAsync(id);
          if (entity is null) return GenericResponse<TDto>.NotFound();
          mapper.Map(dto, entity);
          var updated = await repository.UpdateAsync(entity);
          return GenericResponse<TDto>.Ok(mapper.Map<TDto>(updated));
     }

     public virtual async Task<GenericResponse<bool>> DeleteByIdAsync(Guid id)
     {
          var deleted = await repository.DeleteAsync(id);
          return deleted ? GenericResponse<bool>.Ok(true, "Recurso excluído com sucesso.") : GenericResponse<bool>.NotFound();
     }
}