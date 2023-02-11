using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.StudentName).NotEmpty().WithMessage("İsim Boş Geçilemez")
                         .Length(5, 30).WithMessage("Ad en az 5, en fazla 30 karakter olmalıdır");

            RuleFor(x => x.StudentName).NotEmpty().WithMessage("Soyisim Boş Geçilemez")
                                        .Length(5, 30).WithMessage("Soyad en az 5, en fazla 30 karakter olmalıdır");

            RuleFor(x => x.StudentNo).NotEmpty().WithMessage("Numara Boş Geçilemez")
                                     .Length(1, 30).WithMessage("Soyad en az 5, en fazla 30 karakter olmalıdır")
                                     .Matches("^[0-9]*$").WithMessage("Sadece Sayısal İfade Giriniz");
                                    


            //RuleFor(x => x.StudentMail).NotEmpty().WithMessage("Mail Boş Geçilemez")
            //                         .Length(5, 30).WithMessage("Mail en az 5, en fazla 30 karakter olmalıdır")
            //                         .EmailAddress().WithMessage("Geçerli bir Mail adresi giriniz");

            RuleFor(x => x.StudentProgram).NotEmpty().WithMessage("Bölüm Boş Geçilemez")
                                    .Length(2, 30).WithMessage("İş en az 2 en fazla 30 karakter olmalıdır");
        }
    }
}
