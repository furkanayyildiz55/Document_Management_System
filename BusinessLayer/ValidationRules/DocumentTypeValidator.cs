using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class DocumentTypeValidator  : AbstractValidator<DocumentType>
    {
        public DocumentTypeValidator()
        {
            RuleFor(x => x.DocumentTypeName).NotEmpty().WithMessage("Dosya Türü Adını Doldurunuz");
            RuleFor(x => x.DocumentTypeText).NotEmpty().WithMessage("Dosya Türü Yazısını Doldurunuz");
            RuleFor(x => x.DocumentTypeNumSignature).NotEmpty().WithMessage("Dosya Türü İmza Adedini Doldurunuz");
            RuleFor(x => x.DocumentCreateDate).NotEmpty().WithMessage("Dosya Türü Oluşturma Tarihini Doldurunuz");
            //RuleFor(x => x.DocumentTypeStatus).NotEmpty().WithMessage("Dosya Türü Yazısını Doldurunuz");

        }
    }
}
