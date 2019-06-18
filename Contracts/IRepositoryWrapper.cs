namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }
    }
}