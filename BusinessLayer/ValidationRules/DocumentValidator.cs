using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class DocumentValidator : AbstractValidator<Document>
    {

        public DocumentValidator()
        {

            RuleFor(x => x.DocumentTypeID).NotNull().WithMessage("Belge Türü Boş Geçilemez !");

            RuleFor(x => x.DocumentAlternativeText).Length(0, 350).WithMessage("Alternatif metin maksimum 350 karakter uzunluğunda olabilir !");


        }

    }


}
