using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class DocumentVerificationValidator : AbstractValidator<Document>
    {

        public DocumentVerificationValidator()
        {

            RuleFor(x => x.DocumentVerificationCode).NotEmpty().WithMessage("Belge kodu boş geçilemez !")
                .Length(12, 12).WithMessage("Belge kodu 12 karakterdir. Lütfen kontrol ediniz");


        }

    }
}
