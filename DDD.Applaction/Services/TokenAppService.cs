using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.Common;
using DDD.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using XUCore.NetCore.Jwt;
using XUCore.NetCore.Jwt.Algorithms;
using XUCore.NetCore.Jwt.Builder;

namespace DDD.Applaction.AdminUsers.Services
{
    /// <summary>
    /// Token管理
    /// </summary>
    [JwtAuthorize]
    //[Authorize]
    public class TokenAppService : AppService, ITokenAppService
    {
        private readonly JwtOptions jwtOptions;
        public TokenAppService(IMediatorHandler bus, JwtOptions jwtOptions) : base(bus)
        {
            this.jwtOptions = jwtOptions;
        }

        [HttpPost]
        [JwtAllowAnonymous]
        public async Task<Result<string>> CreateAsync(CancellationToken cancellationToken)
        {
            var token = new JwtBuilder()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .WithSecret(jwtOptions.Secret)
               .JwtId(Id.GuidGenerator.Create())
               .Id(Id.LongGenerator.Create())
               .Account("XUCore")
               .NickName("Nigel")
               .VerifiedPhoneNumber("19173100454")
               .ExpirationTime(DateTime.UtcNow.AddMinutes(1))
               .Build();

            return Success(SubCode.Success, token);
        }

        [HttpGet]
        public async Task<Result<TokenDto>> VerifyAsync(CancellationToken cancellationToken)
        {
            var jwtid = Web.HttpContext.User.Identity.GetValue<Guid>(ClaimName.JwtId);
            var id = Web.HttpContext.User.Identity.GetValue<long>(ClaimName.Id);
            var account = Web.HttpContext.User.Identity.GetValue<string>(ClaimName.Account);
            var nickname = Web.HttpContext.User.Identity.GetValue<string>(ClaimName.NickName);
            var phone = Web.HttpContext.User.Identity.GetValue<string>(ClaimName.VerifiedPhoneNumber);
            var expirationtime = Web.HttpContext.User.Identity.GetValue<long>(ClaimName.ExpirationTime).ToDateTime();

            return Success(SubCode.Success,
                data: new TokenDto
                {
                    JwtId = jwtid,
                    Id = id,
                    Account = account,
                    NickName = nickname,
                    Phone = phone,
                    Expirationtime = expirationtime
                },
                message: "验证成功");
        }

        /*
         * 
         // 以下部分为微软官方提供的JWT

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<string>> CreateMsTokenAsync(CancellationToken cancellationToken)
        {
            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Name,"Nigel")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("",
               "",
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(1),
                creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Success(SubCode.Success, jwtToken);
        }

        [HttpGet]
        public async Task<Result<TokenDto>> VerifyMsTokenAsync(CancellationToken cancellationToken)
        {
            var nameIdentifier = Web.HttpContext.User.Identity.GetValue<long>(ClaimTypes.NameIdentifier);
            var name = Web.HttpContext.User.Identity.GetValue<string>(ClaimTypes.Name);

            return Success(SubCode.Success,
                data: new TokenDto
                {
                    Id = nameIdentifier,
                    NickName = name
                },
                message: "验证成功");
        }
        */
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
