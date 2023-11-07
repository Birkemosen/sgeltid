using sgeltid.Modules.Users.Domain;

namespace Modules.Users.Application;

public record UserCreated(int Id);
public record CreateUserCommand();

public class CreateUserHandler
{
    public static UserCreated Handle(CreateUserCommand command)
    {
        var user = new User();
        //context.Add(user);
        return new UserCreated(1);
    }
}

public class UserCreatedHandler
{
    public static void Handle(UserCreatedHandler created)
    {

    }
}