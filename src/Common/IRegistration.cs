using System;
using System.Threading.Tasks;
using Orleans;
using LanguageExt;

namespace Common
{
    public interface IRegistrationEvent{}

    public class Error : NewType<Error, string>{
        public Error(string value) : base(value) { }
    }

    public class CreateRegistration
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
    }

    public class RegistrationCreated : IRegistrationEvent
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
    }

    public interface IRegistration : IGrainWithStringKey
    {
        Task<Either<Error, RegistrationCreated>> Create(CreateRegistration command);
    }
}