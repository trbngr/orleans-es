using System;
using System.Threading.Tasks;
using Common;
using LanguageExt;
using Orleans.EventSourcing;
using static LanguageExt.Prelude;

namespace Grains
{
    public class RegistrationState
    {
        public bool Created { get; set; }

        public void Apply(RegistrationCreated _) => Created = true;
    }

    public class Registration : JournaledGrain<RegistrationState, IRegistrationEvent>, IRegistration
    {
        public Task<Either<Error, RegistrationCreated>> Create(CreateRegistration command)
        {
            if (State.Created)
                return Left<Error, RegistrationCreated>(Error.New("Registration already exists")).AsTask();

            var e = new RegistrationCreated
            {
                Email = command.Email,
                FirstName = command.FirstName
            };

            RaiseEvent(e);

            return Right<Error, RegistrationCreated>(e).AsTask();
        }

        protected override void OnStateChanged()
        {
            foreach (var registrationEvent in UnconfirmedEvents)
            {
                Console.Out.WriteLine(registrationEvent.GetType());
            }
        }
    }
}