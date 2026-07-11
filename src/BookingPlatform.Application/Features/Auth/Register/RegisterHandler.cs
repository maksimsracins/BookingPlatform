using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Abstractions.Security;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using BookingPlatform.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Auth.Register;

public sealed class RegisterHandler
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterHandler(
        IBookingDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Users
            .AnyAsync(x => x.Email == command.Email, cancellationToken);

        if (exists)
        {
            throw new EmailAlreadyExistException(command.Email);
        }

        var user = new User(
            Guid.NewGuid(),
            command.Email,
            _passwordHasher.Hash(command.Password));

        var business = BookingPlatform.Core.Entities.Business.Create(command.BusinessName);

        var businessUser = new BusinessUser(
            business.Id,
            user.Id,
            BusinessRole.Owner);

        _context.Users.Add(user);
        _context.Businesses.Add(business);
        _context.BusinessUsers.Add(businessUser);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(
            user.Id,
            business.Id);
    }
}