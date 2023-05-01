using FluentValidation;

namespace Application.Common.DTOs.Registration;

public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
{
    public RegistrationDTOValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Недійсний email.");

        RuleFor(dto => dto.Username)
            .NotEmpty().WithMessage("Ім'я користувача обов'язкове.")
            .Length(3, 50).WithMessage("Ім'я користувача має містити від 3 до 50 символів.");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Пароль обов'язковий.")
            .MinimumLength(8).WithMessage("Пароль повинен містити принаймні 8 символів.");

        RuleFor(dto => dto.passwordConfirmation)
            .Equal(dto => dto.Password).WithMessage("Підтвердження пароля має збігатися з паролем.");
    }
}