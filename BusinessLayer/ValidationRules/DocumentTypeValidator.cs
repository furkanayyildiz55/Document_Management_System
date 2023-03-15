using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class DocumentTypeValidator  : AbstractValidator<DocumentType>
    {
        public DocumentTypeValidator()

        {

            
            RuleFor(x => x.DocumentTypeName).NotEmpty().WithMessage("Belge Türü İsimi Boş Geçilemez")
                                     .Length(5, 100).WithMessage("Belge türü isimi en az 5, en fazla 100 karakter olmalıdır");

            RuleFor(x => x.DocumentTypeText).NotEmpty().WithMessage("Belge Türü Yazısı Boş Geçilemez")
                                        .Length(5, 500).WithMessage("Belge türü yazısı en az 5, en fazla 500 karakter olmalıdır");


            RuleFor(x => x.DocumentTypeNumSignature).NotEmpty().WithMessage("Belge Türü imza adedi boş geçilemez")
                .Must(x => x > 0 && x <= 3).WithMessage("En az 1, en fazla 3 imza bilgisi olabilir");


             RuleFor(x => x.DocumentTypeNumSignature.ToString()).Matches("^[0-9]*$").WithMessage("Sadece Sayısal İfade Giriniz");

            

        }
    }
}
