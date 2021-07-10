using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using fabiostefani.io.Core.DomainObjects;
using fabiostefani.io.Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fabiostefani.io.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediator _mediatorHandler;

        protected Guid ClienteId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediator mediatorHandler, 
                                 IHttpContextAccessor httpContextAccessor)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;

            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return;

            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            ClienteId = Guid.Parse(claim.Value);
        }

        protected bool OperacaoValida()
        {
            return !_notifications.TemNotificacao();
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediatorHandler.Publish(new DomainNotification(codigo, mensagem));
        }

        protected new IActionResult Response(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.ObterNotificacoes().Select(n => n.Value)
            });
        }
    }
}