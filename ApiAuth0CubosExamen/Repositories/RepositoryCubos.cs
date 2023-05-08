using ApiAuth0CubosExamen.Data;
using ApiAuth0CubosExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAuth0CubosExamen.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> FindCuboAsync(string marca)
        {
             var consulta = from datos in context.Cubos
                             where datos.Marca == marca
                            select datos;

            return await consulta.ToListAsync();
        }

        private int GetMAxIdCubo()
        {
            return this.context.Cubos.Max(x => x.IdCubo) + 1;
        }

        private int GetMAxIdUsuario()
        {
            return this.context.Usuarios.Max(x => x.IdUsuario) + 1;
        }

        private int GetMaxIdPedido()
        {
            return this.context.Pedidos.Max(x => x.IdPedido) + 1;
        }

        public async Task NewCuboAsync(string nombre, string marca, string imagen, int precio)
        {
            Cubo cubo = new Cubo
            {
                IdCubo = this.GetMAxIdCubo(),
                Nombre = nombre,
                Marca = marca,
                Imagen = imagen,
                Precio = precio
            };

            this.context.Cubos.Add(cubo);
            await this.context.SaveChangesAsync();
        }

        public async Task NewUsuarioAsync(string nombre, string email, string pass, string imagen)
        {
            Usuario usuario = new Usuario
            {
                IdUsuario = this.GetMAxIdUsuario(),
                Nombre = nombre,
                Email = email,
                Pass = pass,
                Imagen = imagen
            };

            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> ExisteCuboAsync(string nombre, string pass)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == nombre &&
            x.Pass == pass);
        }

        public async Task<List<Pedido>> GetPedidosAsync(int id)
        {
            return await this.context.Pedidos.Where(x=>x.IdUsuario==id).ToListAsync();
        }

        public async Task NewPedidoAsync(int idcubo, int idusuario, DateTime fechapedido)
        {
            Pedido pedido = new Pedido
            {
                IdPedido = this.GetMaxIdPedido(),
                IdCubo = idcubo,
                IdUsuario = idusuario,
                FechaPedido = fechapedido
            };

            this.context.Pedidos.Add(pedido);
            await this.context.SaveChangesAsync();
        }
    }
}
