using DDD.Applaction;
using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.Common;
using DDD.Applaction.Dtos;
using DDD.Domain.Core;
using DDD.Domain.Sys.AdminMenu;
using DDD.Domain.Sys.AdminUser;
using DDD.Domain.Sys.LoginRecord;
using DDD.Domain.Sys.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
using XUCore.Paging;
using XUCore.Timing;

namespace DDD.Application.Services
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Login)]
    public class AdminLoginAppService : AppService, IAdminLoginAppService
    {
        private const string userId = "_userid";
        private const string userName = "_username";
        private readonly JwtSettings jwtSettings;

        public AdminLoginAppService(IMediatorHandler bus, JwtSettings jwtSettings) : base(bus)
        {
            this.jwtSettings = jwtSettings;
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> Login([FromBody] AdminUserLoginCommand command, CancellationToken cancellationToken)
        {
            var user = await bus.SendCommand(command);

            var claims = new List<Claim> {
                new Claim(userId, user.Id.ToString()),
                new Claim(userName, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.Now;
            var expires = now.AddDays(1);

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                now,
                expires,
                creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            Web.HttpContext.SigninToSwagger(jwtToken);

            return Success(SubCode.Success, new LoginTokenDto
            {
                Token = jwtToken,
                Expires = expires.ToTimeStamp(false)
            });
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            var id = Web.HttpContext.User.Identity.GetValue<long>(userId);
            var name = Web.HttpContext.User.Identity.GetValue<string>(userName);

            return Success(SubCode.Success, data: name);
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Exists")]
        public async Task<Result<bool>> GetPermissionAsync([FromQuery] PermissionQueryExists command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenu command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenuExpress command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return Success(SubCode.Success, res);
        }

        #endregion


        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<Result<IList<LoginRecordDto>>> GetRecordAsync([FromQuery] LoginRecordQueryList command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordAsync([FromQuery] LoginRecordQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return Success(SubCode.Success, res);
        }

        #endregion
    }
}
