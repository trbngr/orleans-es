using Common;

namespace Grains
{
    public class RegistrationState
    {
        public bool Created { get; set; }

        public void Apply(RegistrationCreated _) => Created = true;
    }
}