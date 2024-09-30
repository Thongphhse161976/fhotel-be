using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Roles;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoleResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Role>().GetAll()
                                            .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoleResponse> Get(Guid id)
        {
            try
            {
                Role role = null;
                role = await _unitOfWork.Repository<Role>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoleId == id)
                    .FirstOrDefaultAsync();

                if (role == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Role, RoleResponse>(role);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoleResponse> Create(RoleRequest request)
        {
            try
            {
                var role = _mapper.Map<RoleRequest, Role>(request);
                role.RoleId = Guid.NewGuid();
                await _unitOfWork.Repository<Role>().InsertAsync(role);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Role, RoleResponse>(role);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoleResponse> Delete(Guid id)
        {
            try
            {
                Role role = null;
                role = _unitOfWork.Repository<Role>()
                    .Find(p => p.RoleId == id);
                if (role == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(role.RoleId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Role, RoleResponse>(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoleResponse> Update(Guid id, RoleRequest request)
        {
            try
            {
                Role role = _unitOfWork.Repository<Role>()
                            .Find(x => x.RoleId == id);
                if (role == null)
                {
                    throw new Exception();
                }
                role = _mapper.Map(request, role);

                await _unitOfWork.Repository<Role>().UpdateDetached(role);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Role, RoleResponse>(role);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
