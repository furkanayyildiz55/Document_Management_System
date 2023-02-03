using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class AdminValidator : AbstractValidator<Admin>
    {   
        public AdminValidator()
        {
            RuleFor(x => x.AdminName).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");
            RuleFor(x => x.AdminSurmane).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");
            RuleFor(x => x.AdminJob).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");
            RuleFor(x => x.AdminMail).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");
          //  RuleFor(x => x.AdminSignatureImage).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");
            RuleFor(x => x.AdminAuthorization).NotEmpty().WithMessage("Lütfen bu alanı doldurunuz !");

        }

    }
}
