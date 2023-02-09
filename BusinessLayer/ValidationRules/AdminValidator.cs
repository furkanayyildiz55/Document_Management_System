using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AdminValidator : AbstractValidator<Admin>
    {
        public AdminValidator()
        {
            RuleFor(x => x.AdminName).NotEmpty().WithMessage("Admin Adı Boş Geçilemez");
            RuleFor(x => x.AdminSurmane).NotEmpty().WithMessage("Admin Soyadı Adı Boş Geçilemez");

            RuleFor(x => x.AdminName).MinimumLength(3).WithMessage("En az 3 karakter giriniz");


        }
    }
}
