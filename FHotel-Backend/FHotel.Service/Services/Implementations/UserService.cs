using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Users;
using FHotel.Service.Validators.HotelResgistrationValidator;
using FHotel.Service.Validators.UserValidator;
using FHotel.Services.DTOs.Roles;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IRoleService _roleService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleService = roleService;
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
                    .Include(x => x.Role)
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

        public async Task<UserResponse> Create(UserCreateRequest request)
        {
            var validator = new UserCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Check dupplicated IdentificationNumber
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.IdentificationNumber == request.IdentificationNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("IdentificationNumber", "Identification number already exists."));
            }

            // Check dupplicated Email
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }

            // Check dupplicated PhoneNumber (nếu cần)
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.PhoneNumber == request.PhoneNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("PhoneNumber", "Phone number already exists."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var validaterole = await _roleService.Get((Guid)request.RoleId);
                var user = _mapper.Map<UserCreateRequest, User>(request);
                user.UserId = Guid.NewGuid();
                user.CreatedDate = localTime;
                user.IsActive = false;
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

        public async Task<UserResponse> Login(UserLoginRequest request)
        {
            // Validate the login request using FluentValidation
            var validator = new UserLoginRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If validation fails, throw a validation exception with the validation failures
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Retrieve the user based on email and password
            var user = _unitOfWork.Repository<User>()
                .Find(x => x.Email == request.Email && x.Password == request.Password);

            // Return null if the user is not found or if credentials are incorrect
            if (user == null)
            {
                return null;
            }

            // Return null if the user's email or password is null (additional precaution)
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            // Retrieve additional user details
            var userRes = await Get(user.UserId);

            return userRes;
        }

        public async Task<UserResponse> ActiveAccount(string email)
        {
            var accounts = await GetAll();
            foreach (var account in accounts)
            {
                if (account.Email!.Equals(email) && account.IsActive == false)
                {
                    User user = _unitOfWork.Repository<User>()
                            .Find(x => x.UserId == account.UserId);
                    user.IsActive = true;
                    await _unitOfWork.Repository<User>().UpdateDetached(user);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<User, UserResponse>(user);
                }
            }
            return null;
        }


    }
}
