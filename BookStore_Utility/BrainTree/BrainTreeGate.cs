using Braintree;
using Microsoft.Extensions.Options;

namespace BookStore_Utility.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        public BrainTreeSettings Options { get; set; }
        private IBraintreeGateway braintreeGateway { get; set; }
        public BrainTreeGate(IOptions<BrainTreeSettings> options)
        {
            Options = options.Value;
        }

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(Options.Enviroment, Options.MerchantId, Options.PublicKey, Options.PrivateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            if (braintreeGateway == null) 
            {
                braintreeGateway = CreateGateway();
            }
            return braintreeGateway;
        }
    }
}
