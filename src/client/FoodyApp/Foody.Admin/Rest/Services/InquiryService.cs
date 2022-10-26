namespace Foody.Admin.Rest.Services;

public class InquiryService : RestService<Inquiry>, IInquiryService
{
    public InquiryService(IHttpsClientHandlerService httpsClientHandler) : base(httpsClientHandler)
    {
        url = Address.Inquiry.List;
    }

    public void Flag(ref Inquiry inquiry) => inquiry.Flagged = !inquiry.Flagged;
}

