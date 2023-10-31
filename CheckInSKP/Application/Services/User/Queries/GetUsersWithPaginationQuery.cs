﻿using AutoMapper;
using CheckInSKP.Application.Services.User.Queries.Dtos;
using CheckInSKP.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.Services.User.Queries
{
    public record GetUsersWithPaginationQuery : IRequest<IEnumerable<UserDto>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersWithPaginationQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.UserAggregate.User> users = await _userRepository.GetAllWithPaginationAsync(request.PageNumber, request.PageSize);
            IEnumerable<UserDto> userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDtos;
        }
    }
}