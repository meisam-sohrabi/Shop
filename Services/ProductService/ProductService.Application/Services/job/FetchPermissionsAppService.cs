using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Permission;
using ProductService.ApplicationContract.DTO.UserPermission;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using ProductService.InfrastructureContract.Interfaces.Query.Generic;
using Quartz;

namespace ProductService.Application.Services.job
{
    public class FetchPermissionsAppService : IJob
    {
        private readonly IGenericQueryRepository<LocalPermissionEntity> _permissionQuery;
        private readonly IGenericQueryRepository<LocalUserPermissionEntity> _userPermissionQuery;
        private readonly IGenericCommandRepository<LocalPermissionEntity> _permissionCommand;
        private readonly IGenericCommandRepository<LocalUserPermissionEntity> _userPermissionCommand;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        public FetchPermissionsAppService(IGenericQueryRepository<LocalPermissionEntity> permissionQuery
            , IGenericQueryRepository<LocalUserPermissionEntity> userPermissionQuery,
            IGenericCommandRepository<LocalPermissionEntity> permissionCommand,
            IGenericCommandRepository<LocalUserPermissionEntity> userPermissionCommand
            ,IUnitOfWork unitOfWork
            , HttpClient httpClient)
        {
            _permissionQuery = permissionQuery;
            _userPermissionQuery = userPermissionQuery;
            _permissionCommand = permissionCommand;
            _userPermissionCommand = userPermissionCommand;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var permissionAddress = "https://localhost:7276/Permission/GetAll";
            var userPermissionAddress = "https://localhost:7276/Permission/GetAllRawUserPermissions";
            var apiKey = "skdfj349werhch88@#@#";
            var permissions = await _permissionQuery.GetQueryable().ToListAsync();
            var userPermissions = await _userPermissionQuery.GetQueryable().ToListAsync();
            if (!permissions.Any() && !userPermissions.Any())
            {

                var permisisonResponse = await GetAsync<List<PermissionDto>>(permissionAddress, apiKey);
                var userPermissionResponse = await GetAsync<List<UserPermissionDto>>(userPermissionAddress, apiKey);
                if (userPermissionResponse.Success && userPermissionResponse.Success)
                {
                    var permissionEntities = permisisonResponse.Data!.Select(c => new LocalPermissionEntity
                    {
                        Id = c.Id,
                        Action = c.Action,
                        Resource = c.Resource,
                        Description = c.Description
                    }).ToList();
                    await _permissionCommand.AddRangeAsync(permissionEntities);

                    var userPermissionEntities = userPermissionResponse.Data!.Select(c => new LocalUserPermissionEntity
                    {
                        PermissionId = c.PermissionId,
                        UserId = c.UserId
                    }).ToList();
                    await _userPermissionCommand.AddRangeAsync(userPermissionEntities);
                   await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        #region Request
        private async Task<BaseResponseDto<T>> GetAsync<T>(string address, string key)
        {
            var output = new BaseResponseDto<T>();
            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Add("Api-Key", key);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                output.Message = $"خطای سرور: {response.StatusCode}";
                output.StatusCode = response.StatusCode;
                output.Success = false;
                return output;
            }
            var result = await response.Content.ReadAsStringAsync();
            var Deserilized = JsonConvert.DeserializeObject<BaseResponseDto<T>>(result);
            if (Deserilized != null)
            {
                return Deserilized;
            }
            output.Message = "خطا در دریافت اطلاعات";
            output.Success = false;
            output.StatusCode = System.Net.HttpStatusCode.NotFound;
            return output;

        }
        #endregion



    }
}
