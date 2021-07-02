using fabiostefani.io.Catalogo.Application.Services;
using fabiostefani.io.Catalogo.Data;
using fabiostefani.io.Catalogo.Data.Repository;
using fabiostefani.io.Catalogo.Domain;
using fabiostefani.io.Core.DomainObjects;
using fabiostefani.io.Core.Messages;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Application.Queries;
using fabiostefani.io.Vendas.Data;
using fabiostefani.io.Vendas.Data.Repository;
using fabiostefani.io.Vendas.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace fabiostefani.io.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Catalogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            
            // Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<VendasContext>();

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
        }
    }
}