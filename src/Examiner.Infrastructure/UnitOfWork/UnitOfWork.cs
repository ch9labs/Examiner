using Examiner.Domain.Entities.Authentication;
using Examiner.Domain.Entities.Notifications.Emails;
using Examiner.Domain.Entities.Users;
using Examiner.Infrastructure.Contexts;
using Examiner.Infrastructure.Repositories;
using Examiner.Infrastructure.Repositories.Interfaces;
using Examiner.Infrastructure.UnitOfWork.Interfaces;

namespace Examiner.Infrastructure.UnitOfWork;

/// <summary>
/// Implements contract for unit of work
/// </summary>

public class UnitOfWork : IUnitOfWork
{
    private readonly ExaminerContext _dbContext;
    private UserRepository<User>? _userRepository;
    private CodeVerificationRepository<CodeVerification>? _codeVerificationRepository;
    // private CodeVerificationHistoryRepository<CodeVerificationHistory>? _codeVerificationHistoryRepository;
    private KickboxVerificationRepository<KickboxVerification>? _kickboxVerificationRepository;

    public UnitOfWork(ExaminerContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository<User>(_dbContext));

    public ICodeVerificationRepository CodeVerificationRepository => 
    _codeVerificationRepository ?? 
    (_codeVerificationRepository = new CodeVerificationRepository<CodeVerification>(_dbContext));

    // public ICodeVerificationHistoryRepository CodeVerificationHistoryRepository => 
    // _codeVerificationHistoryRepository ?? 
    // (_codeVerificationHistoryRepository = new CodeVerificationHistoryRepository<CodeVerificationHistory>(_dbContext));
    
    public IKickboxVerificationRepository KickboxVerificationRepository => 
    _kickboxVerificationRepository ?? 
    (_kickboxVerificationRepository = new KickboxVerificationRepository<KickboxVerification>(_dbContext));

    public async Task CompleteAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    private bool _disposed;

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);

        // Take this object off the finalization queue to prevent
        // finalization code for this object from executing a second time
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cleans up any resources being used
    /// </summary>
    /// <param name="disposing"> Whether or not we are disposing </param>
    /// <returns><see cref="ValueTask"/></returns>

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // dispose managed resources
                await _dbContext.DisposeAsync();
            }

            // dispose any unmanaged resources here

            _disposed = true;
        }
    }
}