using System;
using System.Threading.Tasks;
using Common;
using LanguageExt;
using Orleans.EventSourcing;
using static LanguageExt.Prelude;

namespace Grains
{
    public class Registration : JournaledGrain<RegistrationState, IRegistrationEvent>, IRegistration
    {
        public async Task<Either<Error, RegistrationCreated>> Create(CreateRegistration command)
        {
            if (State.Created)
                return Left<Error, RegistrationCreated>(Error.New("Registration already exists"));

            var e = new RegistrationCreated
            {
                Email = command.Email,
                FirstName = command.FirstName
            };

            RaiseEvent(e);
            await SendWelcomeEmail(command);

            return Right<Error, RegistrationCreated>(e);
        }

        private async Task SendWelcomeEmail(CreateRegistration command)
        {
            var provider = GetStreamProvider("SMSProvider");
            var stream = provider.GetStream<INotification>(Guid.Empty, Streams.Notifications);

            await stream.OnNextAsync(new Notification
            {
                Subject = $"Welcome, {command.FirstName}",
                Body = "Some email body"
            });
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