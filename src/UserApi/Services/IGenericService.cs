using UserApi.Shared;

namespace UserApi.Services;

public interface IGenericService<TDto, in TCreateDto, in TUpdateDto>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<GenericResponse<IEnumerable<TDto>>> GetAllAsync();
    Task<GenericResponse<TDto>> GetByIdAsync(Guid id);
    Task<GenericResponse<TDto>> CreateAsync(TCreateDto dto);
    Task<GenericResponse<TDto>> UpdateByIdAsync(Guid id, TUpdateDto dto);
    Task<GenericResponse<bool>> DeleteByIdAsync(Guid id);
    Task<GenericResponse<bool>> DisableByIdAsync(Guid id);
}