using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.Common;
using DDD.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Swagger;

namespace DDD.Applaction.AdminUsers.Services
{
    /// <summary>
    /// Token管理
    /// </summary>
    [Authorize]
    public class TokenAppService : AppService, ITokenAppService
    {
        private readonly JwtSettings jwtSettings;
        public TokenAppService(IMediatorHandler bus, IStringLocalizer<SubCode> localizer, JwtSettings jwtSettings) : base(bus, localizer)
        {
            this.jwtSettings = jwtSettings;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<string>> CreateTokenAsync(CancellationToken cancellationToken)
        {
            var claims = new Claim[]{
                new Claim("id","1"),
                new Claim("name","Nigel"),
                new Claim("account","account")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(1),
                creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            Web.HttpContext.SigninToSwagger(jwtToken);

            return Success(SubCode.Success, jwtToken);
        }

        [HttpGet]
        public async Task<Result<TokenDto>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            var id = Web.HttpContext.User.Identity.GetValue<long>("id");
            var name = Web.HttpContext.User.Identity.GetValue<string>("name");
            var account = Web.HttpContext.User.Identity.GetValue<string>("account");

            return Success(SubCode.Success,
                data: new TokenDto
                {
                    Id = id,
                    Account = account,
                    NickName = name
                },
                message: "验证成功");
        }
    }

    public class TokenDto
    {
        public Guid JwtId { get; set; }
        public long Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public DateTime Expirationtime { get; set; }
    }
}
