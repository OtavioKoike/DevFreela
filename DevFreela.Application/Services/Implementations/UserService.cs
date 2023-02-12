using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;

namespace DevFreela.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DevFreelaDbContext _dbContext;

        public UserService(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.birthDate, inputModel.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public UserViewModel GetById(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if(user == null)
                return null;
            
            return new UserViewModel(user.Id, user.FullName, user.Email, user.BirthDate);
        }

        public bool Login(LoginInputModel inputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == inputModel.Email && u.Password == inputModel.Password);
            
            if(user == null)
                return false;

            return true;
        }
    }
}