namespace MagicVilla_API.Modelos.Dto
{
	public class LoginResponseDTO
	{
		//public Usuario Usuario { get; set; }
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }

		public string Rol { get; set; }

	}
}
