﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Roles;
using FHotel.Services.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<User>().GetAll()
                                            .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<UserResponse> Get(Guid id)
        {
            try
            {
                User user = null;
                user = await _unitOfWork.Repository<User>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.UserId == id)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<User, UserResponse>(user);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> Create(UserRequest request)
        {
            try
            {
                var user = _mapper.Map<UserRequest, User>(request);
                user.UserId = Guid.NewGuid();
                await _unitOfWork.Repository<User>().InsertAsync(user);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<User, UserResponse>(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> Delete(Guid id)
        {
            try
            {
                User user = null;
                user = _unitOfWork.Repository<User>()
                    .Find(p => p.UserId == id);
                if (user == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(user.UserId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<User, UserResponse>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserResponse> Update(Guid id, UserRequest request)
        {
            try
            {
                User user = _unitOfWork.Repository<User>()
                            .Find(x => x.UserId == id);
                if (user == null)
                {
                    throw new Exception();
                }
                user = _mapper.Map(request, user);

                await _unitOfWork.Repository<User>().UpdateDetached(user);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<User, UserResponse>(user);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
