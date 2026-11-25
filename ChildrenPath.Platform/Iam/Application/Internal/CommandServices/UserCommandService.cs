using ChildrenPath.Platform.Iam.Application.CommandServices;
using ChildrenPath.Platform.Iam.Application.Internal.OutboundServices;
using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using ChildrenPath.Platform.Iam.Domain.Model.Commands;
using ChildrenPath.Platform.Iam.Domain.Model.Errors;
using ChildrenPath.Platform.Iam.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork) : IUserCommandService
{
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username, cancellationToken);

        // Validamos que el usuario exista y que la contraseña coincida
        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(IamErrors.InvalidCredentials);

        var token = tokenService.GenerateToken(user);

        return Result<(User user, string token)>.Success((user, token));
    }

    public async Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        // Validamos que el nombre de usuario no esté en uso
        if (await userRepository.ExistsByUsernameAsync(command.Username, cancellationToken))
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken);

        // Hasheamos la contraseña (Aquí se usa el servicio de BCrypt)
        var hashedPassword = hashingService.HashPassword(command.Password);
        
        // Creamos el Agregado
        var user = new User(command.Username, hashedPassword);

        // Guardamos
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        
        return Result<User>.Success(user);
    }
}