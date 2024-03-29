﻿using EntityLayer.Concrete;
using FluentValidation;


namespace BusinessLayer.ValidationRules
{
    public class AdminValidator : AbstractValidator<Admin>
    {
        public AdminValidator()
        {
            

            RuleFor(x => x.AdminName).NotEmpty().WithMessage("İsim Boş Geçilemez")
                                     .Length(5, 30).WithMessage("Ad en az 5, en fazla 30 karakter olmalıdır");

            RuleFor(x => x.AdminSurmane).NotEmpty().WithMessage("Soyadı Boş Geçilemez")
                                        .Length(5, 30).WithMessage("Soyad en az 5, en fazla 30 karakter olmalıdır");


            RuleFor(x => x.AdminMail).NotEmpty().WithMessage("Mail Boş Geçilemez")
                                     .Length(5, 30).WithMessage("Mail en az 5, en fazla 30 karakter olmalıdır")
                                     .EmailAddress().WithMessage("Geçerli bir Mail adresi giriniz");

            RuleFor(x => x.AdminJob).NotEmpty().WithMessage("İş Boş Geçilemez")
                                    .Length(5, 30).WithMessage("İş en az 5 en fazla 30 karakter olmalıdır");


            RuleFor(x => x.AdminPassword).NotEmpty().WithMessage("Şifre boş geçilemez")
                                         .Length(6, 100).WithMessage("Şifre en az 6 en fazla 100 karakter olmalıdır");



        }
    }
}
