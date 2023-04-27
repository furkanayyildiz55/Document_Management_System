using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator(Student student)
        {

            if (student.StudentUniversityRegistered == false)
            {
                RuleFor(x => x.StudentNo).Null().WithMessage("Yozgat Bozok Ü. Öğrencisi Olmayan Kişiler No Giremez.");

                RuleFor(x => x.StudentProgram).Null().WithMessage("Yozgat Bozok Ü. Öğrencisi Olmayan Kişiler Bölüm Giremez");


                RuleFor(x => x.StudentMail).NotEmpty().WithMessage("Mail Boş Geçilemez")
                                         .Length(5, 40).WithMessage("Mail en az 5, en fazla 40 karakter olmalıdır")
                                         .EmailAddress().WithMessage("Geçerli bir Mail adresi giriniz");


            }
            else if(student.StudentUniversityRegistered == true)
            {

                RuleFor(x => x.StudentNo).NotEmpty().WithMessage("Numara Boş Geçilemez")
                         .Length(1, 30).WithMessage("Numara en az 1, en fazla 30 karakter olmalıdır")
                         .Matches("^[0-9]*$").WithMessage("Sadece Sayısal İfade Giriniz");

                RuleFor(x => x.StudentProgram).NotEmpty().WithMessage("Bölüm Boş Geçilemez")
                                              .Length(2, 30).WithMessage("Bölüm en az 2 en fazla 30 karakter olmalıdır");


                RuleFor(x => x.StudentMail).NotEmpty().WithMessage("Mail Boş Geçilemez")
                                         .Length(5, 40).WithMessage("Mail en az 5, en fazla 40 karakter olmalıdır")
                                         .EmailAddress().WithMessage("Geçerli bir Mail adresi giriniz");

                if (student.StudentMail != null)
                {
                 RuleFor(x => x.StudentMail).Must(x => x.Contains("@ogr.bozok.edu.tr")).WithMessage("ogr.bozok.edu.tr uzantılı mail girilmelidir.");
                }




            }
            else
            {
                RuleFor(x => x.StudentUniversityRegistered).NotEmpty().WithMessage("Okul kayıt bilgisi boş geçilemez");
            }



            RuleFor(x => x.StudentSurname).NotEmpty().WithMessage("Soyisim Boş Geçilemez")
                                                 .Length(2, 30).WithMessage("Soyisim en az 2, en fazla 30 karakter olmalıdır");


            RuleFor(x => x.StudentName).NotEmpty().WithMessage("İsim Boş Geçilemez")
                         .Length(2, 30).WithMessage("İsim en az 2, en fazla 30 karakter olmalıdır");



            RuleFor(x => x.StudentPassword).NotEmpty().WithMessage("Şifre boş geçilemez")
                .Length(6, 50).WithMessage("Şifre en az 2 en fazla 50 karakter olmalıdır");

        }



    }
}
