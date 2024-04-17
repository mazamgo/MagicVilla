using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Repositorio.IRepositorio
{
	public interface IUsuarioRepositorio
	{
		//Metodos para conocer si el usr existe.
		bool IsUsuarioUnico(string userName);

		//Metodos para el logueo.
		Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);

		//Metodo para el registro de nuevo usuario.
		//Task<Usuario> Registrar(RegistroRequestDTO registroRequestDTO);
        Task<UsuarioDto> Registrar(RegistroRequestDTO registroRequestDTO);

    }
}
