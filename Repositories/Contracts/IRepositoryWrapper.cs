namespace Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }
    }
}