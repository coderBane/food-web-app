namespace Foody.Admin.Rest.Interfaces;

public interface IInquiryService : IRestService<Inquiry>
{
    void Flag(ref Inquiry inquiry);
}

